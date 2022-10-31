using LinkNode = Magnuth.NodeLink<FollowPoint>.Node<FollowPoint>;
using UnityEngine;
using Magnuth;

public class Follower
{
    private Follower _parent = null;
    private Follower _child = null;

    private bool _grounded = true;
    private float _groundTimer = 0f;
    private int _groundLayer = 1 << 8;

    private float _animDefSpeed = 0f;
    private Animator _animator = null;
    private static readonly int _animJumpHash = Animator.StringToHash("OnJump");

    private float _maxSpeed = 0f;
    private float _padding = 0f;
    private Transform _transform = null;

    // New follower pathfinding
    private int _maxPoints = 50;
    private LinkNode _target = null;
    private static NodeLink<FollowPoint> _points = null;

// PROPERTIES

    public bool Grounded { 
        get => _grounded; 
        set => _grounded = value;
    }

    public Follower Parent => _parent;
    public Follower Child => _child;
    public LinkNode Target => _target;
    public FollowPoint Point => _target?.Data;
    public Transform Transform => _transform;
    public GameObject GameObject => _transform?.gameObject;

// INITIALISATION

    /// <summary>
    /// Initialises this follower
    /// </summary>
    /// <param name="parent">This follower's parent</param>
    /// <param name="follower">This follower's rigidbody</param>
    /// <param name="padding">This follower's collider radius</param>
    /// <param name="maxSpeed">The player's maximum movement speed</param>
    public Follower(Follower parent, Transform follower, float padding, float maxSpeed){
        _transform = follower;
        _parent = parent;
        _padding = padding;
        _maxSpeed = maxSpeed;
        
        _animator = follower.GetComponentInChildren<Animator>();
        _animDefSpeed = _animator.speed;

        if (parent == null) return; // root
        InitTarget(_transform.position);
    }

    /// <summary>
    /// Initialises the path on player start
    /// </summary>
    public void InitPath(float speed, bool grounded, Vector3 position){
        _points ??= new NodeLink<FollowPoint>();
        var point = new FollowPoint(speed, grounded, position);
        
        _points.AddLast(point);
        _target = _points.Last;
    }

    /// <summary>
    /// Initialises the follow target
    /// </summary>
    public void InitTarget(Vector3 position){
        var node = _parent?.Target;

        while (node?.Left != null){
            node = node.Left;

            if (!node.Data.Occupied)
                break;
        }

        _target = node;
        _target.Data.Occupied = true;
    }

// MANAGEMENT

    /// <summary>
    /// Adds a new parent to this follower
    /// </summary>
    public void SetParent(Follower parent){
        _parent = parent;
    }

    /// <summary>
    /// Adds a new child to this follower
    /// </summary>
    public void SetChild(Follower child){
        _child = child;
    }

    /// <summary>
    /// Repositions this follower to be behind parent
    /// </summary>
    public void Reposition(Follower parent){
        Remove();

        var tfm = parent.Transform;
        var position = tfm.position + (-tfm.forward * _padding);
        
        _transform.position = position;
        _transform.rotation = tfm.rotation;

        SetParent(parent);

        // New follower pathfinding
        InitTarget(position);
    }

    /// <summary>
    /// Removes this follower from the chain
    /// </summary>
    public void Remove(){
        _child?.SetParent(_parent);
        _parent?.SetChild(_child);

        // New follower pathfinding
        _target.Data.Occupied = false;
    }

// OLD MOVEMENT

    /// <summary>
    /// Moves this follower towards the target position
    /// </summary>
    public void Move(Vector3 target, float deltaTime, float speed){
        var position = _transform.position;

        _grounded = IsGrounded(position, deltaTime);
        _animator.SetBool(_animJumpHash, !_grounded);

        var difference = (target - position);
        if (difference == Vector3.zero) return;

        var differenceXZ = difference;
        differenceXZ.y = 0;

        var rotation = Quaternion.LookRotation(differenceXZ);
        _transform.rotation = rotation;

        var followSpeed = Mathf.Max(speed, difference.magnitude);
        var direction = difference.normalized;

        var velocity = direction * (followSpeed * deltaTime);
        _transform.Translate(velocity, Space.World);

        var animMultiplier = (followSpeed / _maxSpeed) * 3f;
        _animator.speed = _grounded ? 
            _animDefSpeed * animMultiplier : 
            _animDefSpeed;

        var offset = -direction * _padding;
        _child?.Move(position + offset, deltaTime, speed);
    }

    /// <summary>
    /// Returns true if follower is grounded
    /// </summary>
    private bool IsGrounded(Vector3 position, float deltaTime){
        _groundTimer += deltaTime;

        if (_groundTimer < 0.05f)
            return _grounded;

        var offset = _transform.forward * 0.5f;
        var p1 = position + offset;
        var p2 = position - offset;

        var grounded = Physics.CheckCapsule(
            p1, p2, 0.25f, _groundLayer
        );

        _groundTimer = 0;
        return grounded;
    }

// PATHFINDING

    /// <summary>
    /// Updates the path on player movement
    /// </summary>
    public void UpdatePath(float speed, bool grounded, Vector3 position){
        var point = _target.Data;
        var diff = (point.Position - position);

        if (diff.magnitude < _padding &&
            grounded == point.Grounded)
            return;

        var next = new FollowPoint(
            speed, grounded, position
        );

        _points.AddLast(next);
        _target = _points.Last;

        if (_points.Count > _maxPoints)
            _points.RemoveFirst();
    }

    /// <summary>
    /// Updates the follow target on follower movement
    /// </summary>
    private void UpdateTarget(float magnitude, float speed, float deltaTime){
        if (magnitude > speed * deltaTime) return;

        var next = _target.Right;
        if (next == null) return;

        if (next.Data.Occupied) return;
        next.Data.Occupied = true;
        
        _target.Data.Occupied = false;
        _target = next;
    }


    /// <summary>
    /// Moves the followers towards its target point
    /// </summary>
    public void Move(float deltaTime){
        var position = _transform.position;
        var point = _target.Data;

        var diffToPoint = point.Position - position;
        if (diffToPoint == Vector3.zero) return;

        var distToPoint = diffToPoint.magnitude;

        FaceTarget(diffToPoint, deltaTime);
        MoveToTarget(point, position, diffToPoint, distToPoint, out var speed, deltaTime);
        UpdateTarget(distToPoint, speed, deltaTime);
        Animate(point, speed);

        _child?.Move(deltaTime);
    }

    /// <summary>
    /// Rotates this follower towards target direction
    /// </summary>
    private void FaceTarget(Vector3 difference, float deltaTime){
        difference.y = 0f;

        if (difference == Vector3.zero) return;
        var rotation = Quaternion.LookRotation(difference);

        _transform.rotation = Quaternion.RotateTowards(
            _transform.rotation, rotation, 270f * deltaTime
        );
    }

    /// <summary>
    /// Moves this follower towards target position
    /// </summary>
    private void MoveToTarget(FollowPoint point, Vector3 position,
    Vector3 diffToPoint, float distToPoint, out float speed,  float deltaTime){
        var pPoint = _parent.Point;

        var pDiff = pPoint.Position - point.Position;
        var pMag  = pDiff.magnitude;
        
        var pDist = pMag - (_padding * 2f);
        var catchup = pDist / _padding;

        speed = Mathf.Max(point.Speed, distToPoint);
        speed = Mathf.Max(speed, speed + catchup);

        var direction = diffToPoint.normalized;
        var velocity = direction * (speed * deltaTime);

        _transform.Translate(velocity, Space.World);
    }

    /// <summary>
    /// Animates this follower based on speed and current point
    /// </summary>
    private void Animate(FollowPoint point, float speed){
        var animMultiplier = (speed / _maxSpeed);
        _animator.SetBool(_animJumpHash, !point.Grounded);

        _animator.speed = point.Grounded ?
            _animDefSpeed * animMultiplier :
            _animDefSpeed;
    }

// DEBUGGING
#if UNITY_EDITOR

    private static readonly Color s_defPointColour = 
        Color.Lerp(Color.red, Color.yellow, 0.5f);

    private static readonly Color s_airPointColour = 
        Color.Lerp(Color.blue, Color.magenta, 0.5f);

    public void DrawPoints(){
        var point = _points.Last;
        
        for (int i = _points.Count - 1; i > 0; i--){
            if (point.Left == null) break;

            Gizmos.color = point.Data.Grounded ? s_defPointColour : s_airPointColour;
            Gizmos.DrawSphere(point.Data.Position, 0.1f);

            point = point.Left;
        }
    }

#endif
}
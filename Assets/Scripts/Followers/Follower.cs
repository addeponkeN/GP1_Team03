using LinkNode = Magnuth.NodeLink<FollowPoint>.Node<FollowPoint>;
using UnityEngine;
using Magnuth;
using System.Collections.Generic;

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
    private int _maxPoints = 30;
    private LinkNode _target = null;
    private static NodeLink<FollowPoint> _points = null;

// PROPERTIES

    public bool Grounded { 
        get => _grounded; 
        set => _grounded = value;
    }
    public Follower Parent => _parent;
    public Follower Child => _child;
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

        // New follower pathfinding
        InitTarget(_transform.position);
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

// MOVEMENT

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

        var offset = Vector3.up * 0.3f;
        var p1 = position + offset;
        var p2 = position + - offset;

        var grounded = Physics.Linecast(p1, p2, _groundLayer);

        _groundTimer = 0;
        return grounded;
    }

// NEW FOLLOWER PATHFINDING

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
    /// Assigns the closest target to the current position
    /// </summary>
    public void InitTarget(Vector3 position){
        var node = _points?.Last;
        if (node == null) return;

        var mindist = float.MaxValue;
        var point = node.Data;
        var closest = node;

        for (int i = _points.Count - 1; i > 0; i--){
            node = node?.Left;
            if (node == null) break;

            point = node.Data;
            if (point.Occupied) continue;

            var dist = (point.Position - position).magnitude;
            if (dist > mindist) break;

            mindist = dist;
            closest = node;
        }

        _target = closest;
        _target.Data.Occupied = true;
    }

    /// <summary>
    /// Moves the followers towards its target point
    /// </summary>
    public void Move(float deltaTime){
        var position = _transform.position;
        var point = _target.Data;

        var difference = (point.Position - position);
        if (difference == Vector3.zero) return;

        // Rotate towards the target position
        var differenceXZ = difference;
        differenceXZ.y = 0f;

        var rotation = Quaternion.LookRotation(differenceXZ);
        _transform.rotation = Quaternion.RotateTowards(
            _transform.rotation, rotation, 270f * deltaTime
        );

        // Move towards target position
        var magnitude = difference.magnitude;
        var direction = difference.normalized;
        var speed = Mathf.Max(point.Speed, magnitude);
        // ADD CATCH UP TO PLAYER?

        var velocity = direction * (speed * deltaTime);
        _transform.Translate(velocity, Space.World);

        // Adapt follower animation
        var animMultiplier = (speed / _maxSpeed);
        _animator.SetBool(_animJumpHash, !point.Grounded);

        _animator.speed = _grounded ?
            _animDefSpeed * animMultiplier :
            _animDefSpeed;

        // Update current target and continue the chain
        UpdateTarget(magnitude);
        _child?.Move(deltaTime);
    }

    /// <summary>
    /// Updates the target point on follower movement
    /// </summary>
    private void UpdateTarget(float magnitude){
        if (magnitude > _padding * 0.5f) return;

        var next = _target.Right;
        if (next == null) return;

        if (next.Data.Occupied) return;
        next.Data.Occupied = true;
        
        _target.Data.Occupied = false;
        _target = next;
    }

// DEBUGGING
#if UNITY_EDITOR
    public void DrawPoints(){
        var point = _points.Last;
        
        for (int i = _points.Count - 1; i > 0; i--){
            if (point.Left == null) break;

            Gizmos.DrawSphere(point.Data.Position, 0.1f);
            point = point.Left;
        }
    }
#endif
}
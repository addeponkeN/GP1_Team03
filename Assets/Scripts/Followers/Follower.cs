using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using static Barmetler.RoadSystem.RoadLinkTool;

public class Follower
{
    private Follower _parent = null;
    private Follower _child = null;

    private bool _grounded = true;
    private float _groundTimer = 0f;
    private int _groundLayer = 1 << 8;

    private float _animDefSpeed = 0f;
    private int _animJumpHash = Animator.StringToHash("OnJump");
    private Animator _animator = null;

    private float _maxSpeed = 0f;
    private float _padding = 0f;
    private Transform _transform = null;

    // Unfinished
    private Vector3 _target = Vector3.zero;
    private static LinkedList<Vector3> _points = null;

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

        _points ??= new LinkedList<Vector3>();
        _target = _transform.position;
        Debug.Log(_points.Count);
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
    }

    /// <summary>
    /// Removes this follower from the chain
    /// </summary>
    public void Remove(){
        _child?.SetParent(_parent);
        _parent?.SetChild(_child);
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
        var p2 = position + -offset;

        var grounded = Physics.Linecast(p1, p2, _groundLayer);

        _groundTimer = 0;
        return grounded;
    }

// UNFINISHED PATHFINDING

    /// <summary>
    /// Moves followers along the player path
    /// </summary>
    private void Move(float speed, float deltaTime){
        var position = _transform.position;
        var difference = (_target - position);
        var magnitude = difference.magnitude;

        var differenceXZ = difference;
        differenceXZ.y = 0;

        if (magnitude != 0)
        {
            var rotation = Quaternion.LookRotation(differenceXZ);
            _transform.rotation = rotation;
        }

        var followSpeed = Mathf.Max(speed, magnitude);
        var direction = difference.normalized;

        var velocity = direction * (followSpeed * deltaTime);
        _transform.Translate(velocity, Space.World);

        var animMultiplier = (followSpeed / _maxSpeed) * 3f;
        _animator.speed = _grounded ?
            _animDefSpeed * animMultiplier :
            _animDefSpeed;

        var offset = -direction * _padding;
        _child?.Move(position + offset, deltaTime, speed);


        UpdateTarget(magnitude);
    }

    /// <summary>
    /// Update the current points target
    /// </summary>
    private void UpdateTarget(float distance){
        if (distance > 0.1f) return;
        
        var point = _points.First;
        if(point == null) return;

        while (point.Value != _target){
            point = point.Next;
        }

        _target = point.Next.Value;
    }

    /// <summary>
    /// Update the points list
    /// </summary>
    public void UpdatePoints(float padding){
        var position = _transform.position;
        var difference = (_target - position).magnitude;

        if (difference < padding) return;
        _points.AddLast(position);

        if (_points.Count > 50)
            _points.RemoveFirst();

        _target = position;
    }

    /// <summary>
    /// Draws the points in the scene
    /// </summary>
    public void DrawPoints(){
        //Debug.Log(_points.Count);
        if (_points.Count == 0) return;
        var point = _points.First;

        for (int i = 0; i < _points.Count - 1; i++){
            Gizmos.DrawSphere(point.Value, 0.1f);
            point = point.Next;
        }

    }
}
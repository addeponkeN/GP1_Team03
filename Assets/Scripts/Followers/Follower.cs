using UnityEngine;

public class Follower
{
    private Follower _parent = null;
    private Follower _child = null;

    private float _radius = 0f;
    private float _maxSpeed = 0f;
    private float _defAnimSpeed = 0f;
    private Transform _transform = null;
    private Animator _animator = null;

// PROPERTIES

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
    /// <param name="radius">This follower's collider radius</param>
    /// <param name="maxSpeed">The player's maximum movement speed</param>
    public Follower(Follower parent, Transform follower, float radius, float maxSpeed){
        _animator = follower.GetComponentInChildren<Animator>();
        _transform = follower;
        _parent = parent;
        _radius = radius;

        _maxSpeed = maxSpeed;
        _defAnimSpeed = _animator.speed;
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
        var position = tfm.position + (-tfm.forward * _radius);
        
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
    public void Move(Vector3 target, float deltaTime){
        var position = _transform.position;

        var difference = (target - position);
        if (difference == Vector3.zero) return;

        var rotation = Quaternion.LookRotation(difference);
        _transform.rotation = rotation;

        var speed = difference.magnitude;
        speed += speed * (speed / _maxSpeed);

        var direction = difference.normalized;
        var velocity = direction * (speed * deltaTime);
        _transform.Translate(velocity, Space.World);

        var multiplier = (speed / _maxSpeed) * 3f;
        _animator.speed = _defAnimSpeed * multiplier;

        var offset = -direction * _radius;
        _child?.Move(position + offset, deltaTime);
    }
}
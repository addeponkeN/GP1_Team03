using UnityEngine;

public class Follower
{
    private Follower _parent = null;
    private Follower _child = null;

    private float _radius = 0f;
    private Rigidbody _rigidbody = null;

// PROPERTIES

    public Follower Parent => _parent;
    public Follower Child => _child;
    public Rigidbody RigidBody => _rigidbody;
    public Transform Transform => _rigidbody?.transform;
    public GameObject GameObject => _rigidbody?.gameObject;

// INITIALISATION

    /// <summary>
    /// Initialises this follower
    /// </summary>
    /// <param name="parent">This follower's parent</param>
    /// <param name="follower">This follower's rigidbody</param>
    /// <param name="radius">This follower's collider radius</param>
    public Follower(Follower parent, Rigidbody follower, float radius){
        _parent = parent;
        _rigidbody = follower;
        _radius = radius;
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
    public void Move(Vector3 target, float speed, float deltaTime){
        var positon = _rigidbody.position;

        var difference = (target - positon);
        if (difference.magnitude < _radius * 0.5f) return;

        var rotation = Quaternion.LookRotation(difference);
        _rigidbody.MoveRotation(rotation);

        var direction = difference.normalized;
        var velocity = direction * (speed * deltaTime);
        _rigidbody.MovePosition(positon + velocity);

        var offset = -direction * _radius;
        _child?.Move(positon + offset, speed, deltaTime);
    }
}
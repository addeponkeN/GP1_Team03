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
    public void AddParent(Follower parent){
        _parent = parent;
    }

    /// <summary>
    /// Adds a new child to this follower
    /// </summary>
    public void AddChild(Follower child){
        _child = child;
    }

    /// <summary>
    /// Removes this follower from the chain
    /// </summary>
    public void Remove(){
        _parent.AddChild(_child);
        _child.AddParent(_parent);
    }

// MOVEMENT

    /// <summary>
    /// Moves this follower towards the target position
    /// </summary>
    public void Move(Vector3 target, float speed, float deltaTime){
        var positon = _rigidbody.position;
        var direction = (target - positon);

        var velocity = direction * (speed * deltaTime);
        _rigidbody.MovePosition(positon + velocity);

        var offset = -direction.normalized * _radius;
        _child?.Move(positon + offset, speed, deltaTime);
    }
}
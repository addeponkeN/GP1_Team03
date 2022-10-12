using SF = UnityEngine.SerializeField;
using UnityEngine;
using Jellybeans.Updates;
using PlayerControllers.Controllers;
using Unity.VisualScripting;

[RequireComponent(typeof(Player))]
public class PlayerFollowers : MonoBehaviour
{
    [SF] private float _followPadding = 1f;
    [SF] private UpdateManager _update = null;

    private int _followCount = 0;
    private Follower _child = null;
    private MovementController _move = null;

// INITIALISATION

    private void Start(){
        var controller = GetComponent<Player>().ControllerManager;
        _move = controller.GetController<MovementController>();
    }

    /// <summary>
    /// Subscribes to fixed update
    /// </summary>
    private void OnEnable(){
        _update.Subscribe(OnFixedUpdate, UpdateType.FixedUpdate);
    }

    /// <summary>
    /// Unsubscribes to fixed update
    /// </summary>
    private void OnDisable(){
        _update.Unsubscribe(OnFixedUpdate, UpdateType.FixedUpdate);
    }

// MANAGEMENT

    /// <summary>
    /// Adds a new follower to the player
    /// </summary>
    public void Add(Rigidbody follower){
        Follower parent = _child;

        while (parent?.Child != null){
            parent = parent.Child;
        }

        var collider = follower.GetComponent<SphereCollider>();
        var follow = new Follower(parent, follower, collider.radius + _followPadding);

        if (parent == null) _child = follow;
        else parent.SetChild(follow);

        _followCount++;
    }

    /// <summary>
    /// Adds a group of followers to the player
    /// </summary>
    public void Add(Rigidbody[] followers){
        for (int i = 0; i < followers.Length; i++){
            Add(followers[i]);
        }
    }

    /// <summary>
    /// Removes number of player followers
    /// </summary>
    public void Remove(int count){
        count = Mathf.Min(_followCount, count);
        Remove(_child, count);
    }

    /// <summary>
    /// Recursively remove followers for count
    /// </summary>
    private void Remove(Follower follower, int count){
        if (count < 1) return;

        follower.Remove();
        Remove(follower.Child, --count);
        
        _followCount--;
    }

// MOVEMENT

    /// <summary>
    /// On update manager fixed update event
    /// </summary>
    private void OnFixedUpdate(float fixedDeltaTime){
        var position = transform.position + (-transform.forward * _followPadding);
        _child?.Move(position, _move.Speed, fixedDeltaTime);
    }
}
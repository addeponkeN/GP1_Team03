using SF = UnityEngine.SerializeField;
using UnityEngine;
using Jellybeans.Updates;
using PlayerControllers.Controllers;

[RequireComponent(typeof(Player))]
public class PlayerFollowers : MonoBehaviour
{
    [SF] private UpdateManager _update = null;

    private MovementController _move = null;
    private Follower _child = null;

// INITIALISATION

    private void Start(){
        var controller = GetComponent<Player>().ControllerManager;
        _move = controller.GetController<MovementController>();
        if (_move == null) Debug.Log("Null");
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
    public void AddFollower(Rigidbody follower){
        Follower parent = _child;

        while (parent?.Child != null){
            parent = parent.Child;
        }

        var collider = follower.GetComponent<SphereCollider>();
        var follow = new Follower(parent, follower, collider.radius);

        if (parent == null) _child = follow;
        else parent.AddChild(follow);
    }

    /// <summary>
    /// Adds a group of followers to the player
    /// </summary>
    public void AddFollowers(Rigidbody[] followers){
        for (int i = 0; i < followers.Length; i++){
            AddFollower(followers[i]);
        }
    }

// MOVEMENT

    /// <summary>
    /// On update manager fixed update event
    /// </summary>
    private void OnFixedUpdate(float fixedDeltaTime){
        _child?.Move(transform.position, _move.Speed, fixedDeltaTime);
    }
}
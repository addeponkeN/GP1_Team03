using SF = UnityEngine.SerializeField;
using UnityEngine;
using Jellybeans.Updates;
using PlayerControllers.Controllers;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class PlayerFollowers : MonoBehaviour
{
    [SF] private float _followPadding = 1f;
    [SF] private UpdateManager _update = null;

    private int _followCount = 0;
    private Follower _root = null;
    private MovementController _move = null;

    [Space, SF] private UnityEvent _onPickup = new();
    [Space, SF] private UnityEvent _onLost = new();

// PROPERTIES

    public int Count => _followCount;

// INITIALISATION

    /// <summary>
    /// Initialises the root
    /// </summary>
    private void Awake(){
        _root = new Follower(null, null, 0f);
    }

    /// <summary>
    /// Initialises the player movement controller reference
    /// </summary>
    private void Start(){
        var controller = GetComponent<Player>().ControllerManager;
        _move = controller.GetController<MovementController>();
    }

    /// <summary>
    /// Subscribes to fixed update
    /// </summary>
    private void OnEnable() => RunUpdate(true);

    /// <summary>
    /// Unsubscribes to fixed update
    /// </summary>
    private void OnDisable() => RunUpdate(false);

// MANAGEMENT

    /// <summary>
    /// Adds a new follower to the player
    /// </summary>
    public void Add(Rigidbody follower){
        Follower parent = _root;

        while (parent.Child != null){
            parent = parent.Child;
        }

        var collider = follower.GetComponent<SphereCollider>();
        var radius = collider.radius + _followPadding;

        var follow = new Follower(parent, follower, radius);
        parent.SetChild(follow);

        _followCount++;
    }

    /// <summary>
    /// Adds a group of followers to the player
    /// </summary>
    public void Add(Rigidbody[] followers){
        for (int i = 0; i < followers.Length; i++){
            Add(followers[i]);
        }
        _onPickup.Invoke();
    }

    /// <summary>
    /// Removes number of player followers
    /// </summary>
    public void Remove(int count){
        count = Mathf.Min(_followCount, count);
        Remove(_root.Child, count);
        _onLost.Invoke();
    }

    /// <summary>
    /// Recursively remove followers for count
    /// </summary>
    private void Remove(Follower follower, int count){
        if (count < 1) return;
        
        follower.Remove();
        follower.GameObject.SetActive(false);

        Remove(follower.Child, --count);
        _followCount--;
    }

// MOVEMENT

    /// <summary>
    /// On update manager fixed update event
    /// </summary>
    private void OnFixedUpdate(float fixedDeltaTime){
        if (_root.Child == null) return;
        var position = transform.position + (-transform.forward * _followPadding);
        _root.Child.Move(position, _move.Speed, fixedDeltaTime);
    }

    /// <summary>
    /// Toggles the fixed update loop
    /// </summary>
    private void RunUpdate(bool run){
        if (run) _update.Subscribe(OnFixedUpdate, UpdateType.FixedUpdate);
        else _update.Unsubscribe(OnFixedUpdate, UpdateType.FixedUpdate);
    }
}
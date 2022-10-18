using SF = UnityEngine.SerializeField;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Jellybeans.Updates;
using PlayerControllers.Controllers;

[RequireComponent(typeof(Player))]
public class PlayerFollowers : MonoBehaviour
{
    [SF] private GameRules _gameRules = null;
    [SF] private UpdateManager _update = null;

    private int _followCount = 0;
    private Follower _root = null;
    private MovementController _move = null;

    [Space, SF] private UnityEvent<Follower> _onPickup = new();
    [Space, SF] private UnityEvent<Follower> _onCrash = new();
    [Space, SF] private UnityEvent<Follower> _onRemoved = new();

// PROPERTIES

    public int Count => _followCount;
    public int KeeptOnLevelUp => _gameRules.KeptOnLevelUp;

// INITIALISATION

    /// <summary>
    /// Initialises the root
    /// </summary>
    private void Awake(){
        var rb = GetComponent<Rigidbody>();
        _root = new Follower(null, rb, 0f);
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
    /// Adds a group of followers to the player
    /// </summary>
    public void Add(List<Rigidbody> fellows){
        for (int i = 0; i < fellows.Count; i++){
            Add(fellows[i]);
        }
    }

    /// <summary>
    /// Adds a new follower to the player
    /// </summary>
    public void Add(Rigidbody fellow){
        Follower parent = _root;

        while (parent.Child != null){
            parent = parent.Child;
        }
        
        fellow.transform.SetParent(null);

        var collider = fellow.GetComponent<SphereCollider>();
        var radius = collider.radius + _gameRules.FollowPadding;

        var follower = new Follower(parent, fellow, radius);
        parent.SetChild(follower);

        _followCount++;
        _onPickup.Invoke(follower);
    }


    /// <summary>
    /// Removes number of player followers
    /// </summary>
    public void Remove(int count){
        count = Mathf.Min(_followCount, count);
        Remove(_root.Child, count);
    }

    /// <summary>
    /// Recursively remove followers for count
    /// </summary>
    private void Remove(Follower follower, int count){
        if (count < 1) return;
        follower.Remove();

        Remove(follower.Child, --count);
        _followCount--;

        _onCrash.Invoke(follower);
    }


    /// <summary>
    /// Removes all followers, starting from the back
    /// </summary>
    public void Clear(){
        var parent = _root;
        
        while(parent.Child != null){
            parent = parent.Child;
        }

        var count = _followCount - _gameRules.KeptOnLevelUp;
        Clear(parent, count);
    }

    /// <summary>
    /// Recursivly removes followers, from the back, for count
    /// </summary>
    private void Clear(Follower follower, int count){
        if (count < 1) return;
        follower.Remove();

        Clear(follower.Parent, --count);
        _followCount--;

        _onRemoved.Invoke(follower);
    }

// MOVEMENT

    /// <summary>
    /// On update manager fixed update event
    /// </summary>
    private void OnFixedUpdate(float fixedDeltaTime){
        if (_root.Child == null) return;
        var position = transform.position + (-transform.forward * _gameRules.FollowPadding);
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
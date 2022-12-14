using SF = UnityEngine.SerializeField;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using PlayerControllers.Controllers;
using Jellybeans.Updates;
using TMPro;

[RequireComponent(typeof(Player), typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerFollowers : MonoBehaviour
{
    [SF] private TMP_Text _hudText = null;
    [SF] private TMP_Text _winScreenText = null;
    [SF] private GameRules _gameRules = null;
    [SF] private PlayerStatContainer _stats = null;
    [SF] private UpdateManager _update = null;

    private int _totalCount = 0;
    private int _followCount = 0;
    private float _followerOffset = 0f;
    private Follower _root = null;

    private Rigidbody _rigidbody = null;
    private CapsuleCollider _collider = null;
    private GroundChecker _ground = null;
    private MovementController _movement = null;

    [Space, SF] private UnityEvent<Follower> _onPickup = new();
    [Space, SF] private UnityEvent<Follower> _onCrash = new();
    [Space, SF] private UnityEvent<Follower> _onRemoved = new();

// PROPERTIES

    public int Count => _followCount;
    public int TotalCount => _totalCount;

// INITIALISATION

    /// <summary>
    /// Initialises the root
    /// </summary>
    private void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _followerOffset = (_collider.height * 0.5f) + _gameRules.FollowPadding;
        _root = new Follower(null, transform, _followerOffset, 0f);

        UpdateText();
    }

    /// <summary>
    /// Initialises the movement controller
    /// </summary>
    private void Start(){
        var controller = GetComponent<Player>().ControllerManager;
        _movement = controller.GetController<MovementController>();

        // New follower pathfinding
        _ground = GetComponent<GroundChecker>();
        _root.InitPath(_movement.Speed, _ground.IsGrounded, transform.position);
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
    public void Add(List<Transform> followers){
        if (_totalCount >= _gameRules.MaxFollowerCount){
            var newcount = _followCount + followers.Count;
            RemoveFromBehind(newcount % _gameRules.MaxFollowerCount);
        }

        for (int i = 0; i < followers.Count; i++){
            Add(followers[i]);
        }

        _totalCount += followers.Count;
        UpdateText();
    }

    /// <summary>
    /// Adds a new follower to the player
    /// </summary>
    private void Add(Transform fellow){
        Follower parent = GetLast();

        var follower = new Follower(
            parent, fellow, 
            _followerOffset,
            _stats.MaxMoveSpeed
        );

        parent.SetChild(follower);
        _followCount++;

        _onPickup.Invoke(follower);
    }


    /// <summary>
    /// Removes number of player followers
    /// </summary>
    public void Remove(int count){
        count = Mathf.Min(_followCount, count);
        RemoveFromFront(GetFirst(), count);

        _totalCount -= count;
        UpdateText();
    }

    /// <summary>
    /// Recursively remove followers for count
    /// </summary>
    private void RemoveFromFront(Follower follower, int count){
        if (count < 1) return;

        if (_totalCount >= _gameRules.MaxFollowerCount){
            var last = GetLast();
            follower.Reposition(last);

        } else follower.Remove();

        RemoveFromFront(follower.Child, --count);
        _followCount--;

        _onCrash.Invoke(follower);
    }

    /// <summary>
    /// Recursivly removes followers, from the back, for count
    /// </summary>
    private void RemoveFromBehind(Follower follower, int count){
        if (count < 1 || follower == null) return;
        follower.Remove();

        RemoveFromBehind(follower.Parent, --count);
        _followCount--;

        _onRemoved.Invoke(follower);
    }

    /// <summary>
    /// Removes followers, from the back, for count
    /// </summary>
    private void RemoveFromBehind(int count){
        RemoveFromBehind(GetLast(), count);
    }


    /// <summary>
    /// Returns the first follower in the chain
    /// </summary>
    public Follower GetFirst(){
        return _root.Child;
    }

    /// <summary>
    /// Returns the last follower in the chain
    /// </summary>
    public Follower GetLast(){
        Follower follower = _root;

        while (follower.Child != null){
            follower = follower.Child;
        }

        return follower;
    }

    /// <summary>
    /// Returns the follower script for this follower
    /// </summary>
    public Follower GetFollower(Transform fellow){
        var follower = _root.Child;

        while (follower.Transform != fellow && follower.Child != null){
            follower = follower.Child;
        }

        if (follower.Transform == fellow) 
            return follower;

        return null;
    }


    /// <summary>
    /// Teleports all followers to position with rotation
    /// </summary>
    public void Teleport(Vector3 position, Quaternion rotation){
        var node = _root.Child;
        
        while (node.Child != null){
            node.Transform.SetPositionAndRotation(position, rotation);
            node = node.Child;
        }
    }

// MOVEMENT

    /// <summary>
    /// On update manager fixed update event
    /// </summary>
    private void OnFixedUpdate(float fixedDeltaTime){
        var position = transform.position;

        var speed = Mathf.Max(
            _movement.Speed,
            _rigidbody.velocity.magnitude
        );

        //_root.UpdatePath(speed, _ground.IsGrounded, position);
        //_root.Child?.Move(fixedDeltaTime);

        // Old follower pathfinding
        var offset = transform.forward * _followerOffset;
        _root.Child?.Move(position - offset, fixedDeltaTime, speed);
    }

    /// <summary>
    /// Toggles the fixed update loop
    /// </summary>
    private void RunUpdate(bool run){
        if (run) _update.Subscribe(OnFixedUpdate, UpdateType.FixedUpdate);
        else _update.Unsubscribe(OnFixedUpdate, UpdateType.FixedUpdate);
    }

// INTERFACE

    /// <summary>
    /// Updates the hud follow counter
    /// </summary>
    private void UpdateText(){
        if (_hudText == null) return;
        _hudText.text = _totalCount.ToString();
        _winScreenText.text = _hudText.text;
    }

    // DEBUGGING
#if UNITY_EDITOR
    //private void OnDrawGizmos(){
    //    if (_root == null) return;
    //    _root.DrawPoints();
    //}
#endif
}
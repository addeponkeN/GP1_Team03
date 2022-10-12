using SF = UnityEngine.SerializeField;
using UnityEngine;
using PlayerControllers.Controllers;
using Jellybeans.Updates;

public class Hazard : MonoBehaviour
{
    [SF] private float _collisionTimer = 2.5f;
    [SF] private float _minPlayerSpeed = 3f;
    [SF] private float _minBreakSpeed = 25f;
    [SF] private Vector2Int _minMaxFollowerLoss = Vector2Int.one;
    [Space]
    [SF] private LayerMask _playerLayer = 1 << 0;
    [SF] private PlayerStatContainer _stats = null;
    [SF] private UpdateManager _update = null;
    
    private float _timer = 0f;

// COLLISION HANDLING

    /// <summary>
    /// On player colliding with hazard
    /// </summary>
    private void OnCollisionEnter(Collision other){
        var layer = other.gameObject.layer;
        if (((1 << layer) & _playerLayer) == 0) return;
        
        // Timer between collisions
        if (_timer > 0) return;

        var player = other.gameObject;
        var movement = GetMovement(player);    
        if (movement.Speed < _minPlayerSpeed) return;

        // Speed that breaks hazard
        if (movement.Speed >= _minBreakSpeed){
            this.gameObject.SetActive(false);
            return;
        }

        RemoveFollower(player, movement.Speed);
        RunTimer(true);
    }

    /// <summary>
    /// Remove follower(s) from player
    /// </summary>
    private void RemoveFollower(GameObject player, float speed){
        var followers = player.GetComponent<PlayerFollowers>();

        var percent = Mathf.InverseLerp(_minPlayerSpeed, _stats.MaxMoveSpeed, speed);
        var count = (int)Mathf.Lerp(_minMaxFollowerLoss.x, _minMaxFollowerLoss.y, percent);

        followers.Remove(count);
    }

    /// <summary>
    /// Returns the player movement controller
    /// </summary>
    private MovementController GetMovement(GameObject player){
        var controller = player.GetComponent<Player>().ControllerManager;
        return controller.GetController<MovementController>();
    }

// TIMER HANDLING

    /// <summary>
    /// Decrement collision timer
    /// </summary>
    /// <param name="deltaTime"></param>
    private void OnUpdate(float deltaTime){
        _timer -= deltaTime;

        if (_timer > 0) return;
        RunTimer(false);
    }

    /// <summary>
    /// Toggles update loop
    /// </summary>
    private void RunTimer(bool run){
        if (run){
            _timer = _collisionTimer;
            _update.Subscribe(OnUpdate, UpdateType.Update);
        
        } else {
            _timer = 0;
            _update.Unsubscribe(OnUpdate, UpdateType.Update);
        }
    }
}
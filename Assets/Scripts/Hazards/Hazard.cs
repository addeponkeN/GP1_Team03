using SF = UnityEngine.SerializeField;
using PlayerControllers.Controllers;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SF] private bool _canBeDestroyed = true;
    [SF] private int _followerLoss = 2;
    [Space]
    [SF] private bool _canBeRespawned = true;
    [SF] private float _respawnTimer = 10f;
    [Space]
    [SF] private LayerMask _playerLayer = 1 << 0;
    [SF] private PlayerStatContainer _playerStats = null;
    [SF] private ParticleSystem _crashFX = null;
    [SF] private AudioSource _crashSound = null;

    private BoostController _boost = null;
    private MovementController _movement = null;
    private PlayerFollowers _followers = null;
    private readonly float _speedPercent = 0.5f;

// COLLISION HANDLING

    /// <summary>
    /// On player colliding with hazard
    /// </summary>
    private void OnCollisionEnter(Collision other){
        var layer = other.gameObject.layer;
        if (((1 << layer) & _playerLayer) == 0) return;

        var player = other.gameObject;
        var movement = GetMovement(player);

        var minSpeed = _playerStats.MaxMoveSpeed * _speedPercent;
        if (movement.Speed < minSpeed) return;

        if (_crashFX != null) 
            _crashFX.Play();

        if (_crashSound != null) 
            _crashSound.Play();

        var boost = GetBoost(player);
        if (!boost.IsBoosting) 
            RemoveFollower(player);

        if (!_canBeDestroyed) return;
        ToggleHazard(false);

        if (!_canBeRespawned) return;
        Invoke("Respawn", _respawnTimer);
    }
        
    /// <summary>
    /// Changes the hazard's enabled state
    /// </summary>
    private void ToggleHazard(bool enabled){
        var renderers = GetComponentsInChildren<MeshRenderer>();
        
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].enabled = enabled;

        var collider = GetComponent<Collider>();
        collider.enabled = enabled;
    }

    /// <summary>
    /// Respawns the hazard, on invoke
    /// </summary>
    private void Respawn() => ToggleHazard(true);

// PLAYER HANDLING

    /// <summary>
    /// Remove follower(s) from player
    /// </summary>
    private void RemoveFollower(GameObject player){
        _followers ??= player.GetComponent<PlayerFollowers>();
        _followers.Remove(_followerLoss);
    }

    /// <summary>
    /// Returns the player movement controller
    /// </summary>
    private MovementController GetMovement(GameObject player){
        if (_movement != null) return _movement;
        var controller = player.GetComponent<Player>().ControllerManager;
        return controller.GetController<MovementController>();
    }
    
    /// <summary>
    /// Returns the player boost controller
    /// </summary>
    private BoostController GetBoost(GameObject player){
        if (_boost != null) return _boost;
        var controller = player.GetComponent<Player>().ControllerManager;
        return controller.GetController<BoostController>();
    }
}
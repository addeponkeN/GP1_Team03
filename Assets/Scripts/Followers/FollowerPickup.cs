using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;
using SF = UnityEngine.SerializeField;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using PlayerControllers.Controllers;

[RequireComponent(typeof(SphereCollider))]
public class FollowerPickup : MonoBehaviour
{
    [Header("Circle Settings")]
    [SF] private int _zones = 3;
    [SF] private int _minPickupCount = 1;
    [SF] private float _respawnTimer = 10f;
    [SF] private LayerMask _playerLayer = 1 << 0;
    [SF] private InputActionReference _pickupInput = null;
    [SF] private GameObject[] _followerPrefab = null;
    [SF] private List<GameObject> _people = null;
    [Space]
    [SF] private UnityEvent _onPickup = new();

    private Transform _player = null;
    private PlayerFollowers _followers = null;
    private SphereCollider _collider = null;

// INITIALISATION

    /// <summary>
    /// Initialises the collider
    /// </summary>
    private void Awake(){
        _collider ??= GetComponent<SphereCollider>();
        _collider.isTrigger = true;
    }

    /// <summary>
    /// Subscribes to interaction input
    /// </summary>
    private void OnEnable(){
        _pickupInput.action.canceled += ctx => OnInteractInput(ctx);
        _pickupInput.action.Enable();
    }

    /// <summary>
    /// Unsubscribes from interaction input
    /// </summary>
    private void OnDisable(){
        _pickupInput.action.canceled -= OnInteractInput;
    }

// FOLLOWERS

    /// <summary>
    /// On interact button released
    /// </summary>
    private void OnInteractInput(CallbackContext context){
        if (_player == null) return;

        var recruited = GetFollowers();
        _followers.Add(recruited);
        _onPickup.Invoke();

        SetActiveState(false);
        Invoke("RespawnFollowers", _respawnTimer);

        SetBoost(_player, true);
        _player = null;
    }

    /// <summary>
    /// Returns the recruited followers
    /// </summary>
    private List<Transform> GetFollowers(){
        var recruited = new List<Transform>();

        var distance = Vector3.Distance(
            _player.position, transform.position
        );

        var scale = transform.localScale;
        var radius = _collider.radius * Mathf.Max(scale.x, scale.z);
        var difference = radius / _zones;

        for (int i = 0; i <= _zones; i++){
            bool less = distance < radius - (difference * i);
            bool greater = distance > radius - (difference * (i + 1));
            if (!less || !greater) continue;
            
            var percent = Mathf.InverseLerp(0, _zones, i + 1);
            var count =  (int)Mathf.Lerp(_minPickupCount, _people.Count, percent);

            for (int j = count - 1; j >= 0; j--){
                var person = _people[j];

                if (person == null) continue;
                person.SetActive(false);

                var prefab = _followerPrefab[
                    Random.Range(0, _followerPrefab.Length)
                ];

                var follower = Instantiate(
                    prefab, person.transform.position, person.transform.rotation, null
                );
                
                recruited.Add(follower.transform);
            }
        }

        return recruited;
    }

    /// <summary>
    /// Toggles the visible state
    /// </summary>
    private void SetActiveState(bool active){
        var mr = GetComponent<MeshRenderer>();
        if (mr != null) mr.enabled = active;

        for (int i = 0; i < transform.childCount; i++){
            var child = transform.GetChild(i);
            child.gameObject.SetActive(active);
        }

        for (int i = 0; i < _people.Count; i++)
            _people[i]?.SetActive(active);

        _collider.enabled = active;
    }

    /// <summary>
    /// Respawns the pickup circle and its followers
    /// </summary>
    private void RespawnFollowers(){
        SetActiveState(true);
    }

// TRIGGERING

    /// <summary>
    /// If the player enters the pickup circle
    /// </summary>
    private void OnTriggerEnter(Collider other){
        var layer = other.gameObject.layer;
        if (((1 << layer) & _playerLayer) == 0) return;

        _player ??= other.transform;
        _followers ??= _player.GetComponent<PlayerFollowers>();
        
        SetBoost(_player, false);
    }

    /// <summary>
    /// If the player exits the pickup circle
    /// </summary>
    private void OnTriggerExit(Collider other){
        if (other.transform != _player) return;

        SetBoost(_player, true);
        _player = null;
    }

    /// <summary>
    /// Toggles boost when inside/outside the pickup circle
    /// </summary>
    private void SetBoost(Transform player, bool enabled){
        var controller = _player.GetComponent<Player>().ControllerManager;
        var boost = controller.GetController<BoostController>();
        boost.SetEnabled(enabled);
    }

// DEBUGGING VISUALS
#if UNITY_EDITOR

    /// <summary>
    /// Draws a percent based circle around the pickup
    /// </summary>
    private void OnDrawGizmos(){
        if (!enabled) return;

        _collider ??= GetComponent<SphereCollider>();
        if (_collider == null) return;
        
        var radius = _collider.radius;
        var difference = 1f / _zones;

        for (int i = 0; i <= _zones; i++){
            var percent = difference * i;
            var colour = Color.Lerp(Color.red, Color.green, percent);

            DrawCircle(colour, radius * percent);
        }
    }

    /// <summary>
    /// Draws a circle around the pickup position
    /// </summary>
    private void DrawCircle(Color colour, float radius){
        Gizmos.color = colour;

        var centre = transform.position;
        var scale = transform.localScale;
        radius *= Mathf.Max(scale.x, scale.z);

        var point = centre + (Vector3.forward * radius);
        var previus = point;

        var edges = 16f;
        var amount = 360f / edges;

        for (int i = 1; i <= edges; i++){
            var offset = Quaternion.Euler(0, amount * i, 0);
            var current = offset * (point - centre) + centre;

            Gizmos.DrawLine(previus, current);
            previus = current;
        }
    }

#endif
}
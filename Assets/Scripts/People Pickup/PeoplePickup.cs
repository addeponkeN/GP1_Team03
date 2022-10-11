using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;
using SF = UnityEngine.SerializeField;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SphereCollider))]
public class PeoplePickup : MonoBehaviour
{
    [Header("Circle Settings")]
    [SF] private int _zones = 3;
    [SF] private int _minPickupCount = 1;
    [SF] private LayerMask _playerLayer = 1 << 0;
    [SF] private List<GameObject> _people = null;

    [Header("References")]
    [SF] private InputActionReference _pickupInput = null;
    [SF] private SphereCollider _collider = null;

    private Transform _player = null;

    // INITIALISATION

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        _collider.isTrigger = true;
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnEnable()
    {
        //_pickupInput.action.canceled += ctx => OnInteractInput(ctx);
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnDisable()
    {
        //_pickupInput.action.canceled -= OnInteractInput;
    }

    // TEMP
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            OnInteractInput(default(CallbackContext));
    }

    // INPUT EVENTS

    /// <summary>
    /// On interact button released
    /// </summary>
    private void OnInteractInput(CallbackContext context)
    {
        if (_player == null) return;

        var distance = Vector3.Distance(
            _player.position, transform.position
        );

        var difference = _collider.radius / _zones;
        var percent = Mathf.InverseLerp(0, _collider.radius, distance);
        var count = Mathf.Lerp(_minPickupCount, _people.Count, percent);


    }

    // TRIGGERING

    /// <summary>
    /// If the player enters the pickup circle
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        var layer = other.gameObject.layer;
        if (((1 << layer) & _playerLayer) == 0) return;

        _player = other.transform;
    }

    /// <summary>
    /// If the player exits the pickup circle
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.transform != _player) return;
        _player = null;
    }

    // DEBUGGING
#if UNITY_EDITOR

    /// <summary>
    /// Draws a percent based circle around the pickup
    /// </summary>
    private void OnDrawGizmos()
    {
        if (_collider == null) return;
        var radius = _collider.radius;
        var difference = 1f / _zones;

        for (int i = 0; i <= _zones; i++)
        {
            var percent = difference * i;
            var colour = Color.Lerp(Color.red, Color.green, percent);

            DrawCircle(colour, radius * percent);
        }
    }

    /// <summary>
    /// Draws a circle around the pickup position
    /// </summary>
    private void DrawCircle(Color colour, float radius)
    {
        Gizmos.color = colour;

        var centre = transform.position;
        var point = centre + (Vector3.forward * radius);
        var previus = point;

        var edges = 16f;
        var amount = 360f / edges;

        for (int i = 1; i <= edges; i++)
        {
            var offset = Quaternion.Euler(0, amount * i, 0);
            var current = offset * (point - centre) + centre;

            Gizmos.DrawLine(previus, current);
            previus = current;
        }
    }

#endif
}
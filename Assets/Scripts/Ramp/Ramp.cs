using SF = UnityEngine.SerializeField;
using UnityEngine;
using Jellybeans.Updates;
using PlayerControllers;
using PlayerControllers.Controllers;

public class Ramp : MonoBehaviour
{
    [SF] private AnimationCurve _speedTimeCurve = AnimationCurve.Linear(0, 0, 3, 3);
    [SF] private LayerMask _playerLayer = 1 << 0;
    [SF] private UpdateManager _update = null;
    [SF] private Transform[] _points = null;

    private bool _running = false;
    private float _time = 0f;
    private float _minCurveTime = 0;
    private float _maxCurveTime = 0;
    
    private int _index = 0;
    private float _startSpeed = 0f;
    private Vector3 _current = Vector3.zero;
    private Vector3 _target = Vector3.zero;

    private GameObject _player = null;
    private Rigidbody _rigidbody = null;
    private PlayerControllerManager _controller = null;

// INITIALISATION

    /// <summary>
    /// Initialise curve settings
    /// </summary>
    private void Awake(){
        var length = _speedTimeCurve.length - 1;
        _minCurveTime = _speedTimeCurve.keys[0].time;
        _maxCurveTime = _speedTimeCurve.keys[length].time;
    }

// RAMP HANDLING

    /// <summary>
    /// On player entering ramp
    /// </summary>
    private void OnTriggerEnter(Collider other){
        if (_running) return;

        var layer = other.gameObject.layer;
        if (((1 << layer) & _playerLayer) == 0) 
            return;

        _player ??= other.gameObject;
        _rigidbody ??= _player.GetComponent<Rigidbody>();

        // Disables movement controller
        _controller ??= _player.GetComponent<Player>().ControllerManager;
        SetControl(_player, false);

        // Set initial speed on curve
        var movement = _controller.GetController<MovementController>();
        var keys = _speedTimeCurve.keys;
        keys[0].value = movement.Speed;
        _speedTimeCurve = new AnimationCurve(keys);

        // Init values
        _index = 0;
        _time = _minCurveTime;
        _current = _rigidbody.position;
        _target = _points[_index].position;

        SetUpdate(true);
    }

    /// <summary>
    /// Moves the player along the path
    /// </summary>
    private void OnFixedUpdate(float fixedDeltaTime){
        _time += fixedDeltaTime;

        var speed = _speedTimeCurve.Evaluate(_time);
        var direction = (_target - _current).normalized;
        var velocity = direction * speed;

        _rigidbody.transform.Translate(velocity);
        //_rigidbody.MovePosition(_rigidbody.position + velocity);
        _rigidbody.MoveRotation(Quaternion.LookRotation(direction));

        if (Vector3.Distance(_current, _target) < 0.1f){
            if (_index + 1 < _points.Length){ 
                _current = _rigidbody.position;
                _target = _points[++_index].position;
            
            } else _time = _maxCurveTime;
        }

        if (_time >= _maxCurveTime){
            SetUpdate(false);
            SetControl(_player, true);
        }
    }

    /// <summary>
    /// Toggles player movement control
    /// </summary>
    private void SetControl(GameObject player, bool enabled){
        _rigidbody.useGravity = enabled;
        _controller.SetEnabled(enabled);
    }

    /// <summary>
    /// Toggles update loop
    /// </summary>
    private void SetUpdate(bool enabled){
        if (enabled) _update.Subscribe(
            OnFixedUpdate, 
            UpdateType.FixedUpdate
        );
        
        else _update.Unsubscribe(
            OnFixedUpdate, 
            UpdateType.FixedUpdate
        );

        _running = enabled;
    }

// DEBUGGING VISUALS
#if UNITY_EDITOR
    private readonly Color _lineColour = Color.Lerp(
        Color.red, Color.yellow, 0.5f
    );

    /// <summary>
    /// Draws a line between points
    /// </summary>
    private void OnDrawGizmos(){
        if (_points.Length == 0) return;
        Gizmos.color = _lineColour;

        for (int i = 0; i < _points.Length - 1; i++){
            var point1 = _points[i];
            var point2 = _points[i + 1];

            if (point1 == null || 
                point2 == null) 
                continue;

            Gizmos.DrawLine(
                point1.position, 
                point2.position
            );
        }
    }

#endif
}
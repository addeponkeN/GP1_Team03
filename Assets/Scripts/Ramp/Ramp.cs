using SF = UnityEngine.SerializeField;
using UnityEngine;
using Jellybeans.Updates;
using PlayerControllers;

public class Ramp : MonoBehaviour
{
    [SF] private AnimationCurve _speedTimeCurve = AnimationCurve.Linear(0, 5, 1, 10);
    [SF] private LayerMask _playerLayer = 1 << 0;
    [SF] private UpdateManager _update = null;
    [SF] private Transform[] _points = null;

    private int _current = 0;
    private float _time = 0f;
    private float _minCurveTime = 0;
    private float _maxCurveTime = 0;
    private Vector3 _point1 = Vector3.zero;
    private Vector3 _point2 = Vector3.zero;

    private GameObject _player = null;
    private Rigidbody _rigidbody = null;
    private PlayerControllerManager _controller = null;

// INITIALISATION

    /// <summary>
    /// Initialise curve settings
    /// </summary>
    private void Awake(){
        _minCurveTime = _speedTimeCurve.keys[_speedTimeCurve.length - 1].time;
        _maxCurveTime = _speedTimeCurve.keys[0].time;
    }

// RAMP HANDLING

    /// <summary>
    /// On player entering ramp
    /// </summary>
    private void OnTriggerEnter(Collider other){
        var layer = other.gameObject.layer;
        if (((1 << layer) & _playerLayer) == 0) return;

        _player ??= other.gameObject;
        _rigidbody ??= _player.GetComponent<Rigidbody>();
        _controller ??= _player.GetComponent<Player>().ControllerManager;
        SetControl(_player, false);

        _current = 0;
        _time = _minCurveTime;
        _point1 = _rigidbody.transform.position;
        _point2 = _points[_current].position;
        SetUpdate(true);
    }

    /// <summary>
    /// Moves the player along the path
    /// </summary>
    private void OnFixedUpdate(float fixedDeltaTime){
        _time += fixedDeltaTime;
        Debug.Log("running");
        var direction = (_point2 - _point1).normalized;
        var speed = _speedTimeCurve.Evaluate(_time);
        _rigidbody.MovePosition(_rigidbody.position + (direction * speed));

        if (Vector3.Distance(_point1, _point2) < 0.01){
            if (_current + 1< _points.Length){ 
                _point1 = _rigidbody.transform.position;
                _point2 = _points[++_current].position;
            
            } else _time = _maxCurveTime;
        }

        if (_time > _maxCurveTime){
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
        if (enabled) _update.Subscribe(OnFixedUpdate, UpdateType.FixedUpdate);
        else _update.Unsubscribe(OnFixedUpdate, UpdateType.FixedUpdate);
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
using SF = UnityEngine.SerializeField;
using Attributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.UIElements;

public class GroundChecker : MonoBehaviour
{
    public bool IsGrounded
    {
        get => _isGrounded;
        private set => _isGrounded = value;
    }

    public event UnityAction<bool> OnGroundedChangedEvent;
    
    [Space(10)]
    
#if UNITY_EDITOR
    [ReadOnly]
#endif
    
    [SF] private bool _isGrounded;
    [Space(10f)]
    [SF] private Transform _center;
    [SF] private float _radius = 0.1f;
    [SF] private float _distance = 0.5f;
    [Space(10f)]
    [SF] private int[] _layerMasks;

    private int _layer;
    private bool _oldGrounded;

    private void Start()
    {
        _layer = 1 << 0;
        
        for(int i = 0; i < _layerMasks.Length; i++)
        {
            if(i == 0)
                _layer = 1 << _layerMasks[i];
            else
                _layer &= _layerMasks[i];
        }
    }

    private void Update(){
        _oldGrounded = IsGrounded;

        var position = _center.position;
        var offsetZ = _center.forward * _distance;

        IsGrounded = Physics.CheckCapsule(
            position + offsetZ, 
            position - offsetZ, 
            _radius, _layer
        );

        if (IsGrounded != _oldGrounded){
            OnGroundedChangedEvent?.Invoke(IsGrounded);
        }
    }

#if UNITY_EDITOR
    //private void OnDrawGizmos(){
    //    var fwDir = _center.forward;
    //    var position = _center.position;

    //    Gizmos.DrawWireSphere(position + fwDir * _distance, _radius);
    //    Gizmos.DrawWireSphere(position, _radius);
    //    Gizmos.DrawWireSphere(position - fwDir * _distance, _radius);
    //}
#endif
}
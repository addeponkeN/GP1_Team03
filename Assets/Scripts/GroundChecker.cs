using SF = UnityEngine.SerializeField;
using Attributes;
using UnityEngine;
using UnityEngine.Events;

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
    [Space(10f)]
    [SF] private int[] _layerMasks;

    private bool _oldGrounded;
    private int _layer;

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

    private void Update()
    {
        _oldGrounded = IsGrounded;
        IsGrounded = Physics.CheckSphere(_center.position, _radius, _layer);

        if(IsGrounded != _oldGrounded)
        {
            OnGroundedChangedEvent?.Invoke(IsGrounded);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_center.position, _radius);
    }
#endif
}
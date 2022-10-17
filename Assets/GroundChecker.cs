using System;
using UnityEngine;
using UnityEngine.Events;

public class GroundChecker : MonoBehaviour
{
    public bool IsGrounded { get; private set; }

    [SerializeField] private Transform _center;
    [SerializeField] private float _radius = 0.1f;
    
    private bool _oldGrounded;
    
    private UnityAction<bool> onGroundedChanged;

    private void Update()
    {
        _oldGrounded = IsGrounded;
        IsGrounded = Physics.CheckSphere(_center.position, _radius);

        if(IsGrounded != _oldGrounded)
        {
            onGroundedChanged?.Invoke(IsGrounded);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_center.position, _radius);
    }
}
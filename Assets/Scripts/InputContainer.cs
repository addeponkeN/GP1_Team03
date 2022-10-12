using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class InputContainer
{
    [Header("Inputs")]
    
    public InputActionReference Movement;
    public InputActionReference Boost;
}

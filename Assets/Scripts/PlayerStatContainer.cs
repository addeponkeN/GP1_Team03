using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatContainer",
    menuName = "BikeMania/PlayerStatContainer", order = 0)]
public class PlayerStatContainer : ScriptableObject
{
    private Player _p;

//  PLAYER STAT FIELDS

    [TooltipAttribute("The movement acceleration of the bike")]
    [Range(1f, 50f)] 
    public float MovementAcceleration;


    [TooltipAttribute("The maximum speed of the bike")]
    [Range(2f, 50f)]
    public float MaxMoveSpeed;


    [TooltipAttribute("The rotation speed of the bike")]
    [Range(1f, 50f)] 
    public float RotationSpeed;
    
    
    [TooltipAttribute("The maximum rotation speed of the bike")]
    [Range(1f, 50f)] 
    public float MaxRotationSpeed;

    
    [TooltipAttribute("**RotationScales** rotation speed with movement speed\n" +
                      "0f = No rotation speed when moving\n" +
                      "1f = Max rotation when moving\n")]
    [Range(0f, 1f)]
    public float RotationSpeedModifier;

    
    /// <summary>
    /// Setup the stat container
    /// </summary>
    /// <param name="player">Reference to the main player</param>
    public void Init(Player player)
    {
        _p = player;
    }
}

[Serializable]
public class Stat<T>
{
     public static implicit operator T(Stat<T> stat) => stat.Value; 
     
     public event Action<Stat<T>> ValueChangedEvent;
     
     [SerializeField] public T Value;

     public Stat(T value)
     {
         Value = value;
         ValueChangedEvent = null;
     }

     public void SetValue(T value)
     {
         Value = value;
         ValueChangedEvent?.Invoke(this);
     }
}
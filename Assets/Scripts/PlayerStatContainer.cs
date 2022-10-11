using System;
using Attributes;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatContainer",
    menuName = "BikeMania/PlayerStatContainer", order = 0)]
public class PlayerStatContainer : ScriptableObject
{
    private Player _p;

//  PLAYER STAT FIELDS

    [Range(1f, 50f)] [TooltipAttribute("The movement acceleration of the bike")]
    public float MovementAcceleration;


    [StatFloatRange(2f, 50f)] [TooltipAttribute("The maximum speed of the bike")]
    public Stat<float> MaxMoveSpeed = new(25f);


    [TooltipAttribute("The rotation speed of the bike")]
    [StatFloatRange(1f, 50f)] 
    public Stat<float> RotationSpeed;
    
    
    [TooltipAttribute("The maximum rotation speed of the bike")]
    [StatFloatRange(1f, 50f)] 
    public Stat<float> MaxRotationSpeed;

    
    [SerializeField]
    [TooltipAttribute("Scales rotation speed with movement speed\n" +
                      "0f = No rotation speed when moving\n" +
                      "1f = Max rotation when moving\n" +
                      "0.5f = Half rotation speed when movement speed is at max")]
    [StatFloatRange(0f, 1f)]
    public Stat<float> MovementRotationSpeedModifier;

    
    /// <summary>
    /// Setup the stat container
    /// </summary>
    /// <param name="player">The main player</param>
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
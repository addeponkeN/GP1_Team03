using UnityEngine;

[CreateAssetMenu(fileName = "GameRules",
    menuName = "BikeMania/GameRules", order = 0)]
public class GameRules : ScriptableObject
{
    [Tooltip("The amount of followers that is required to win")]
    [Range(1, 1000)]
    public int FollowerWinCount = 30;

    [Tooltip("The amount of time before losing (in seconds)")]
    [Range(30f, 600f)]
    public float Time = 60f * 3f;
    
}
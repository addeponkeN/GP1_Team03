using System.Runtime.InteropServices;
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

    [Tooltip("The distance between followers")]
    public float FollowPadding = 2f;

    [Tooltip("The amount of followers that are kept on level up")]
    public int KeptOnLevelUp = 2;

    [Tooltip("The amount of followers that are required to level up")]
    public int LevelUpCount = 20;
}
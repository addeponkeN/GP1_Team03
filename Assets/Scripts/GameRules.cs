using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "GameRules",
    menuName = "BikeMania/GameRules", order = 0)]
public class GameRules : ScriptableObject
{
    //[Tooltip("The amount of followers that is required to win")]
    //[Range(1, 100)]
    //public int FollowerWinCount = 30;

    [Tooltip("The amount of time before losing (in seconds)")]
    //[Range(30f, 600f)]
    public float Time = 60f * 3f;

    [Tooltip("The distance between followers")]
    public float FollowPadding = 2f;

    [Tooltip("The maximum amount of followers that follows the player")]
    public int MaxFollowerCount = 20;

    [Tooltip("How many players")]
    [Range(1, 2)]
    public int PlayerCount = 1;
}
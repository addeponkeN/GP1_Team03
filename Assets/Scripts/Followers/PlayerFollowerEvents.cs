using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowerEvents : MonoBehaviour
{
    /// <summary>
    /// On follower picked up event
    /// </summary>
    public void OnPickedUp(Follower follower){

    }

    /// <summary>
    /// On follower crashed event
    /// </summary>
    public void OnCrashed(Follower follower){
        Destroy(follower.GameObject);
    }

    /// <summary>
    /// On follower removed on level up event
    /// </summary>
    public void OnRemoved(Follower follower){
        Destroy(follower.GameObject);
    }
}
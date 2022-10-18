using SF = UnityEngine.SerializeField;
using UnityEngine;

public class PlayerFollowerEvents : MonoBehaviour
{
    [SF] private ParticleSystem _pickupFX = null;

    /// <summary>
    /// On follower picked up event
    /// </summary>
    public void OnPickedUp(Follower follower){
        if (follower == null) return;
        _pickupFX?.Play();
    }

    /// <summary>
    /// On follower crashed event
    /// </summary>
    public void OnCrashed(Follower follower){
        if (follower == null) return;
        Destroy(follower.GameObject);
    }

    /// <summary>
    /// On follower removed on level up event
    /// </summary>
    public void OnRemoved(Follower follower){
        if (follower == null) return;
        Destroy(follower.GameObject);
    }
}
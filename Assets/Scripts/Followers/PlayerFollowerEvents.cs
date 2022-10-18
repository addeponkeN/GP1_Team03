using SF = UnityEngine.SerializeField;
using UnityEngine;

public class PlayerFollowerEvents : MonoBehaviour
{
    [SF] private GameObject _pickupFXPrefab = null;

    // Create pool for pickup fx and then invoke(fx play time) to disable it when done


    /// <summary>
    /// On follower picked up event
    /// </summary>
    public void OnPickedUp(Follower follower){
        if (follower == null) return;

        //var tfm = follower.Transform;
        //Instantiate(_pickupFXPrefab, tfm.position, tfm.rotation);
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
using SF = UnityEngine.SerializeField;
using UnityEngine;
using System.Collections.Generic;

public class PlayerFollowerEvents : MonoBehaviour
{
    [SF] private GameObject _pickupFXPrefab = null;

    private Queue<ParticleSystem> _pickupFXPool = null;

// INITIALISATION

    /// <summary>
    /// Initialises the follower events
    /// </summary>
    private void Awake(){
        InitFxPool(_pickupFXPrefab, ref _pickupFXPool);
    }

    /// <summary>
    /// Initialises a new particle pool
    /// </summary>
    private void InitFxPool(GameObject prefab, ref Queue<ParticleSystem> pool) {
        pool = new Queue<ParticleSystem>();

        for (int i = 0; i < 6; i++){
            var fxObj = Instantiate(_pickupFXPrefab, Vector3.zero, Quaternion.identity);
            var fxSys = fxObj.GetComponent<ParticleSystem>();
            pool.Enqueue(fxSys);
        }
    }

// FOLLOWER EVENTS

    /// <summary>
    /// On follower picked up event
    /// </summary>
    public void OnPickedUp(Follower follower){
        if (follower == null) return;
        var tfm = follower.Transform;
        PlayFX(tfm, ref _pickupFXPool);
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

// EFFECTS

    /// <summary>
    /// Plays the first available particle
    /// </summary>
    private void PlayFX(Transform follower, ref Queue<ParticleSystem> pool){
        if (pool.Count == 0) return;
        var fx = pool.Dequeue();

        fx.transform.position = follower.position;
        fx.Play();

        pool.Enqueue(fx);
    }
}
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowers : MonoBehaviour
{
    private List<Transform> _followers = null;


    public void AddFollower(Transform follower){
        _followers ??= new List<Transform>();

        if (!_followers.Contains(follower))
            _followers.Add(follower);
    }

    public void AddFollowers(Transform[] followers){
        _followers ??= new List<Transform>();

        for (int i = 0; i < followers.Length; i++){
            var follower = followers[i];
            
            if (!_followers.Contains(follower))
                _followers.Add(follower);
        }
    }

    public void RemoveFollowers(Transform follower){
        if (_followers == null ||
            _followers.Count == 0)
            return;

        var index = _followers.IndexOf(follower);
        if (index < 0) return;
        
        _followers.RemoveAt(index);
    }
}
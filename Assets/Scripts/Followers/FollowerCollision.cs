using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FollowerCollision : MonoBehaviour
{
    private int _jumpPadLayer = 1 << 11;
    private Follower _follower = null;

    private void Awake(){
        GetComponent<Collider>().isTrigger = true;
    }

    public void InitFollower(Follower follower){
        _follower = follower;
    }

    private void OnTriggerEnter(Collider other){
        var layer = (1 << other.gameObject.layer);
        _follower.Grounded = (layer & _jumpPadLayer) != 0;
    }
}
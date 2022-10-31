using SF = UnityEngine.SerializeField;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SF] private LayerMask _playerMask = 1 << 0;
    [SF] private Transform _destination = null;

    private void OnTriggerEnter(Collider other){
        var layer = (1 << other.gameObject.layer);
        if ((layer & _playerMask) == 0) return;

        var position = _destination.position;
        var rotation = _destination.rotation;

        other.transform.position = position;
        other.transform.rotation = rotation;

        var followers = other.GetComponent<PlayerFollowers>();
        followers?.Teleport(position, rotation);
    }
}

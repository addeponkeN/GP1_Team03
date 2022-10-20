using SF = UnityEngine.SerializeField;
using UnityEngine;

public class InBoundsChecker : MonoBehaviour
{
    [SF] private int _lowestPoint;
    [SF] private Transform _respawnPoint;

    private void Update()
    {
        if(transform.position.y < _lowestPoint)
        {
            Teleport();
        }
    }

    private void Teleport()
    {
        transform.SetPositionAndRotation(_respawnPoint.position, _respawnPoint.rotation);
    }
    
}
using SF = UnityEngine.SerializeField;
using UnityEngine;
using PlayerControllers.Controllers;
using Unity.VisualScripting;

public class Hazard : MonoBehaviour
{
    [SF] private float _minPlayerSpeed = 0f;
    [SF] private Vector2Int _minMaxLoss = Vector2Int.one;
    [SF] private LayerMask _playerLayer = 1 << 0;
    [SF] private PlayerStatContainer _stats = null;

    private float _collideTimer = 0f;

    /// <summary>
    /// On player colliding with hazard
    /// </summary>
    private void OnCollisionEnter(Collision other){
        var layer = other.gameObject.layer;
        if (((1 << layer) & _playerLayer) == 0) return;

        var gObj = other.gameObject;
        var controller = gObj.GetComponent<Player>().ControllerManager;
        var movement   = controller.GetController<MovementController>();
        if (movement.Speed < _minPlayerSpeed) return;

        var followers  = gObj.GetComponent<PlayerFollowers>();
        var percent = Mathf.InverseLerp(_minPlayerSpeed, _stats.MaxMoveSpeed, movement.Speed); 
        var count = (int)Mathf.Lerp(_minMaxLoss.x, _minMaxLoss.y, percent);

        followers.Remove(count);
    }
}
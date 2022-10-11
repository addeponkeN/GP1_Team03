using Jellybeans.Updates;
using PlayerControllers;
using PlayerControllers.Controllers;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerControllerManager ControllerManager;
    [SerializeField] private UpdateManager _updateManager;

    private void Awake()
    {
        ControllerManager = new PlayerControllerManager(gameObject);
        ControllerManager.AddController(new MovementController());
        ControllerManager.AddController(new TurnController());
        _updateManager.Subscribe(ControllerManager.Update, UpdateType.Update);
        
    }

    private void OnDestroy()
    {
        _updateManager.Unsubscribe(ControllerManager.Update, UpdateType.Update);
    }
}
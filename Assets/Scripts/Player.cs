using Jellybeans.Updates;
using PlayerControllers;
using PlayerControllers.Controllers;
using UnityEngine;

/// <summary>
/// Main player script
/// </summary>
public class Player : MonoBehaviour
{
    public PlayerStatContainer Stats;
    public PlayerControllerManager ControllerManager;

    [SerializeField] private UpdateManager _updateManager;

    private void Awake()
    {
        ControllerManager = new PlayerControllerManager(this);
        ControllerManager.AddController(new MovementController());
        ControllerManager.AddController(new TurnController());

        Stats.Init(this);
        _updateManager.Initialise(GameCore.Get.gameObject.GetComponent<UpdateRelay>());
        _updateManager.Subscribe(ControllerManager.Update, UpdateType.Update);
    }

    private void OnDestroy()
    {
        _updateManager.Unsubscribe(ControllerManager.Update, UpdateType.Update);
    }

}
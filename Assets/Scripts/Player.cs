using System;
using BikeMania;
using Jellybeans.Updates;
using PlayerControllers;
using PlayerControllers.Controllers;
using UnityEngine;
using Util.PostProcessingExtended;

/// <summary>
/// Main player script
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [NonSerialized] public Rigidbody Body;
    [NonSerialized] public CapsuleCollider CapCollider;

    [SerializeField] private MovementController _movement;
    [SerializeField] private TurnController _turning;

    public PlayerEnergy Energy;
    public PlayerStatContainer Stats;
    public PlayerControllerManager ControllerManager;
    public GroundedController GroundedController;
    public InputContainer Input;

    public PostProcessLerper PpLerper;

    [SerializeField] private UpdateManager _updateManager;

    private void Awake()
    {
        Body = GetComponent<Rigidbody>();
        CapCollider = GetComponent<CapsuleCollider>();

        Energy = new PlayerEnergy();

        Stats.Init(this);

        ControllerManager = new PlayerControllerManager(this);
        ControllerManager.AddController(_movement);
        ControllerManager.AddController(_turning);
        ControllerManager.AddController(new BoostController());
        ControllerManager.Init();

        _movement.AssignAnimator(GetComponentInChildren<Animator>());

        GroundedController = new(this);
    }

    private void Start()
    {
        Energy.Start(this);

        _updateManager.Subscribe(Energy.Update, UpdateType.Update);
        _updateManager.Subscribe(ControllerManager.Update, UpdateType.Update);
        _updateManager.Subscribe(ControllerManager.FixedUpdate, UpdateType.FixedUpdate);
    }

    private void OnDestroy()
    {
        _updateManager.Unsubscribe(Energy.Update, UpdateType.Update);
        _updateManager.Unsubscribe(ControllerManager.Update, UpdateType.Update);
        _updateManager.Unsubscribe(ControllerManager.FixedUpdate, UpdateType.FixedUpdate);
    }
}
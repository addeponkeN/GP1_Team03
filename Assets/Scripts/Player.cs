using System;
using BikeMania;
using Jellybeans.Updates;
using PlayerControllers;
using PlayerControllers.Controllers;
using UnityEngine;

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

    private void Reset()
    {
        return; //  temp
        CapCollider = GetComponent<CapsuleCollider>();
        Body = GetComponent<Rigidbody>();

        //  reset capsule collider
        CapCollider.center = new Vector3(-.42f, -0.065f, -0.039f);
        CapCollider.radius = 0.65f;
        CapCollider.height = 2.5f;

        //  reset rigidbody
        Body.useGravity = true;
        Body.isKinematic = false;

        Body.mass = 1;
        Body.drag = 0;
        Body.angularDrag = 1;

        Body.interpolation = RigidbodyInterpolation.Interpolate;
        Body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Body.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }
}
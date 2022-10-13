using System;
using Cinemachine;
using Jellybeans.Updates;
using PlayerControllers;
using PlayerControllers.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Main player script
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [NonSerialized] public Rigidbody Body;
    [NonSerialized] public CapsuleCollider CapCollider;

    public PlayerEnergy Energy;
    public PlayerStatContainer Stats;
    public PlayerControllerManager ControllerManager;
    public InputContainer Input;

    [SerializeField] private UpdateManager _updateManager;

    private void Awake()
    {
        Body = GetComponent<Rigidbody>();
        CapCollider = GetComponent<CapsuleCollider>();

        Energy = new PlayerEnergy();
        
        Stats.Init(this);

        ControllerManager = new PlayerControllerManager(this);
        ControllerManager.AddController(new MovementController());
        ControllerManager.AddController(new TurnController());
        ControllerManager.AddController(new BoostController());
        ControllerManager.Init();
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
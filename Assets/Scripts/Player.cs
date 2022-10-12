using System;
using Cinemachine;
using Jellybeans.Updates;
using PlayerControllers;
using PlayerControllers.Controllers;
using UnityEngine;

/// <summary>
/// Main player script
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [NonSerialized] public Rigidbody Body;
    
    public PlayerStatContainer Stats;
    public PlayerControllerManager ControllerManager;

    [SerializeField] private UpdateManager _updateManager;

    private void Awake()
    {
        Body = GetComponent<Rigidbody>();
        
        ControllerManager = new PlayerControllerManager(this);
        Stats.Init(this);
        ControllerManager.AddController(new MovementController());
        ControllerManager.AddController(new TurnController());
    }

    private void Start()
    {
        _updateManager.Subscribe(ControllerManager.Update, UpdateType.Update);
        _updateManager.Subscribe(ControllerManager.FixedUpdate, UpdateType.FixedUpdate);

    }

    private void OnDestroy()
    {
        _updateManager.Unsubscribe(ControllerManager.Update, UpdateType.Update);
        _updateManager.Unsubscribe(ControllerManager.FixedUpdate, UpdateType.FixedUpdate);
    }
}
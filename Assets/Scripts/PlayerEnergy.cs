using System;
using PlayerControllers.Controllers;
using UnityEngine;

public class EnergyInfo
{
    public float Value;
    public float Percentage;

    public EnergyInfo() { }
    public EnergyInfo(float value, float percentage)
    {
        Value = value;
        Percentage = percentage;
    }
}

public class PlayerEnergy
{
    public event Action<EnergyInfo> EnergyChangedEvent;

    public float Energy => _energy;
    
    private float EnergyPercentage => _energy / _stats.MaxEnergy;
    private float _energy;

    private EnergyInfo _info;
    private PlayerStatContainer _stats;

    private bool CanBoost() => _energy >= _stats.BoostCost;
    private bool IsEnergyMaxed() => _energy >= _stats.MaxEnergy;

    public void Start(Player player)
    {
        _info = new();
        _stats = player.Stats;
        
        SetEnergy(_stats.MaxEnergy);
        
        var boostController = player.ControllerManager.GetController<BoostController>();
        boostController.BoostedEvent += OnBoostedEvent;
        boostController.AttemptBoostEvent += BoostControllerOnAttemptBoostEvent;
    }

    private void BoostControllerOnAttemptBoostEvent(BoostController.BoostResponse response)
    {
        response.SetCanBoost(CanBoost());
    }

    private void SetEnergy(float value)
    {
        _energy = value;
        _info.Value = value;
        _info.Percentage = EnergyPercentage;
        EnergyChangedEvent?.Invoke(_info);
    }

    private void OnBoostedEvent()
    {
        SetEnergy(_energy - _stats.BoostCost);
    }

    public void Update(float dt)
    {
        if(!IsEnergyMaxed())
        {
            RegenerateEnergy(dt);
        }
    }

    private void RegenerateEnergy(float dt)
    {
        _energy += _stats.EnergyRegeneration * dt;
        SetEnergy(Mathf.Clamp(_energy, 0f, _stats.MaxEnergy));
    }
}
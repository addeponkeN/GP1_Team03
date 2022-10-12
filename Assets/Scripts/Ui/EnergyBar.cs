using PlayerControllers.Controllers;
using Ui.Widgets;
using UnityEngine;

namespace Ui
{
    public class EnergyBar : MonoBehaviour
    {
        private ProgressBar _progressBar;

        private void Awake()
        {
            _progressBar = GetComponent<ProgressBar>();
        }

        private void Start()
        {
            //  temporary check
            var player = GameCore.Get?.Player;
            if(player == null) return;

            var boostController = player.ControllerManager.GetController<BoostController>();
            boostController.BoostedEvent += OnBoostedEvent;
            boostController.AttemptBoostEvent += OnAttemptBoostEvent;
            
            SetEnergy(_maxEnergy);
        }

        private void OnAttemptBoostEvent(BoostController.BoostResponse response)
        {
            response.SetCanBoost(CanBoost());
        }

        private void OnBoostedEvent()
        {
            SetEnergy(_energy - _boostCost);
        }

        //  TEMPORARY
        private void SetEnergy(float value)
        {
            _energy = value;
            _progressBar.Value = EnergyPercentage;
        }


//  TEMPORARY ENERGY

        bool CanBoost() => _energy >= _boostCost;

        private float EnergyPercentage => _energy / _maxEnergy;
        private float _energy;
        private float _maxEnergy = 100f;
        private float _energyRegenerateRate = 2f;
        private float _boostCost = 20f;

        private void Update()
        {
            if(_energy < _maxEnergy)
            {
                _energy += _energyRegenerateRate * Time.deltaTime;
                SetEnergy(Mathf.Clamp(_energy, 0f, _maxEnergy));
            }
        }
    }
}
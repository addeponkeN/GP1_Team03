using Ui.Widgets;
using UnityEngine;

namespace Ui
{
    public class EnergyBar : MonoBehaviour
    {
        [SerializeField] private Player _player;
        private ProgressBar _progressBar;

        private void Awake()
        {
            _progressBar = GetComponent<ProgressBar>();
        }

        private void Start()
        {
            // _progressBar.Value = _player.Energy.Energy;
            // _player.Energy.EnergyChangedEvent += EnergyOnEnergyChangedEvent;
        }

        private void EnergyOnEnergyChangedEvent(EnergyInfo energyInfo)
        {
            _progressBar.Value = energyInfo.Percentage;
        }
    }
}
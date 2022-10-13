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

            player.Energy.EnergyChangedEvent += EnergyOnEnergyChangedEvent;
        }

        private void EnergyOnEnergyChangedEvent(EnergyInfo energyInfo)
        {
            _progressBar.Value = energyInfo.Percentage;
        }
    }
}
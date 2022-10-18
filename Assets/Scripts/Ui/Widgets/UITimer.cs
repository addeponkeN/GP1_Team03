using TMPro;
using UnityEngine;

namespace Ui.Widgets
{
    public class UITimer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private Util.Timer _timer;

        private void Start()
        {
            // _timer.OnIntervalEvent += TimerOnOnIntervalEvent;
        }

        private void TimerOnOnIntervalEvent(Util.Timer timer)
        {
            // _text.text = $"{(int)timer}";
        }
    }
}
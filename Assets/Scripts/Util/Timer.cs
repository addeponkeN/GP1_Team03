using System;
using System.Diagnostics;

namespace Util
{
    [Serializable]
    public class Timer
    {
        public static implicit operator float(Timer timer) => timer.Time;
        
        private float _intervalTimer;
        private float _timer;
        private bool _done;

        [NonSerialized] public float Interval = -1f;

        public event Action<Timer> OnDoneEvent;
        public event Action<Timer> OnIntervalEvent;

        public float Time => _timer;

        public Timer(float time) : this(time, -1f) { }
        public Timer(float time, float updateInterval)
        {
            _timer = time;
            Interval = updateInterval;
        }

        public void Update(float dt)
        {
            if(_done) return;

            if(Interval >= 0f)
            {
                _intervalTimer -= dt;
                if(_intervalTimer <= 0f)
                {
                    OnIntervalEvent?.Invoke(this);
                    _intervalTimer += Interval;
                }
            }

            _timer -= dt;
            if(_timer <= 0)
            {
                OnDoneEvent?.Invoke(this);
            }
        }
    }
}
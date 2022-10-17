using System;

namespace Util
{
    public class Timer
    {
        private float _timer;
        private bool _done;

        public event Action<Timer> OnDoneEvent;

        public Timer(float time)
        {
            _timer = time;
        }

        public void Update(float dt)
        {
            if(_done) return;
            
            _timer -= dt;
            if(_timer <= 0)
            {
                OnDoneEvent?.Invoke(this);
            }
        }
    }
}
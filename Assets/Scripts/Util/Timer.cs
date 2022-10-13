
using System;

namespace Util
{
    public class Timer
    {
        private float _timer;
        private float _time;

        public event Action<Timer> DoneEvent;
    
        public Timer(float time)
        {
            _time = time;
            _timer = time;
        }

        public void Update(float dt)
        {
            _timer -= dt;
            if(_timer <= 0)
            {
                DoneEvent?.Invoke(this);
            }
        }
    
    }
}
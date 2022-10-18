using System;

namespace Util
{
    /// <summary>
    /// yes
    /// </summary>
    public class Accelerator
    {
        public float Speed => _speed;

        public float Acceleration;
        public float Deceleration;
        public float MaxSpeed;
        public float Sensitivity;

        private float _speed;

        public Accelerator(float acceleration, float maxSpeed, float sensitivity = 0.25f)
        {
            Acceleration = acceleration;
            Deceleration = acceleration;
            MaxSpeed = maxSpeed;
            Sensitivity = sensitivity;
        }

        public Accelerator(float acceleration, float deceleration, float maxSpeed, float sensitivity = 0.25f)
        {
            Acceleration = acceleration;
            Deceleration = deceleration;
            MaxSpeed = maxSpeed;
            Sensitivity = sensitivity;
        }

        private float AbsoluteSpeed => MathF.Abs(_speed);
        private float AbsoluteMaxSpeed => MathF.Abs(MaxSpeed);

        public bool IsAccelerating()
            => AbsoluteSpeed > Sensitivity;

        private bool IsSpeedAboveMaxSpeed()
            => AbsoluteSpeed > AbsoluteMaxSpeed;

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        private void Decelerate(float dt)
        {
            //  if acceleration is less than SENSITIVITY, force stop
            //  to stop any micro movement jitter
            if(AbsoluteSpeed < Sensitivity)
            {
                _speed = 0;
            }
            else
            {
                //  deaccelerate
                var accelNormalized = OmegaMathf.Normalize(_speed);
                _speed += Deceleration * -accelNormalized * dt;
            }
        }

        public void Update(float dt, float direction)
        {
            if(direction == 0 || IsSpeedAboveMaxSpeed())
            {
                Decelerate(dt);
            }
            else
            {
                if(_speed > 0 && direction < 0 || _speed < 0 && direction > 0)
                {
                    Decelerate(dt);
                }
                else
                {
                    //  accelerate
                    _speed += direction * Acceleration * dt;
                }
            }
        }

        public void Stop()
        {
            _speed = 0;
        }
    }
}
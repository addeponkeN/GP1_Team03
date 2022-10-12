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
        public float MaxSpeed;
        public float Sensitivity;

        private float _speed;

        public Accelerator(float acceleration, float maxSpeed, float sensitivity = 0.1f)
        {
            Acceleration = acceleration;
            MaxSpeed = maxSpeed;
            Sensitivity = sensitivity;
        }

        private float AbsoluteSpeed => MathF.Abs(_speed);
        private float AbsoluteMaxSpeed => MathF.Abs(MaxSpeed);

        public bool IsAccelerating()
            => AbsoluteSpeed > Sensitivity;

        private bool IsSpeedAboveMaxSpeed()
            => AbsoluteSpeed > AbsoluteMaxSpeed;

        private void Brake(float dt)
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
                _speed += Acceleration * -accelNormalized * dt;
            }
        }

        public void Update(float dt, float direction)
        {
            if(direction == 0 || IsSpeedAboveMaxSpeed())
            {
                Brake(dt);
            }
            else
            {
                //  accelerate
                _speed += direction * Acceleration * dt;
            }
        }
    }
}
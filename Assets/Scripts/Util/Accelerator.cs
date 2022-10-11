using UnityEngine;

namespace Util
{
    public class Accelerator
    {
        public float Speed => _speed;

        public float Acceleration;
        public float MaxSpeed;
        public float Sensitivity;

        private float _speed;

        public Accelerator(float acceleration, float maxSpeed)
        {
            Acceleration = acceleration;
            MaxSpeed = maxSpeed;
            Sensitivity = 0.1f;
        }

        private float AbsoluteSpeed()
            => Mathf.Abs(_speed);

        public bool IsAccelerating()
            => AbsoluteSpeed() > Sensitivity;

        public void Update(float dt, float direction)
        {
            float absoluteSpeed = AbsoluteSpeed();

            if(direction == 0)
            {
                //  if acceleration is less than SENSITIVITY, force stop
                if(absoluteSpeed < Sensitivity)
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
            else
            {
                //  add acceleration towards direction (input)
                _speed += direction * Acceleration * dt;
            }

            _speed = Mathf.Clamp(_speed, -MaxSpeed, MaxSpeed);
        }
    }
}
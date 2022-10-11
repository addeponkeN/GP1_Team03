using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace PlayerControllers.Controllers
{
    public class MovementController : BasePlayerController
    {
        public float Speed => _acceleration;
        public float MaxSpeed => _maxAcceleration;
        
        private float _acceleration;
        private float _accelSpeed = 5f;
        private float _maxAcceleration = 10f;

        public override void Update(float delta)
        {
            base.Update(delta);

            var player = Manager.PlayerGo;
            var dt = Time.deltaTime;
            var fw = player.transform.forward;

            float dir = Input.GetAxisRaw("Vertical");
            float accelerationAbs = Mathf.Abs(_acceleration);

            if(dir == 0)
            {
                //  if acceleration is less than 1 or more than -1, make sure to stop
                if(accelerationAbs < 1f)
                {
                    _acceleration = 0;
                }
                else
                {
                    //  brake
                    var accelNormalized = OmegaMathf.Normalize(_acceleration);
                    _acceleration += _accelSpeed * -accelNormalized * dt;
                }
            }
            else
            {
                //  add acceleration from input (dir)
                _acceleration += dir * _accelSpeed * dt;
            }

            if(accelerationAbs > 0.1f)
            {
                _acceleration = Mathf.Clamp(_acceleration, -_maxAcceleration, _maxAcceleration);

                var move = fw * (_acceleration * dt);
                player.transform.position += move;
            }
        }
    }
}
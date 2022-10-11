using UnityEngine;
using Util;

namespace PlayerControllers.Controllers
{
    public class MovementController : BasePlayerController
    {
        public float Speed => _accelerator.Speed;
        public float MaxSpeed => _accelerator.MaxSpeed;

        private Accelerator _accelerator;

        public override void Init()
        {
            base.Init();
            
            //  todo - get stats from player
            float accelerationSpeed = 5f;
            float maxAccelerationSpeed = 10f;

            _accelerator = new Accelerator(accelerationSpeed, maxAccelerationSpeed);
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            float dir = Input.GetAxisRaw("Vertical");
            _accelerator.Update(delta, dir);

            if(_accelerator.IsAccelerating())
            {
                var player = Manager.PlayerGo;
                var fw = player.transform.forward;
                
                var move = fw * (_accelerator.Speed * delta);
                player.transform.position += move;
            }
        }
    }
}
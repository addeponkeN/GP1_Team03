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
            
            var stats = Manager.Player.Stats;
            _accelerator = new Accelerator(
                stats.MovementAcceleration, 
                stats.MaxMoveSpeed);
            
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
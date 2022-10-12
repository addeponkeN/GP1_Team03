using UnityEngine;
using Util;

namespace PlayerControllers.Controllers
{
    public class MovementController : BasePlayerController
    {
        public float Speed => _accelerator.Speed;
        public float MaxSpeed => _accelerator.MaxSpeed;

        private Accelerator _accelerator;
        private PlayerStatContainer _stats;

        public override void Init()
        {
            base.Init();
            _stats = Manager.Player.Stats;
            _accelerator = new Accelerator(
                _stats.MovementAcceleration, 
                _stats.MaxMoveSpeed);
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            var stats = Manager.Player.Stats;
            _accelerator.Acceleration = stats.MovementAcceleration;
            _accelerator.MaxSpeed = stats.MaxMoveSpeed;
            
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
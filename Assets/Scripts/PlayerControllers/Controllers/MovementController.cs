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

        public override void FixedUpdate(float dt)
        {
            base.FixedUpdate(dt);
            var stats = Manager.Player.Stats;
            _accelerator.Acceleration = stats.MovementAcceleration * 10f;
            _accelerator.MaxSpeed = stats.MaxMoveSpeed * 10f;

            float dir = Input.GetAxisRaw("Vertical");
            _accelerator.Update(dt, dir);

            if(_accelerator.IsAccelerating())
            {
                var body = Manager.Player.Body;
                var tf = body.transform;
                var fw = tf.forward;
                fw = new Vector3(fw.x, 0, fw.z);

                var move = fw * (_accelerator.Speed * dt);
                body.velocity *= 0.5f;
                body.AddForce(move, ForceMode.VelocityChange);
            }
        }
    }
}
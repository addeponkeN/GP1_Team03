using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace PlayerControllers.Controllers
{
    public class MovementController : BasePlayerController
    {
        public float Speed => _accelerator.Speed;
        public float MaxSpeed => _accelerator.MaxSpeed;

        private Accelerator _accelerator;
        private PlayerStatContainer _stats;
        private InputActionReference _inputMove;

        public override void Init()
        {
            base.Init();
            _stats = Manager.Player.Stats;
            _accelerator = new Accelerator(
                _stats.MovementAcceleration,
                _stats.MaxMoveSpeed);
            _accelerator.Sensitivity = 0.25f;

            _inputMove = Manager.Player.Input.Movement;
            _inputMove.action.Enable();
        }

        public override void FixedUpdate(float dt)
        {
            base.FixedUpdate(dt);

            var dir = _inputMove.action.ReadValue<Vector2>().y;
            
            var stats = Manager.Player.Stats;
            _accelerator.Acceleration = stats.MovementAcceleration;
            _accelerator.MaxSpeed = stats.MaxMoveSpeed;

            _accelerator.Update(dt, dir);

            if(_accelerator.IsAccelerating())
            {
                var body = Manager.Player.Body;
                var tf = body.transform;
                var fw = tf.forward;
                fw = new Vector3(fw.x, 0, fw.z);

                var move = fw * (_accelerator.Speed * dt);
                body.velocity *= 0.5f;
                body.MovePosition(tf.position + move);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace PlayerControllers.Controllers
{
    public class MovementController : BasePlayerController
    {
        public float Speed => _accelerator.Speed;
        public float MaxSpeed => _accelerator.MaxSpeed;

        public float MoveSpeedMultiplier { get; set; } = 1f;

        private Accelerator _accelerator;
        private PlayerStatContainer _stats;
        private InputActionReference _inputMove;

        public override void Init()
        {
            base.Init();
            _stats = Manager.Player.Stats;

            const float acceleratorBreakSensitivity = 0.25f;
            
            _accelerator = new Accelerator(
                _stats.MovementAcceleration,
                _stats.MaxMoveSpeed,
                acceleratorBreakSensitivity);

            _inputMove = Manager.Player.Input.Movement;
            _inputMove.action.Enable();
        }

        public override void FixedUpdate(float dt)
        {
            base.FixedUpdate(dt);

            var dir = _inputMove.action.ReadValue<Vector2>().y;
            
            //  fetch the latest player stats and apply to the controller
            _accelerator.Acceleration = _stats.MovementAcceleration * MoveSpeedMultiplier;
            _accelerator.MaxSpeed = _stats.MaxMoveSpeed * MoveSpeedMultiplier;

            _accelerator.Update(dt, dir);

            if(_accelerator.IsAccelerating())
            {
                var body = Manager.Player.Body;
                var tf = body.transform;
                var fw = tf.forward;
                fw.y = 0;

                var move = fw * (_accelerator.Speed * dt);
                body.velocity *= 0.5f;
                body.MovePosition(tf.position + move);
            }
        }
    }
}
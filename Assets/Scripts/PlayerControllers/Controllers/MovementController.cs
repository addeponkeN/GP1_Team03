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

            _inputMove = Manager.Player.Input.Movement;
            
            // _inputMove.action.ca
        }

        public override void SetEnabled(bool enabled)
        {
            base.SetEnabled(enabled);
            // if(enabled)
            // {
                // _inputMove.action.
            // }
            // else
            // {
                // _inputMove.action.Disable();
            // }
        }

        public override void FixedUpdate(float dt)
        {
            base.FixedUpdate(dt);

            var stats = Manager.Player.Stats;
            _accelerator.Acceleration = stats.MovementAcceleration;
            _accelerator.MaxSpeed = stats.MaxMoveSpeed;

            float dir = _inputMove.action.ReadValue<Vector2>().x;
            Debug.Log(dir);
            
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
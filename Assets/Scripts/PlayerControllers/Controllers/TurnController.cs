using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace PlayerControllers.Controllers
{
    public class TurnController : BasePlayerController
    {
        private MovementController _movement;
        private Accelerator _accelerator;
        private PlayerStatContainer _stats;
        private InputActionReference _input;

        /// <summary>
        /// How much movement speed slows down the rotation speed
        /// Between 0 to 1
        /// todo - Move this to player stat container layer
        /// </summary>
        private float MovementTurnSpeedModifier => _stats.RotationSpeedModifier;

        public override void Init()
        {
            base.Init();
            _movement = Manager.GetController<MovementController>();
            _input = Manager.Input.Movement;
            _input.action.Enable();

            _stats = Manager.Player.Stats;
            _accelerator = new Accelerator(
                _stats.RotationSpeed * 10f,
                _stats.MaxRotationSpeed * 10f, 
                5f);
        }

        public override void FixedUpdate(float fixedDelta)
        {
            base.FixedUpdate(fixedDelta);
            
            float dir = _input.action.ReadValue<Vector2>().x;
            
            _accelerator.Update(fixedDelta, dir);
            _accelerator.Acceleration = _stats.RotationSpeed * 10f;
            _accelerator.Deceleration = _stats.RotationSpeed * 10f * 2f;
            _accelerator.MaxSpeed = _stats.MaxRotationSpeed * 10f;

            if(_accelerator.IsAccelerating())
            {
                var body = Manager.Player.Body;

                float movementModifier = 1f - (_movement.Speed * MovementTurnSpeedModifier / _movement.MaxSpeed);
                float finalTurnSpeed = _accelerator.Speed * movementModifier;
                
                body.MoveRotation(body.rotation * Quaternion.AngleAxis(finalTurnSpeed * fixedDelta, Vector3.up));
            }
        }
    }
}
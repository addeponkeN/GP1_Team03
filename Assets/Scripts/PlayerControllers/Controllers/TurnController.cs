using UnityEngine;
using Util;

namespace PlayerControllers.Controllers
{
    public class TurnController : BasePlayerController
    {
        private MovementController _movement;
        private Accelerator _accelerator;
        private PlayerStatContainer _stats;

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

            _stats = Manager.Player.Stats;
            _accelerator = new Accelerator(
                _stats.RotationSpeed * 10f,
                _stats.MaxRotationSpeed * 10f);
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            float dir = Input.GetAxisRaw("Horizontal");
            _accelerator.Update(delta, dir);
            _accelerator.Acceleration = _stats.RotationSpeed;
            _accelerator.MaxSpeed = _stats.MaxRotationSpeed;

            if(_accelerator.IsAccelerating())
            {
                var player = Manager.PlayerGo;

                float movementModifier = 1f - (_movement.Speed * MovementTurnSpeedModifier / _movement.MaxSpeed);
                float finalTurnSpeed = _accelerator.Acceleration * movementModifier * dir;

                player.transform.rotation *= Quaternion.AngleAxis(finalTurnSpeed * delta, Vector3.up);
            }
        }
    }
}
using UnityEngine;

namespace PlayerControllers.Controllers
{
    public class TurnController : BasePlayerController
    {
        private float _rotationSpeed = 100f;
        private MovementController _movement;

        public override void Init()
        {
            base.Init();
            _movement = Manager.GetController<MovementController>();
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            float dir = Input.GetAxisRaw("Horizontal");
            if(dir == 0)
                return;

            float dt = Time.deltaTime;
            var player = Manager.PlayerGo;

            float movementTurnSpeed = 1f - (_movement.Speed * 0.5f / _movement.MaxSpeed);
            float finalTurnSpeed = _rotationSpeed * movementTurnSpeed * dir;
            
            player.transform.rotation *= Quaternion.AngleAxis(finalTurnSpeed * dt, Vector3.up);
        }
    }
}
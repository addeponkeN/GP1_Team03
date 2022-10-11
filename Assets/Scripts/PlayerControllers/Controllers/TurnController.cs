using UnityEngine;

namespace PlayerControllers.Controllers
{
    public class TurnController : BasePlayerController
    {
        private float _rotationSpeed = 100f;

        private MovementController movement;
        
        public override void Init()
        {
            base.Init();
            movement = Manager.GetController<MovementController>();
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            //  NICE

            float dir = Input.GetAxisRaw("Horizontal");
            if(dir == 0)
                return;

            float dt = Time.deltaTime;
            var player = Manager.PlayerGo;

            float movementTurnSpeed = 1f - (movement.Speed * 0.5f / movement.MaxSpeed);
            float finalTurnSpeed = _rotationSpeed * movementTurnSpeed * dir;

            Debug.Log($"Speed: {(movement.Speed / movement.MaxSpeed) * 100f}%");
            
            player.transform.rotation *= Quaternion.AngleAxis(finalTurnSpeed * dt, Vector3.up);
            
            
        }
    }
}
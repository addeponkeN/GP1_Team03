using UnityEngine;

namespace PlayerControllers.Controllers
{
    public class BoostController : BasePlayerController
    {
        public bool IsBoosting => _boostTimer > 0f;
        
        //  temporary cooldown timer
        private float _cooldownTimer;
        private float _boostTimer;
        
        private MovementController _move;
        private PlayerStatContainer _stats;
        
        public override void Init()
        {
            base.Init();
            _move = Manager.GetController<MovementController>();
            _stats = Manager.Player.Stats;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartBoost();
            }
            
        }
        
        bool CanBoost()
        {
            // _boostTimer = 
            return false;
        }

        private void StartBoost()
        {
            _boostTimer = _stats.BoostTime;
        }

        private void EndBoost()
        {
            
        }
        
    }
}
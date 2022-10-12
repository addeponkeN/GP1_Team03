using System;
using UnityEngine.InputSystem;

namespace PlayerControllers.Controllers
{
    public class BoostController : BasePlayerController
    {
        public bool IsBoosting => _boostTimer > 0f;

        public event Action BoostedEvent;
        
        private float _boostTimer;
        private bool _boosted;

        private MovementController _move;
        private PlayerStatContainer _stats;
        private InputActionReference _input;
        
        public override void Init()
        {
            base.Init();
            _move = Manager.GetController<MovementController>();
            _stats = Manager.Player.Stats;
            _input = Manager.Input.Boost;

            _input.action.Enable();
            _input.action.performed += ActionOnPerformed;
        }

        public override void SetEnabled(bool enabled)
        {
            base.SetEnabled(enabled);
            if(!enabled)
                EndBoost();
        }

        private void ActionOnPerformed(InputAction.CallbackContext context)
        {
            if(CanBoost())
            {
                StartBoost();
            }
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            if(_boostTimer > 0)
                _boostTimer -= delta;

            if(!IsBoosting && _boosted != IsBoosting)
            {
                EndBoost();
            }

            _boosted = IsBoosting;
        }

        private bool CanBoost()
        {
            return !IsBoosting;
        }

        private void StartBoost()
        {
            _boostTimer = _stats.BoostTime;
            _move.MoveSpeedMultiplier = 3f;
            BoostedEvent?.Invoke();
        }

        private void EndBoost()
        {
            _move.MoveSpeedMultiplier = 1f;
        }

        public override void Exit()
        {
            base.Exit();
            _input.action.performed -= ActionOnPerformed;
        }
    }
}
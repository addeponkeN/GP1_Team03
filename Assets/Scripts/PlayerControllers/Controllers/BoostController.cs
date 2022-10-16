using System;
using UnityEngine.InputSystem;

namespace PlayerControllers.Controllers
{
    public class BoostController : BasePlayerController
    {
        public class BoostResponse
        {
            public bool CanBoost { get; private set; }

            public void SetCanBoost(bool canBoost)
            {
                CanBoost = canBoost;
            }
        }

        public bool IsBoosting => _boostTimer > 0f;

        public event Action BoostedEvent;
        public event Action<BoostResponse> AttemptBoostEvent;

        private float _boostTimer;
        private bool _boosted;

        private BoostResponse _boostRespone;
        private MovementController _move;
        private PlayerStatContainer _stats;
        private InputActionReference _input;

        public override void Init()
        {
            base.Init();
            _boostRespone = new();
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
            if(!Enabled) return false;
            if(IsBoosting) return false;

            _boostRespone.SetCanBoost(true);
            AttemptBoostEvent?.Invoke(_boostRespone);

            return _boostRespone.CanBoost;
        }

        private void StartBoost()
        {
            _boostTimer = _stats.BoostTime;
            _move.SetMultipliers(_stats.BoostAmount * 2f, _stats.BoostAmount);
            BoostedEvent?.Invoke();
        }

        private void EndBoost()
        {
            _move.SetMultipliers();
        }

        public override void Exit()
        {
            base.Exit();
            _input.action.performed -= ActionOnPerformed;
        }
    }
}
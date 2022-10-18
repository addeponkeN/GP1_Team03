using PlayerControllers.Controllers;
using UnityEngine;

namespace BikeMania
{
    public class GroundedController
    {
        private static readonly int OnJump = Animator.StringToHash("OnJump");

        private MovementController _move;
        private Animator _animator;
        private bool _isDisabledUntilGrounded;

        public GroundedController(Player player)
        {
            _move = player.ControllerManager.GetController<MovementController>();
            _animator = player.GetComponentInChildren<Animator>();

            var groundChecker = player.GetComponent<GroundChecker>();
            groundChecker.OnGroundedChangedEvent += OnGroundedChangedEvent;
        }

        public void DisableMovementUntilGrounded()
        {
            _isDisabledUntilGrounded = true;
            _move.Stop();
            _move.Enabled = false;
            _animator.speed = 1f;
            _animator.SetBool(OnJump, true);
        }

        private void OnGroundedChangedEvent(bool isGrounded)
        {
            if(_isDisabledUntilGrounded && isGrounded)
            {
                _animator.speed = 1f;
                _animator.SetBool(OnJump, false);
                _move.Enabled = true;
                _move.Accelerator.SetSpeed(_move.MaxSpeed);
                _isDisabledUntilGrounded = false;
            }
        }
    }
}
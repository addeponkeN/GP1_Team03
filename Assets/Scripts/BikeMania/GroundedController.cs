using PlayerControllers.Controllers;

namespace BikeMania
{
    public class GroundedController
    {
        private MovementController _move;
        private bool _isDisabledUntilGrounded;

        public GroundedController(Player player)
        {
            _move = player.ControllerManager.GetController<MovementController>();

            var groundChecker = player.GetComponent<GroundChecker>();
            groundChecker.OnGroundedChangedEvent += OnGroundedChangedEvent;
        }

        public void DisableMovementUntilGrounded()
        {
            _isDisabledUntilGrounded = true;
            _move.Stop();
            _move.Enabled = false;
        }

        private void OnGroundedChangedEvent(bool isGrounded)
        {
            if(_isDisabledUntilGrounded && isGrounded)
            {
                _move.Enabled = true;
                _isDisabledUntilGrounded = false;
            }
        }
    }
}
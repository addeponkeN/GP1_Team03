using UnityEngine;

namespace PlayerPush
{
    public class PlayerPusher : MonoBehaviour
    {

        /// <summary>
        /// On/off switch for the pusher
        /// </summary>
        [SerializeField] private bool _isActivated = true;
        
        /// <summary>
        /// Destination
        /// </summary>
        [SerializeField] private Transform _destination;

        private LayerMask _playerMask = 1 << 6;

        private void OnTriggerEnter(Collider other)
        {
            if(!_isActivated) return;
            if(((1 << other.gameObject.layer) & _playerMask) == 0) return;

            //  get player component
            var player = other.gameObject.GetComponent<Player>();
            var body = player.Body;

            //  set locals
            var dest = _destination.position;
            var pos = transform.position;
            var playerPos = player.transform.position;
            
            var direction = (dest - playerPos).normalized;
            var distance = Vector2.Distance(
                new Vector2(dest.x, dest.z),
                new Vector2(playerPos.x, playerPos.z));

            const float magicalMathNumber = 16.666f;

            //  the Y difference of the PAD and DESTINATION
            float diffY = dest.y - pos.y;

            //  the amount of force to push up (Y)
            float powerUp = diffY * magicalMathNumber;

            //  the amount of force to push towards destination (XZ)
            float power = distance * magicalMathNumber;

            //  Stop the player for the duration when flying
            StopPlayer(player);

            //  Move player a small amount towards direction to avoid groundcheck bugs stuff
            body.MovePosition(body.position + Vector3.up * 0.1f);

            //  reset the velocity to ensure the force amount is always the same
            var bodyVel = body.velocity;
            bodyVel.y = 0;
            body.velocity = bodyVel;

            //  Push player towards the final destination
            body.AddForce(Vector3.up * powerUp + direction * power, ForceMode.Impulse);
        }

        /// <summary>
        /// Stop and disable the player 
        /// </summary>
        /// <param name="player"></param>
        private void StopPlayer(Player player)
        {
            player.GroundedController.DisableMovementUntilGrounded();
        }
        
        public void SetActivated(bool isActivated)
        {
            _isActivated = isActivated;
        }
        
    }
}
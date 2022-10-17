using PlayerControllers.Controllers;
using UnityEngine;

namespace PlayerPush
{
    public class PlayerPusher : MonoBehaviour
    {
        /// <summary>
        /// Direction of the pusher
        /// </summary>
        public Vector3 Direction = Vector3.up;

        /// <summary>
        /// The push force
        /// </summary>
        public float Power = 30f;

        /// <summary>
        /// The distance the player be pushed 
        /// </summary>
        public float Distance = 5f;


        [SerializeField] private Transform Destination;

        private LayerMask _playerMask = 1 << 6;

        private void OnTriggerEnter(Collider other)
        {
            if(((1 << other.gameObject.layer) & _playerMask) == 0)
                return;

            var player = other.gameObject.GetComponent<Player>();

            var dest = Destination.position;
            var pos = transform.position;
            var direction = (dest - pos).normalized;
            
            float distance = Vector2.Distance(
                new Vector2(dest.x, dest.z), 
                new Vector2(pos.x, pos.z));

            float diffY = dest.y - pos.y;
            float powerUp = diffY * 16.666f;
            float power = distance * 16.666f;
            
            var body = player.GetComponent<Rigidbody>();

            //  Stop the player for the duration when flying
            StopPlayer(player);

            //  Move player a small amount towards direction
            body.MovePosition(body.position + direction * 0.1f);
            
            //  reset the velocity to ensure the force amount
            var bodyVel = body.velocity;
            bodyVel.y = 0;
            body.velocity = bodyVel;
            
            //  Push player towards destination
            body.AddForce(Vector3.up * powerUp + direction * power, ForceMode.Impulse);
        }

        private void StopPlayer(Player player)
        {
            var move = player.ControllerManager.GetController<MovementController>();
            move.Stop();
            move.Enabled = false;
        }
    }
}
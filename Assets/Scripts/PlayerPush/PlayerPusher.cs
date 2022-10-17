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

        private LayerMask _playerMask = 1 << 6;

        private void OnTriggerEnter(Collider other)
        {
            if(((1 << other.gameObject.layer) & _playerMask) == 0)
                return;

            var player = other.gameObject.GetComponent<Player>();
            StopPlayer(player);

            var pushDirection = transform.forward;

            var finalDirection = (pushDirection + Direction).normalized;
            
            var body = player.GetComponent<Rigidbody>();
            body.MovePosition(body.position + finalDirection * 0.1f);
            body.AddForce(finalDirection * Power, ForceMode.Impulse);
        }

        private void StopPlayer(Player player)
        {
            var move = player.ControllerManager.GetController<MovementController>();
            move.Stop();
            // move.Enabled = false;
        }
    }
}
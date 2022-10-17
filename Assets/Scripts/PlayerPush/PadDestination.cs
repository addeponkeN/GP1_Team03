using UnityEngine;

namespace PlayerPush
{
    public class PadDestination : MonoBehaviour
    {
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}

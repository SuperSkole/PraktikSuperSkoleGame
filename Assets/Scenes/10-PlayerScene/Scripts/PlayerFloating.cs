using UnityEngine;

namespace Scenes._10_PlayerScene.Scripts
{

    public class PlayerFloating : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        [SerializeField] private float rideHeight = 2f;
        [SerializeField] private float rideSpringStrength = 1f;
        [SerializeField] private float rideSpringDamper = 1f;

        private void FixedUpdate()
        {
            Floating();
        }

        /// <summary>
        /// makes the player "float" a bit over the ground with a springy effect, made to fix player "jomping" when mooving over lips/steps
        /// </summary>
        private void Floating()
        {
            bool rayDidHit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, rideHeight * 2);
            if (rayDidHit && !hit.collider.isTrigger)
            {

                //fancy magic vector math that "just works"(tm)  (sofie)
                Vector3 vel = rb.velocity;
                Vector3 rayDir = Vector3.down;

                Vector3 otherVel = Vector3.zero;

                float rayDirVel = Vector3.Dot(rayDir, vel);
                float otherDirVel = Vector3.Dot(rayDir, otherVel);

                float relVel = rayDirVel - otherDirVel;

                float x = hit.distance - rideHeight;

                float springForce = (x * rideSpringStrength) - (relVel * rideSpringDamper);

                Debug.DrawLine(transform.position, transform.position + (rayDir * springForce), Color.yellow);

                rb.AddForce(rayDir * springForce);
            }
        }
    }
}

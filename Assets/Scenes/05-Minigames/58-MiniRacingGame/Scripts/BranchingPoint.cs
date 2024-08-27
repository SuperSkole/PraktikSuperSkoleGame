using UnityEngine;

namespace RacingGame
{
    public class BranchingPoint : MonoBehaviour
    {
        public RacingGameCore racingGameCore; // Reference to the RacingGameCore script

        /// <summary>
        /// Checks the branch is triggered by the player car.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ActiveCar"))
            {
                // Notify the RacingGameCore about which branch was triggered
                racingGameCore.BranchTriggered(gameObject.name);
            }
        }
    }
}

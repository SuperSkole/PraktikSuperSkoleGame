using UnityEngine;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    public class BranchingPoint : MonoBehaviour
    {
        public RacingCore racingGameCore; // Reference to the RacingGameCore script

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

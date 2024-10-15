using UnityEngine;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    public class RacingBranch : MonoBehaviour
    {
        public delegate void TriggerHit(string branchName);
        public static event TriggerHit OnBranchTriggered;
        private bool exited = false;
        public enum Branch { Left, Right }
        [SerializeField]
        private Branch currentBranch;
        private readonly string branch;
        /// <summary>
        /// Triggers the racing core branch code
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                if (!exited)
                {
                    exited = true;
                    OnBranchTriggered(currentBranch.ToString());
                }
            }
        }
    }
}

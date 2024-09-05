using System.Collections;
using UnityEngine;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    public class TriggerExit : MonoBehaviour
    {
        public float delay = 5f;

        public delegate void ExitAction();
        public static event ExitAction OnChunkExited;

        private bool exited = false;

        /// <summary>
        /// Checks when a player leaves a chunk, then sets a coroutine to remove it.
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("ActiveCar"))
            {
                if (!exited)
                {
                    exited = true;
                    OnChunkExited();
                    StartCoroutine(WaitAndDeactivate());
                }
            }
        }

        /// <summary>
        /// Waits a bit, then removes a chunk
        /// </summary>
        private IEnumerator WaitAndDeactivate()
        {
            yield return new WaitForSeconds(delay);

            transform.root.gameObject.SetActive(false);
        }
    }
}

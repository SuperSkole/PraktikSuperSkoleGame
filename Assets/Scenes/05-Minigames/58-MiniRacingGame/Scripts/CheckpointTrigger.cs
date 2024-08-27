using UnityEngine;

namespace Scenes.Minigames.MiniRacingGame
{
    public class CheckpointTrigger : MonoBehaviour
    {
        public RacingGameCore racingGameCore;
        public RacingGameCore.Checkpoint checkpointType;
        private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

        /// <summary>
        /// When waking up, get renderer.
        /// </summary>
        private void Awake()
        {
            // Get the SpriteRenderer component attached to this checkpoint
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// When hit by the player car, do a checkpoint check.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ActiveCar"))
            {
                racingGameCore.CheckpointTriggered(checkpointType);
                racingGameCore.passedCheckpointTrans = transform;
            }
        }

        /// <summary>
        /// Sets visibility of sprite renderer.
        /// </summary>
        /// <param name="isVisible">Should it be visible</param>
        public void SetVisible(bool isVisible)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = isVisible;
            }
        }
    }
}

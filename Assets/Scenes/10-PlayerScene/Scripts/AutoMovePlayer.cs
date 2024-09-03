using System;
using System.Collections;
using UnityEngine;

namespace Scenes._10_PlayerScene.Scripts
{
    /// <summary>
    /// Handles automatic movement of the player character to a specified position or the closest target.
    /// </summary>
    public class AutoMovePlayer : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5.0f;
        public Vector3 DropOffPoint;
        private GameObject spawnedPlayer;

        private void Awake()
        {
            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
        }

        private void OnEnable()
        {
            PlayerEvents.OnMovePlayerToBlock += MovePlayerToBlockAndPickUpBlock;
        }

        private void OnDisable()
        {
            PlayerEvents.OnMovePlayerToBlock -= MovePlayerToBlockAndPickUpBlock;
        }
        
        private void OnDestroy()
        {
            PlayerEvents.OnMovePlayerToBlock -= MovePlayerToBlockAndPickUpBlock;
        }
        
        /// <summary>
        /// Public method to initiate movement to a target position.
        /// </summary>
        /// <param name="targetPosition">The position to move to.</param>
        public void MovePlayerToClickedPositionAndPickupBlock(Vector3 targetPosition)
        {
            MoveToPosition(targetPosition, PickUpBlock);
        }
        
        /// <summary>
        /// Public method to initiate movement to a target position, typically called by external scripts.
        /// </summary>
        /// <param name="targetPosition">The position to move to.</param>
        public void MoveToClickedPosition(Vector3 targetPosition)
        {
            MoveToPosition(targetPosition);
        }

        /// <summary>
        /// Initiates movement to the specified block position.
        /// </summary>
        /// <param name="blockPosition">The position of the block to move to.</param>
        private void MovePlayerToBlockAndPickUpBlock(Vector3 blockPosition)
        {
            MoveToPosition(blockPosition, PickUpBlock);
        }

        /// <summary>
        /// Moves the player to a specified target position.
        /// </summary>
        /// <param name="targetPosition">The position to move to.</param>
        /// <param name="onReachedTarget">Action to perform once the target is reached.</param>
        public void MoveToPosition(Vector3 targetPosition, Action onReachedTarget = null)
        {
            StartCoroutine(MoveToPositionCoroutine(targetPosition, onReachedTarget));
        }

        private IEnumerator MoveToPositionCoroutine(Vector3 targetPosition, Action onReachedTarget = null)
        {
            while (Vector3.Distance(spawnedPlayer.transform.position, targetPosition) > 0.1f)
            {
                spawnedPlayer.transform.position = Vector3.MoveTowards(spawnedPlayer.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            onReachedTarget?.Invoke();
        }

        /// <summary>
        /// Picks up the block after reaching its position.
        /// </summary>
        private void PickUpBlock()
        {
            GameObject block = GameObject.Find("WordBlock");
            if (block != null)
            {
                block.transform.SetParent(spawnedPlayer.transform);
                
                // Adjust position relative to the player if needed
                block.transform.localPosition = Vector3.zero;  

                // Move to the drop-off point after picking up the block
                MoveToDropOffPoint();
            }
            else
            {
                Debug.LogError("Block not found for pick-up.");
            }
        }

        /// <summary>
        /// Moves the player to the designated drop-off point.
        /// </summary>
        private void MoveToDropOffPoint()
        {
            MoveToPosition(DropOffPoint, DropOffBlock);
        }

        /// <summary>
        /// Drops off the block at the drop-off point.
        /// </summary>
        private void DropOffBlock()
        {
            GameObject block = GameObject.Find("WordBlock");
            if (block != null)
            {
                block.transform.SetParent(null);
                block.transform.position = DropOffPoint;
            }
            else
            {
                Debug.LogError("Block not found for drop-off.");
            }
        }
    }
}

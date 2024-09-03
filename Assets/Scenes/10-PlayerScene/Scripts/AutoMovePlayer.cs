using System;
using System.Collections;
using Scenes._05_Minigames._56_WordFactory.Scripts.Managers;
using Spine.Unity;
using UnityEngine;

namespace Scenes._10_PlayerScene.Scripts
{
    /// <summary>
    /// Handles automatic movement of the player character to a specified position or the closest target.
    /// </summary>
    public class AutoMovePlayer : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5.0f;
        public GameObject DropOffPoint;
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
        public void MovePlayerToClickedPositionAndPickupBlock(GameObject block)
        {
            MoveToPosition(block, PickUpBlock);
        }
        
        /// <summary>
        /// Public method to initiate movement to a target position, typically called by external scripts.
        /// </summary>
        /// <param name="targetPosition">The position to move to.</param>
        public void MoveToClickedPosition(GameObject block)
        {
            MoveToPosition(block);
        }

        /// <summary>
        /// Initiates movement to the specified block position.
        /// </summary>
        /// <param name="blockPosition">The position of the block to move to.</param>
        private void MovePlayerToBlockAndPickUpBlock(GameObject block)
        {
            MoveToPosition(block, PickUpBlock);
        }

        /// <summary>
        /// Moves the player to a specified target position.
        /// </summary>
        /// <param name="targetPosition">The position to move to.</param>
        /// <param name="onReachedTarget">Action to perform once the target is reached.</param>
        public void MoveToPosition(GameObject block, Action onReachedTarget = null)
        {
            StartCoroutine(MoveToPositionCoroutine(block, onReachedTarget));
        }

        public IEnumerator MoveToPositionCoroutine(GameObject block, Action onReachedTarget = null)
        {
            Debug.Log("moving");
            spawnedPlayer.GetComponent<SpinePlayerMovement>().SetCharacterState("Walk");
            while (Vector3.Distance(spawnedPlayer.transform.position, block.transform.position) > 1.5f)
            {
                spawnedPlayer.transform.position = Vector3.MoveTowards(spawnedPlayer.transform.position, block.transform.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            spawnedPlayer.GetComponent<SpinePlayerMovement>().SetCharacterState("Idle");
            onReachedTarget?.Invoke();
        }

        private IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        /// <summary>
        /// Picks up the block after reaching its position.
        /// </summary>
        private void PickUpBlock()
        {
            spawnedPlayer.GetComponent<SpinePlayerMovement>().SetCharacterState("Throw");
            
            Debug.Log("Picked up block");
            GameObject block = GameObject.Find("WordBlock");
            if (block != null)
            {
                block.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                block.transform.SetParent(spawnedPlayer.transform);
                
                // Adjust position relative to the player if needed
                block.transform.localPosition = spawnedPlayer.transform.GetChild(1).position;
                
                // Wait for animation before moving
                StartCoroutine(Wait(1));

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
            Debug.Log("dropped off block");
            GameObject block = GameObject.Find("WordBlock");
            if (block != null)
            {
                block.transform.SetParent(null);
                block.transform.position = DropOffPoint.transform.position;
                
                Destroy(block);
            }
            else
            {
                Debug.LogError("Block not found for drop-off.");
            }
            
            MoveToPosition(WordFactoryGameManager.Instance.PlayerSpawnPoint);
        }
    }
}

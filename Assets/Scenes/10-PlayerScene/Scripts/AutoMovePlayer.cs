using System;
using System.Collections;
using Scenes._50_Minigames._56_WordFactory.Scripts.Managers;
using Spine.Unity;
using Spine;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

namespace Scenes._10_PlayerScene.Scripts
{
    /// <summary>
    /// Handles automatic movement of the player character to a specified position or the closest target.
    /// </summary>
    public class AutoMovePlayer : MonoBehaviour
    {
        public GameObject DropOffPoint;
        
        [SerializeField] private float moveSpeed = 5.0f;
        
        private GameObject spawnedPlayer;
        private Bone bone;
        private Vector3 offset = new Vector3(0, 1, 0);

        private bool boxOn = false;

        private SkeletonAnimation skeletonAnimation;

        private GameObject savedBlock;

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
            StopAllCoroutines();
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

        private void Update()
        {
            if (boxOn)
            {
                Vector3 boneWorldPos = skeletonAnimation.transform.TransformPoint(new Vector3(bone.WorldX, bone.WorldY, 0));

                savedBlock.transform.position = boneWorldPos + offset;
            }
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
                savedBlock = block;

                skeletonAnimation = spawnedPlayer.GetComponent<SpinePlayerMovement>().skeletonAnimation;

                bone = skeletonAnimation.skeleton.FindBone("Head");

                boxOn = true;


                // Wait for animation to complete before moving to the drop-off point
                StartCoroutine(
                    WaitForAnimation(
                        "Throw",
                        MoveToDropOffPoint));
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
                boxOn = false;
                // Re-enable physics interactions
                Rigidbody blockRigidbody = block.GetComponent<Rigidbody>();
                if (blockRigidbody != null)
                {
                    blockRigidbody.useGravity = true;  
                    blockRigidbody.isKinematic = false;
                }
                
                Destroy(block);
            }
            else
            {
                Debug.LogError("Block not found for drop-off.");
            }
            
            MoveToPosition(WordFactoryGameManager.Instance.PlayerSpawnPoint);
        }
        
        /// <summary>
        /// Waits for the specified animation to finish playing, then invokes a callback action.
        /// </summary>
        /// <param name="animationName">The name of the animation to wait for.</param>
        /// <param name="onComplete">The callback action to invoke after the animation completes.</param>
        private IEnumerator WaitForAnimation(string animationName, Action onComplete)
        {
            var state = spawnedPlayer.GetComponent<SpinePlayerMovement>().skeletonAnimation.state;
            // if (spawnedPlayer == null)
            // {
            //     Debug.LogError("Spawned player is null.");
            //     yield break;
            // }
            //
            // var skeletonAnimation = spawnedPlayer.GetComponent<SpinePlayerMovement>().skeletonAnimation;
            // if (skeletonAnimation == null)
            // {
            //     Debug.LogError("SkeletonAnimation component is missing on the spawned player.");
            //     yield break;
            // }
            //
            // var state = skeletonAnimation.state;
            // if (state == null)
            // {
            //     Debug.LogError("SkeletonAnimation state is null.");
            //     yield break;
            // }

            // Wait until the animation completes
            while (state.GetCurrent(0) != null && state.GetCurrent(0).Animation.Name == animationName && !state.GetCurrent(0).IsComplete)
            {
                yield return null;
            }

            // Invoke the callback action
            onComplete?.Invoke();
        }

    }
}

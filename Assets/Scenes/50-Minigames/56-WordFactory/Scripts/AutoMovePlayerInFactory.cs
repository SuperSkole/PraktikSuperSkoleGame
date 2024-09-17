using System;
using System.Collections;
using Scenes._10_PlayerScene.Scripts;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts
{
    /// <summary>
    /// Handles automatic movement of the player character to a specified position or the closest target.
    /// </summary>
    public class AutoMovePlayerInFactory : MonoBehaviour
    {
        public GameObject DropOffPoint;
        public GameObject PlayerSpawnPoint;
        
        [SerializeField] private float moveSpeed = 15.0f;
        private Coroutine currentMoveCoroutine;
        
        private GameObject spawnedPlayer;
        private GameObject chosenBlock;
        private GameObject savedBlock;

        private bool boxOn;

        private SkeletonAnimation skeletonAnimation;
        private Bone bone;
        private Vector3 offset = new Vector3(0, 1, 0);
        private PlayerAnimatior playerAnimator;
        private Rigidbody savedBlockRigidbody;
        private float minDistanceSqr = 1.5f * 1.5f;
        private Vector3 targetPosition;
        private Vector3 scale;


        public static event Action OnBlockDroppedOff;

        private void Awake()
        {
            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
        }
        
        private void Start()
        {
            playerAnimator = spawnedPlayer.GetComponent<PlayerAnimatior>();
            skeletonAnimation = playerAnimator.skeletonAnimation;
            bone = skeletonAnimation.skeleton.FindBone("Head");
        }


        private void OnEnable()
        {
            PlayerEvents.OnMovePlayerToBlock += MovePlayerToBlockAndPickUpBlock;
            PlayerEvents.OnMovePlayerToPosition += OnMovePlayerToPositionHandler;
        }

        private void OnDisable()
        {
            PlayerEvents.OnMovePlayerToBlock -= MovePlayerToBlockAndPickUpBlock;
            PlayerEvents.OnMovePlayerToPosition -= OnMovePlayerToPositionHandler;
        }
        
        private void OnDestroy()
        {
            StopAllCoroutines();
            PlayerEvents.OnMovePlayerToBlock -= MovePlayerToBlockAndPickUpBlock;
            PlayerEvents.OnMovePlayerToPosition -= OnMovePlayerToPositionHandler;
        }
        
        // Correctly defined event handler
        private void OnMovePlayerToPositionHandler(GameObject targetPosition)
        {
            //Debug.Log("moving back");
            MoveToPosition(targetPosition);
        }

        
        /// <summary>
        /// Public method to initiate movement to a target position.
        /// </summary>
        public void MovePlayerToClickedPositionAndPickupBlock(GameObject block)
        {
            MoveToPosition(block, PickUpBlock);
        }
        
        /// <summary>
        /// Public method to initiate movement to a target position, typically called by external scripts.
        /// </summary>
        public void MoveToClickedPosition(GameObject block)
        {
            MoveToPosition(block);
        }

        /// <summary>
        /// Initiates movement to the specified block position.
        /// </summary>
        /// <param name="block">The position of the block to move to.</param>
        private void MovePlayerToBlockAndPickUpBlock(GameObject block)
        {
            //Debug.Log(block);
            chosenBlock = block;
            MoveToPosition(block, PickUpBlock);
        }

        /// <summary>
        /// Moves the player to a specified target position.
        /// </summary>
        /// <param name="target">The position to move to.</param>
        /// <param name="onReachedTarget">Action to perform once the target is reached.</param>
        private void MoveToPosition(GameObject target, Action onReachedTarget = null)
        {
            if (currentMoveCoroutine != null)
            {
                StopCoroutine(currentMoveCoroutine);
            }
            
            currentMoveCoroutine = StartCoroutine(MoveToPositionCoroutine(target, onReachedTarget));
        }

        public IEnumerator MoveToPositionCoroutine(GameObject target, Action onReachedTarget = null)
        {
            // Lock Z position
            float fixedZ = 0f; 

            // Set character to "Walk" state once before the loop
            playerAnimator.SetCharacterState("Walk");

            // Continue updating target position in real-time to handle moving blocks
            while ((spawnedPlayer.transform.position - target.transform.position).sqrMagnitude > minDistanceSqr)
            {
                // Ensure the target still exists
                if (!target)
                {
                    yield break; 
                }

                targetPosition.Set(target.transform.position.x, spawnedPlayer.transform.position.y, fixedZ);
                float scaleX = spawnedPlayer.transform.position.x > targetPosition.x ? Mathf.Abs(spawnedPlayer.transform.localScale.x) : -Mathf.Abs(spawnedPlayer.transform.localScale.x);
                scale.Set(scaleX, spawnedPlayer.transform.localScale.y, spawnedPlayer.transform.localScale.z);
                spawnedPlayer.transform.localScale = scale;

                // Move towards the block
                spawnedPlayer.transform.position = Vector3.MoveTowards(spawnedPlayer.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Set character back to "Idle" state after movement is complete
            playerAnimator.SetCharacterState("Idle");
            onReachedTarget?.Invoke();
        }

        private void LateUpdate()
        {
            if (!boxOn || !savedBlock)
            {
                return;
            }

            // Double-check in case the block was destroyed since the last frame
            if (!savedBlock)  
            {
                boxOn = false;
                return; 
            }

            Vector3 boneWorldPos = skeletonAnimation.transform.TransformPoint(new Vector3(bone.WorldX, bone.WorldY, 0));
            savedBlock.transform.position = boneWorldPos + offset;
        }

        /// <summary>
        /// Picks up the block after reaching its position.
        /// </summary>
        private void PickUpBlock()
        {
            playerAnimator.SetCharacterState("Throw");
            
            //Debug.Log("Picked up block");
            
            GameObject block = chosenBlock;
            if (block)
            {
                savedBlock = block;
                savedBlockRigidbody = savedBlock.GetComponent<Rigidbody>();
                
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
            // EO; if there is no block to drop off.
            //Debug.Log("Attempting to drop off the block...");
            if (!chosenBlock)
            {
                Debug.LogError("Block not found for drop-off. It may have been destroyed or not set correctly.");
                OnBlockDroppedOff?.Invoke();
                return;  
            }

            // Assuming we have a valid block if we reach here.
            boxOn = false;
            if (savedBlockRigidbody)
            {
                savedBlockRigidbody.useGravity = true;  
                savedBlockRigidbody.isKinematic = false;
            }
            else
            {
                Debug.LogError("Rigidbody not found on the block.");
            }

            Destroy(chosenBlock);
            
            // Clear the reference to ensure it's not used post-destruction.
            chosenBlock = null;  
            savedBlock = null;  

            OnBlockDroppedOff?.Invoke();
        }

        
        /// <summary>
        /// Waits for the specified animation to finish playing, then invokes a callback action.
        /// </summary>
        /// <param name="animationName">The name of the animation to wait for.</param>
        /// <param name="onComplete">The callback action to invoke after the animation completes.</param>
        private IEnumerator WaitForAnimation(string animationName, Action onComplete)
        {
            var state = playerAnimator.skeletonAnimation.state;

            // Wait until the animation completes
            while (state.GetCurrent(0) != null &&
                   state.GetCurrent(0).Animation.Name == animationName &&
                   !state.GetCurrent(0).IsComplete)
            {
                yield return null;
            }

            // Invoke the callback action
            onComplete?.Invoke();
        }
    }
}
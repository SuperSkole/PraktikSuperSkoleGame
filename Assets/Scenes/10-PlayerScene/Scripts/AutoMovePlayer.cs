using System;
using System.Collections;
using Scenes._50_Minigames._65_MonsterTower.Scripts;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Scenes._10_PlayerScene.Scripts
{
    /// <summary>
    /// Handles automatic movement of the player character to a specified position or the closest target.
    /// </summary>
    public class AutoMovePlayer : MonoBehaviour
    {
        public GameObject DropOffPoint;
        public GameObject PlayerSpawnPoint;
        public MonsterTowerManager monsterTowerManager;
        
        [SerializeField] private float moveSpeed = 15.0f;
        private Coroutine currentMoveCoroutine;
        
        private GameObject spawnedPlayer;
        private GameObject chosenBlock;
        private GameObject savedBlock;

        private bool boxOn = false;

        private SkeletonAnimation skeletonAnimation;
        private Bone bone;
        private Vector3 offset = new Vector3(0, 1, 0);
        private PlayerAnimatior playerAnimatior;


        public static event Action OnBlockDroppedOff;

        private void Awake()
        {
            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
        }
        
        private void Start()
        {
            playerAnimatior = spawnedPlayer.GetComponent<PlayerAnimatior>();
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
            Debug.Log("moving back");
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
           
            chosenBlock = block;
            MoveToPosition(block, PickUpBlock);
        }

        /// <summary>
        /// Moves the player to a specified target position.
        /// </summary>
        /// <param name="target">The position to move to.</param>
        /// <param name="onReachedTarget">Action to perform once the target is reached.</param>
        public void MoveToPosition(GameObject target, Action onReachedTarget = null)
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

            // Continue updating target position in real-time to handle moving blocks
            while (Vector3.Distance(transform.position, target.transform.position) > 1.5f)
            {
                // Ensure the target still exists
                if (!target)
                {
                    yield break; 
                }
                
                Vector3 targetPosition = new Vector3(target.transform.position.x, spawnedPlayer.transform.position.y, fixedZ);
                spawnedPlayer.transform.localScale = spawnedPlayer.transform.position.x > targetPosition.x 
                    ? new Vector3(Mathf.Abs(spawnedPlayer.transform.localScale.x), spawnedPlayer.transform.localScale.y, spawnedPlayer.transform.localScale.z)
                    : new Vector3(-Mathf.Abs(spawnedPlayer.transform.localScale.x), spawnedPlayer.transform.localScale.y, spawnedPlayer.transform.localScale.z);
        
                Debug.Log("moving");
                playerAnimatior.SetCharacterState("Walk");

                // Check if player has reached the block within a threshold distance
                if (Vector3.Distance(spawnedPlayer.transform.position, targetPosition) <= 1.5f)
                {
                    // Exit loop when close enough to the block
                    break; 
                }

                // Move towards the block
                spawnedPlayer.transform.position = Vector3.MoveTowards(spawnedPlayer.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            playerAnimatior.SetCharacterState("Idle");
            onReachedTarget?.Invoke();
        }


        private void Update()
        {
            // Check if savedBlock is not null before accessing
            if (!boxOn || !savedBlock)
            {
                return;
            }

            // Double-check in case the block was destroyed since the last frame
            if (!savedBlock)  
            {
                Debug.Log("Attempted to access a destroyed block.");
                boxOn = false;
                    
                // Skip the rest of the update for this frame
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
            playerAnimatior.SetCharacterState("Throw");
            
            Debug.Log("Picked up block");
            //GameObject block = GameObject.Find("WordBlock");
            GameObject block = chosenBlock;
            if (block)
            {
                savedBlock = block;

                skeletonAnimation = playerAnimatior.skeletonAnimation;
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
            // EO; if there is no block to drop off.
            Debug.Log("Attempting to drop off the block...");
            if (!chosenBlock)
            {
                Debug.LogError("Block not found for drop-off. It may have been destroyed or not set correctly.");
                OnBlockDroppedOff?.Invoke();
                return;  
            }

            // Assuming we have a valid block if we reach here.
            boxOn = false;
            Rigidbody blockRigidbody = chosenBlock.GetComponent<Rigidbody>();
            if (blockRigidbody)
            {
                blockRigidbody.useGravity = true;  
                blockRigidbody.isKinematic = false;
            }
            else
            {
                Debug.LogError("Rigidbody not found on the block.");
            }

            if(monsterTowerManager)
            {
                monsterTowerManager.catapultAming.SetAmmo(chosenBlock);
            }
            else
            {
                Debug.LogError("MonsterTowerManager not set.");
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
            var state = playerAnimatior.skeletonAnimation.state;

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
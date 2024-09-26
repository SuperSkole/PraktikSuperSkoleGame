using Spine.Unity;
using System.Collections;
using UnityEngine;

namespace Scenes._10_PlayerScene.Scripts
{

    public class PlayerMovement_POD : MonoBehaviour
    {


        //Blend paramters
        public float walkThreshold = 0.1f;
        public float moveSpeed = 5.0f;
        public bool facingRight = true;

        public Camera sceneCamera;
        public LayerMask placementLayermask;
        private Vector3 targetPosition;
        private bool isMoving;

        public bool hoveringOverUI = false;
        [SerializeField] private PlayerAnimatior animatior;
        [SerializeField] private GameObject interactionGO;
        [SerializeField] ParticleSystem pointAndClickEffect;
        [SerializeField] private Rigidbody rb;

        /// <summary>
        /// Handles player input for point-and-click movement.
        /// </summary>
        void Update()
        {
            if (!hoveringOverUI && Input.GetMouseButtonDown(0))
            {
                Vector3 newMoveToPos = GetSelectedMapPosition();
                if (newMoveToPos != Vector3.zero)
                {
                    StartMovement(newMoveToPos);
                }
            }

        }

        /// <summary>
        /// im using FixedUpdate becorse this is all physics
        /// </summary>
        private void FixedUpdate()
        {
            if (!isMoving || Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
            {
                if (isMoving)
                {
                    StopPointAndClickMovement();
                }
                PlayerWASDMovement();
            }
            if (isMoving && Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                MoveToTarget();
            }
        }


        /// <summary>
        /// Handles movement of the player using WASD keys.
        /// </summary>
        private void PlayerWASDMovement()
        {
            //StopCoroutine(MoveToTarget());
            //Remove Raw to add inertia
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized * moveSpeed;
            movement = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * movement;
            // Move the player
            GetComponent<Rigidbody>().velocity = new(movement.x, GetComponent<Rigidbody>().velocity.y, movement.z);

            //transform.Translate(movement * moveSpeed * Time.deltaTime);

            // Flip the player based on the horizontal input
            if (horizontalInput < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                interactionGO.transform.localScale = new Vector3(Mathf.Abs(-interactionGO.transform.localScale.x), interactionGO.transform.localScale.y, interactionGO.transform.localScale.z);
                interactionGO.transform.localPosition = new Vector3(3.75f, 2.5f, -2.5f);
            }
            else if (horizontalInput > 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                interactionGO.transform.localScale = new Vector3(-Mathf.Abs(-interactionGO.transform.localScale.x), interactionGO.transform.localScale.y, interactionGO.transform.localScale.z);
                interactionGO.transform.localPosition = new Vector3(-3.75f, 2.5f, -2.5f);
            }

            if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
            {
                animatior.SetCharacterState("Walk");
            }
            else
            {
                animatior.SetCharacterState("Idle");
            }
        }
        /// <summary>
        /// Starts the movement of the player towards a target position clicked on the map.
        /// </summary>
        /// <param name="newMoveToPos">The target position to move towards.</param>
        private void StartMovement(Vector3 newMoveToPos)
        {
            newMoveToPos += new Vector3(0, 2, 0);
            targetPosition = newMoveToPos;
            isMoving = true;
            // Stop any ongoing point-and-click movement
            if (isMoving)
            {
                animatior.SetCharacterState("Idle");
            }
            if (transform.position.x > targetPosition.x)
            {
                // Moving right
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                interactionGO.transform.localScale = new Vector3(Mathf.Abs(-interactionGO.transform.localScale.x), interactionGO.transform.localScale.y, interactionGO.transform.localScale.z);
                interactionGO.transform.localPosition = new Vector3(3.75f, 2.5f, -2.5f);

            }
            else
            {
                // Moving left
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                interactionGO.transform.localScale = new Vector3(-Mathf.Abs(-interactionGO.transform.localScale.x), interactionGO.transform.localScale.y, interactionGO.transform.localScale.z);
                interactionGO.transform.localPosition = new Vector3(-3.75f, 2.5f, -2.5f);
            }
            animatior.SetCharacterState("Walk");
            var effect = Instantiate(pointAndClickEffect, new Vector3(targetPosition.x, targetPosition.y - 1.892f, targetPosition.z), pointAndClickEffect.transform.rotation);
            Destroy(effect.gameObject, 0.5f);
        }

        /// <summary>
        /// Coroutine that smoothly moves the player towards the target position.
        /// </summary>
        /// <returns></returns>
        private void MoveToTarget()
        {
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetPosition.x, targetPosition.z)) > 0.1f)
            {
                // Move towards the target position at the specified speed
                Vector3 moveVel = (targetPosition - transform.position).normalized * moveSpeed;
                GetComponent<Rigidbody>().velocity = new(moveVel.x, GetComponent<Rigidbody>().velocity.y, moveVel.z);
                return;
            }
            // Snap to the exact position when very close to avoid overshooting
            transform.position = targetPosition;
            isMoving = false;
        }

        /// <summary>
        /// Stops the point-and-click movement of the player.
        /// </summary>
        public void StopPointAndClickMovement()
        {
            isMoving = false;
        }
        /// <summary>
        /// Gets the position on the map that the player clicked on.
        /// </summary>
        /// <returns>The position on the map where the player clicked, or Vector3.zero if no valid position was found.</returns>
        public Vector3 GetSelectedMapPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = sceneCamera.nearClipPlane;
            Ray ray = sceneCamera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, placementLayermask))
            {
                if (hit.transform.gameObject.layer != 11)
                    return hit.point;
            }
            return Vector3.zero;
        }
    }
}

using UnityEngine;

namespace Scenes.Minigames.WordFactory.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class WordBlockController : MonoBehaviour
    {
        private Vector3 offset;
        private bool isDragging = false;
        private Camera mainCam;
        private Rigidbody rb;

        private void Awake()
        {
            mainCam = Camera.main;
            rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY; // Lock Z position and rotation
        }

        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(1)) // Right mouse button
            {
                Destroy(gameObject);
                return;
            }

            if (Input.GetMouseButton(0)) // Left mouse button
            {
                // Calculate the offset between the block and the mouse position
                offset = gameObject.transform.position - GetMouseWorldPos();
                isDragging = true;
                rb.isKinematic = true; // Disable physics while dragging
            }
        }

        private void OnMouseUp()
        {
            // Stop dragging the block
            isDragging = false;
            rb.isKinematic = false; // Enable physics again
            rb.velocity = Vector3.zero; // Ensure the block falls straight down
        }

        private void Update()
        {
            // If dragging, update the position of the block
            if (isDragging)
            {
                Vector3 newPos = GetMouseWorldPos() + offset;
                rb.MovePosition(new Vector3(newPos.x, newPos.y, transform.position.z));
            }

            // Ensure the block is always facing the -Z direction
            transform.rotation = Quaternion.Euler(0, 0, 0); // Adjust this as needed to face the correct direction

            // Constrain the block's position within screen bounds
            ConstrainWithinScreenBounds();

            // Check for right mouse button click to destroy block
            if (Input.GetMouseButtonDown(1)) // Right mouse button
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Perform the raycast
                if (Physics.Raycast(ray, out hit))
                {
                    // Check if the hit object has the tag "WordBlock"
                    if (hit.collider.gameObject == gameObject && hit.collider.CompareTag("WordBlock"))
                    {
                        // Destroy the block
                        Destroy(gameObject);
                    }
                }
            }
        }

        private Vector3 GetMouseWorldPos()
        {
            // Get the mouse position in world coordinates
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = mainCam.WorldToScreenPoint(gameObject.transform.position).z;
            return mainCam.ScreenToWorldPoint(mousePoint);
        }

        private void ConstrainWithinScreenBounds()
        {
            Vector3 pos = transform.position;
            Vector3 minScreenBounds = mainCam.ViewportToWorldPoint(new Vector3(0, 0, mainCam.nearClipPlane));
            Vector3 maxScreenBounds = mainCam.ViewportToWorldPoint(new Vector3(1, 1, mainCam.nearClipPlane));

            pos.x = Mathf.Clamp(pos.x, minScreenBounds.x, maxScreenBounds.x);
            pos.y = Mathf.Clamp(pos.y, minScreenBounds.y, maxScreenBounds.y);
            transform.position = pos;
        }
    }
}

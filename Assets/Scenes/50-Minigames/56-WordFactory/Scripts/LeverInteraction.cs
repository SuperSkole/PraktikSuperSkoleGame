using System;
using System.Collections;
using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts
{
    public class LeverInteraction : MonoBehaviour
    {
        public enum RotationDirection
        {
            Clockwise,
            CounterClockwise
        }

        public event Action OnLeverPulled; 

        [SerializeField] private Transform leverHandle; 
        [SerializeField] private Vector3 defaultRotation;
        [SerializeField] private Vector3 pulledRotationClockwise; 
        [SerializeField] private Vector3 pulledRotationCounterClockwise;
        [SerializeField] private float resetDelay = 0.5f;
        [SerializeField] private float pullSpeed = 1.0f; 
        public RotationDirection ChosenRotationDirection;

        private bool isNearLever = false;
        private bool isPulling = false;

        private void Start()
        {
            // Set the lever to its default position
            leverHandle.localEulerAngles = defaultRotation;
        }

        private void Update()
        {
            // Check if the player is near the lever and presses the "E" key
            if (isNearLever && Input.GetKeyDown(KeyCode.E))
            {
                PullLever();
            }

            // Handle touch input
            HandleTouchInput();
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform == leverHandle)
                        {
                            PullLever();
                        }
                    }
                }
            }
        }

        private void OnMouseDown()
        {
            // Handle mouse click
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == leverHandle)
                {
                    PullLever();
                }
            }
        }

        private void PullLever()
        {
            if (isPulling) return; // Prevent multiple pulls at once
            isPulling = true;

            // Determine the target rotation based on the lever's direction
            Vector3 targetRotation = (ChosenRotationDirection == RotationDirection.Clockwise) ? pulledRotationClockwise : pulledRotationCounterClockwise;

            // Rotate the lever to the pulled position
            StartCoroutine(RotateLever(targetRotation));

            // Trigger the lever pulled event
            OnLeverPulled?.Invoke();
            Debug.Log("Lever pulled!");

            // Reset the lever after a short delay
            StartCoroutine(ResetLever());
        }

        private IEnumerator RotateLever(Vector3 targetRotation)
        {
            while (Vector3.Distance(leverHandle.localEulerAngles, targetRotation) > 0.01f)
            {
                leverHandle.localEulerAngles = Vector3.Lerp(leverHandle.localEulerAngles, targetRotation, pullSpeed * Time.deltaTime);
                yield return null;
            }

            leverHandle.localEulerAngles = targetRotation;
        }

        private IEnumerator ResetLever()
        {
            // Wait for the specified delay
            yield return new WaitForSeconds(resetDelay);

            // Rotate the lever back to the default position
            StartCoroutine(RotateLever(defaultRotation));

            isPulling = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if the player is near the lever
            if (other.CompareTag("Player"))
            {
                isNearLever = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Check if the player leaves the lever area
            if (other.CompareTag("Player"))
            {
                isNearLever = false;
            }
        }
    }
}

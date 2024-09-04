using System;
using System.Collections;
using Scenes._50_Minigames._56_WordFactory.Scripts.Managers;
using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts
{
    public class GearRotationController : MonoBehaviour
    {
        public static event Action<int, bool> OnGearRotate;
        public static event Action OnRotationComplete; 
        
        [SerializeField] private float rotationDuration = 0.5f; 
        
        public int gearIndex;
        public bool isRotating = false; // Flag to check if rotation is ongoing
        
        // Method to rotate the gear clockwise by one tooth
        public void RotateClockwise()
        {
            if (!isRotating)
            {
                WordFactorySoundManager.Instance.PlaySound(WordFactorySoundManager.SoundEvent.RotateGear);
                StartCoroutine(RotateGear(gameObject, true));
            }
        }

        // Method to rotate the gear counterclockwise by one tooth
        public void RotateCounterClockwise()
        {
            if (!isRotating)
            {
                WordFactorySoundManager.Instance.PlaySound(WordFactorySoundManager.SoundEvent.RotateGear);
                StartCoroutine(RotateGear(gameObject, false));
            }
        }
        
        /// Coroutine to animate the rotation of a gear by one tooth
        private IEnumerator RotateGear(GameObject gear, bool clockwise)
        {
            var numberOfTeeth = WordFactoryGameManager.Instance.GetNumberOfGears() >= 2
                ? WordFactoryGameManager.Instance.GetNumberOfTeeth()
                : 9;
            
            GearRotationController controller = gear.GetComponent<GearRotationController>();
            if (controller.isRotating)
                yield break;  
    
            controller.isRotating = true;  
            OnGearRotate?.Invoke(gearIndex, clockwise);

            float anglePerTooth = 360f / numberOfTeeth;
            float angle = clockwise ? anglePerTooth : -anglePerTooth;
            Quaternion startRotation = gear.transform.localRotation;
            Quaternion endRotation = Quaternion.Euler(0, 0, angle) * startRotation;

            float elapsed = 0f;
            while (elapsed < rotationDuration)
            {
                gear.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, elapsed / rotationDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            gear.transform.localRotation = endRotation;
            controller.isRotating = false;
            NotifyRotationComplete();
            
        }

        // Notification method when rotation is complete
        public void NotifyRotationComplete()
        {
            OnRotationComplete?.Invoke();
        }
    }
}

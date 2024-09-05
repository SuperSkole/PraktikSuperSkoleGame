using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scenes._50_Minigames._56_WordFactory.Scripts
{
    public class ColorTooth : MonoBehaviour
    {
        public static event Action<Transform, Color, int, float> OnToothColorChange;
        public static event Action<GameObject[]> OnRequestBlinkAllTeethRandomly;
        public static event Action OnBlinkingCompleted;


        private void OnEnable()
        {
            OnToothColorChange += HandleColorChange;
            OnRequestBlinkAllTeethRandomly += HandleBlinkAllTeethRandomly;
        }

        private void OnDisable()
        {
            OnToothColorChange -= HandleColorChange;
            OnRequestBlinkAllTeethRandomly -= HandleBlinkAllTeethRandomly;
        }

        // Method to raise the color change event
        public static void RaiseColorChangeEvent(Transform tooth, Color color, int blinkCount = 0, float blinkInterval = 0.25f)
        {
            OnToothColorChange?.Invoke(tooth, color, blinkCount, blinkInterval);
        }

        // Handle the color change based on the event
        private void HandleColorChange(Transform tooth, Color color, int count, float interval)
        {
            if (count > 0)
            {
                StartCoroutine(BlinkColorRoutine(tooth, color, count, interval));
            }
            else
            {
                SetToothColor(tooth, color);
            }
        }

        // Coroutine for setting tooth color with a new material instance
        private void SetToothColor(Transform tooth, Color color)
        {
            StartCoroutine(ColorChangeRoutine(tooth, color, 0)); // Duration set to 0 for immediate change
        }

        private IEnumerator ColorChangeRoutine(Transform tooth, Color color, float duration)
        {
            Renderer toothRenderer = tooth.GetComponent<Renderer>();
            // Clone the material
            Material newMaterial = new Material(toothRenderer.material); 
            // Assign new material to the renderer
            toothRenderer.material = newMaterial; 
            newMaterial.color = color;

            if (duration > 0)
            {
                yield return new WaitForSeconds(duration);
                newMaterial.color = Color.white;
            }
        }

        // Coroutine for blinking tooth color
        private IEnumerator BlinkColorRoutine(Transform tooth, Color color, int count, float interval)
        {
            Renderer renderer = tooth.GetComponent<Renderer>();
            Material newMaterial = new Material(renderer.material);
            renderer.material = newMaterial;

            for (int i = 0; i < count; i++)
            {
                newMaterial.color = color;
                yield return new WaitForSeconds(interval);
                newMaterial.color = Color.white;
                yield return new WaitForSeconds(interval);
            }
        }

        // Method to blink all teeth in random colors once
        private void HandleBlinkAllTeethRandomly(GameObject[] gears) 
        {
            foreach (GameObject gear in gears) 
            {
                Transform teethContainer = gear.transform.Find("TeethContainer");
                if (teethContainer != null) 
                {
                    int teethCount = teethContainer.childCount;
                    if (teethCount == 0) return;

                    // Random starting point and direction
                    int startToothIndex = Random.Range(0, teethCount);
                    bool isClockwise = Random.value > 0.5f;
                    int direction = isClockwise ? 1 : -1;

                    StartCoroutine(BlinkTeethInOrder(teethContainer, startToothIndex, direction, teethCount));
                }
            }
        }

        private IEnumerator BlinkTeethInOrder(Transform teethContainer, int startToothIndex, int direction, int teethCount)
        {
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            // Total number of blinks, 3 rounds around the gear
            int totalLoops = teethCount * 3; 

            for (int i = 0; i < totalLoops; i++)
            {
                int toothIndex = (startToothIndex + i * direction) % teethCount;
                if (toothIndex < 0) toothIndex += teethCount; // Adjust for negative indices

                Transform tooth = teethContainer.GetChild(toothIndex);
                // Blink each tooth 
                RaiseColorChangeEvent(tooth, randomColor, 1, 0.1f); 
                // Wait for blink interval before moving to the next tooth
                yield return new WaitForSeconds(0.05f); 

                // Reset color after blink (optional, remove if you want to keep the last color)
                Renderer toothRenderer = tooth.GetComponent<Renderer>();
                if (toothRenderer != null)
                {
                    toothRenderer.material.color = Color.white;
                }
            }
            
            // After finishing the blinking loop:
            if (OnBlinkingCompleted != null) {
                OnBlinkingCompleted.Invoke();
            }
        }


        public static void RequestBlinkAllTeethRandomly(GameObject[] gears) 
        {
            OnRequestBlinkAllTeethRandomly?.Invoke(gears);
        }
    }
}

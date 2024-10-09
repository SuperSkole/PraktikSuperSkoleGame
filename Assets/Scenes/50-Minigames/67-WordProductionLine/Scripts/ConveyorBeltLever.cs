using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scenes._50_Minigames._67_WordProductionLine.Scripts
{

    public class ConveyorBeltLever : MonoBehaviour
    {


        [SerializeField] private GameObject stopBeltLever;

        [SerializeField]
        private CreateProductionLineBoxes createProductionLineBoxes;

        [SerializeField]
        private CreateImageBox createImageBox;

        [SerializeField]
        private int leverId;

        [SerializeField]
        private AudioClip leverSound;

        private ProductionLine[] productionLine;

        public float rotationAngle = -35f;
        private bool isRotated = false;


        private void Start()
        {
            productionLine = FindObjectsOfType<ProductionLine>();
        }

        /// <summary>
        /// If the mouse is over can interact with object.
        /// </summary>
        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0) && leverId == 1)
            {
                foreach (var item in productionLine)
                {
                    if (item.converyerBeltId == 1)
                    {
                        item.conveyerBeltOn = !item.conveyerBeltOn;
                    }

                }
                createImageBox.isOn = !createImageBox.isOn;


                RotateLever();
            }

            if (Input.GetMouseButtonDown(0) && leverId == 2)
            {
                foreach (var item in productionLine)
                {
                    if (item.converyerBeltId == 2)
                    {
                        item.conveyerBeltOn = !item.conveyerBeltOn;
                    }

                }
                createProductionLineBoxes.isOn = !createProductionLineBoxes.isOn;

                RotateLever();
            }
        }

        /// <summary>
        /// Rotates the lever on the z angle depending on whether its on or off.
        /// </summary>
        private void RotateLever()
        {
            if (stopBeltLever != null)
            {
                // Rotate the lever based on its current state
                if (isRotated)
                {
                    // Rotate back to the original position (reset)
                    stopBeltLever.transform.rotation = Quaternion.Euler(0, 0, 35);
                    AudioManager.Instance.PlaySound(leverSound, SoundType.SFX);
                }
                else
                {
                    // Rotate to the desired angle on the Z-axis
                    stopBeltLever.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
                    AudioManager.Instance.PlaySound(leverSound, SoundType.SFX);
                }

                // Toggle the rotation state
                isRotated = !isRotated;
            }
            else
            {
                Debug.LogError("Lever GameObject is not assigned!");
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltLever : MonoBehaviour
{


    [SerializeField] private GameObject stopBeltLever;

    [SerializeField]
    private CreateProductionLineBoxes createProductionLineBoxes;    

    private ProductionLine[] productionLine;

    public float rotationAngle = 45f;
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
        if (Input.GetMouseButtonDown(0))
        {
            foreach (var item in productionLine)
            {
                item.conveyerBeltOn = !item.conveyerBeltOn;
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
                stopBeltLever.transform.rotation = Quaternion.Euler(0, -90, 135);
            }
            else
            {
                // Rotate to the desired angle on the Z-axis
                stopBeltLever.transform.rotation = Quaternion.Euler(0, -90, rotationAngle);
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

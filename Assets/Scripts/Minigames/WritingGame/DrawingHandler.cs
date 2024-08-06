using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI components

public class DrawingHandler : MonoBehaviour
{
    public Camera m_camera;
    public GameObject brush;

    LineRenderer currentLineRenderer;
    Vector3 lastPos;
    public bool isPainting = true; // Initialize as true to enable drawing initially
    public float offsetDistance = 0.5f; // Distance offset from the canvas

    public DrawingEvaluator drawingEvaluator; // Reference to the evaluator script

    private List<GameObject> drawnBrushInstances = new List<GameObject>(); // List to hold drawn brush instances

    // Ink Meter Variables
    public Slider inkMeterSlider; // Reference to the UI Slider for the ink meter
    public float maxInkAmount = 100f; // Maximum amount of ink
    private float currentInkAmount;

    private void Start()
    {
        currentInkAmount = maxInkAmount;
        inkMeterSlider.maxValue = maxInkAmount;
        inkMeterSlider.value = currentInkAmount;
    }

    private void Update()
    {
        if (isPainting)
        {
            Drawing();
        }

        // Update the ink meter UI
        inkMeterSlider.value = currentInkAmount;
    }

    void Drawing()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TryCreateBrush();
        }
        else if (Input.GetKey(KeyCode.Mouse0) && currentLineRenderer != null)
        {
            PointToMousePos();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            EndDrawing();
        }
    }

    void TryCreateBrush()
    {
        RaycastHit hit;

        // Use raycast to find the point in the world where the mouse is pointing
        if (Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            Vector3 mousePos = hit.point;
            mousePos += hit.normal * offsetDistance; // Offset the position

            CreateBrush(mousePos, hit.normal);
        }
    }

    void CreateBrush(Vector3 position, Vector3 normal)
    {
        GameObject brushInstance = Instantiate(brush);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        // Set the position of the brush instance to the mouse position with offset
        brushInstance.transform.position = position;

        // Set the rotation of the brush instance to face the camera
        brushInstance.transform.rotation = Quaternion.LookRotation(normal);

        // Initialize the line renderer with the first point
        currentLineRenderer.positionCount = 2; // Ensure we have at least two points to start with
        currentLineRenderer.SetPosition(0, position);
        currentLineRenderer.SetPosition(1, position);

        lastPos = position; // Initialize lastPos with the current mouse position

        // Add the brush instance to the list
        drawnBrushInstances.Add(brushInstance);
    }

    void AddAPoint(Vector3 pointPos)
    {
        float distance = Vector3.Distance(lastPos, pointPos);
        currentInkAmount -= distance; // Decrease ink based on the distance drawn

        if (currentInkAmount <= 0)
        {
            // Out of ink, stop drawing
            EndDrawing();
            return;
        }

        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    void PointToMousePos()
    {
        if (currentLineRenderer == null) return; // Check if currentLineRenderer is null

        RaycastHit hit;
        if (Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            Vector3 mousePos = hit.point;
            mousePos += hit.normal * offsetDistance; // Offset the position

            if (lastPos != mousePos)
            {
                AddAPoint(mousePos);
                lastPos = mousePos; // Update lastPos with the current mouse position
            }
        }
    }

    void EndDrawing()
    {
        if (currentLineRenderer != null)
        {
            drawingEvaluator.EvaluateDrawing(currentLineRenderer); // Call the evaluator
            currentLineRenderer = null;
        }

        // Reset ink amount for the next segment
        currentInkAmount = maxInkAmount;
    }

    public void ClearDrawnSegments()
    {
        // Destroy all drawn brush instances
        foreach (var brushInstance in drawnBrushInstances)
        {
            Destroy(brushInstance);
        }
        drawnBrushInstances.Clear();
    }
}
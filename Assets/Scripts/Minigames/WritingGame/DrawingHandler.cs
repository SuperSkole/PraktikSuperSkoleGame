using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingHandler : MonoBehaviour
{
    public Camera m_camera;
    public GameObject brush;

    LineRenderer currentLineRenderer;
    Vector3 lastPos;
    public bool isPainting = true; 
    public float offsetDistance = 0.5f; 

    public DrawingEvaluator drawingEvaluator; 

    private List<GameObject> drawnBrushInstances = new List<GameObject>();


    public Slider inkMeterSlider;
    public float maxInkAmount = 100f;
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

        inkMeterSlider.value = currentInkAmount;
    }


    /// <summary>
    /// used for drawing
    /// </summary>
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

    /// <summary>
    /// gets the mouse pos in the world and sends it to CreateBrush.
    /// </summary>
    void TryCreateBrush()
    {
        RaycastHit hit;

        if (Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            Vector3 mousePos = hit.point;
            mousePos += hit.normal * offsetDistance;

            CreateBrush(mousePos, hit.normal);
        }
    }

    /// <summary>
    /// creates a new brush(linesecment) to draw with.
    /// </summary>
    /// <param name="position">the start position of were the line starts</param>
    /// <param name="normal">the normal of the raycast</param>
    void CreateBrush(Vector3 position, Vector3 normal)
    {
        GameObject brushInstance = Instantiate(brush);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        brushInstance.transform.position = position;
        brushInstance.transform.rotation = Quaternion.LookRotation(normal);
        currentLineRenderer.positionCount = 2;
        currentLineRenderer.SetPosition(0, position);
        currentLineRenderer.SetPosition(1, position);
        lastPos = position;
        drawnBrushInstances.Add(brushInstance);
    }


    /// <summary>
    /// adds a point to the current line rendere for a secment.
    /// </summary>
    /// <param name="pointPos">the pos of the new point for the line</param>
    void AddAPoint(Vector3 pointPos)
    {
        float distance = Vector3.Distance(lastPos, pointPos);
        currentInkAmount -= distance;

        if (currentInkAmount <= 0)
        { 
            EndDrawing();
            return;
        }

        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }


    /// <summary>
    /// adds a line to the mouse from the last mouse position.
    /// </summary>
    void PointToMousePos()
    {
        if (currentLineRenderer == null) return;

        RaycastHit hit;
        if (Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            Vector3 mousePos = hit.point;
            mousePos += hit.normal * offsetDistance;

            if (lastPos != mousePos)
            {
                AddAPoint(mousePos);
                lastPos = mousePos;
            }
        }
    }

    /// <summary>
    /// stops drawing, and check how good the player did.
    /// </summary>
    void EndDrawing()
    {
        if (currentLineRenderer != null)
        {
            drawingEvaluator.EvaluateDrawing(currentLineRenderer);
            currentLineRenderer = null;
        }
        currentInkAmount = maxInkAmount;
    }

    /// <summary>
    /// clears all drawing segments to clear the bord.
    /// </summary>
    public void ClearDrawnSegments()
    {
        foreach (var brushInstance in drawnBrushInstances)
        {
            Destroy(brushInstance);
        }
        drawnBrushInstances.Clear();
    }
}
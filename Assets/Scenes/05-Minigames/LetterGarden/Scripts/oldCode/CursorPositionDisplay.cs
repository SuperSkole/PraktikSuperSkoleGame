using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CursorPositionDisplay : MonoBehaviour
{
    public Camera mainCamera;
    public TextMeshProUGUI cursorPositionText;
    private bool isDisplaying = true;

    public DrawingEvaluator drawingEvaluator; // Reference to the DrawingEvaluator script

    private LineRenderer cursorLineRenderer;
    private string rating = "";

    private void Start()
    {
        // Initialize LineRenderer to visualize cursor position
        GameObject cursorLineObject = new GameObject("CursorLine");
        cursorLineRenderer = cursorLineObject.AddComponent<LineRenderer>();
        cursorLineRenderer.positionCount = 2;
        cursorLineRenderer.startWidth = 0.05f;
        cursorLineRenderer.endWidth = 0.05f;
        cursorLineRenderer.material = new Material(Shader.Find("Sprites/Default")) { color = Color.black };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isDisplaying = !isDisplaying;
            cursorPositionText.gameObject.SetActive(isDisplaying);
        }

        if (isDisplaying)
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 worldPosition = hit.point;
                cursorLineRenderer.SetPosition(0, mainCamera.transform.position);
                cursorLineRenderer.SetPosition(1, worldPosition);

                if (drawingEvaluator.IsLetterComplete())
                {
                    float overallDistance = drawingEvaluator.GetOverallDistanceAverage();
                    if (overallDistance < 0.2f)
                    {
                        rating = "Correct letter! Good job :)";
                    }
                    else
                    {
                        rating = "You're a little off. Try again!";
                    }
                    cursorPositionText.text = $"Cursor Position: {worldPosition.ToString("F2")}\nAll letters complete!\nAccuracy: {overallDistance:F2}\n{rating}";
                }
                else
                {
                    string currentLetterName = drawingEvaluator.lettersData.letters[drawingEvaluator.GetCurrentLetterIndex()].letterName;
                    cursorPositionText.text = $"Cursor Position: {worldPosition.ToString("F2")}\nCurrent Letter: {currentLetterName}\n";
                    ShowDistances(worldPosition);
                }
            }
            else
            {
                cursorPositionText.text = "Cursor Position: Not on surface";
            }
        }
    }

    void ShowDistances(Vector3 cursorPosition)
    {
        if (drawingEvaluator.GetCurrentSegmentIndex() >= drawingEvaluator.lettersData.letters.Count)
        {
            cursorPositionText.text += "No predefined segments available.";
            return;
        }

        int currentSegmentIndex = drawingEvaluator.GetCurrentSegmentIndex();
        var segment = drawingEvaluator.lettersData.letters[drawingEvaluator.GetCurrentLetterIndex()].segments[currentSegmentIndex];
        string distancesInfo = $"Segment {currentSegmentIndex + 1}:\n";

        foreach (var point in segment.points)
        {
            float distanceToPoint = Vector3.Distance(cursorPosition, point);
            distancesInfo += $"  Point {segment.points.IndexOf(point) + 1}: {distanceToPoint:F2}\n";
        }

        cursorPositionText.text += distancesInfo;
    }

    public void DisplayLetterCompletion(float accuracy, bool isCorrect)
    {
        if (isCorrect)
        {
            rating = "Correct letter! Good job :)";
        }
        else
        {
            rating = "Accuracy too low! Try again!";
        }

        Debug.Log($"DisplayLetterCompletion called. Accuracy: {accuracy}, isCorrect: {isCorrect}");
        cursorPositionText.text = $"Letter completed!\nAccuracy: {accuracy:F2}\n{rating}";
    }
}
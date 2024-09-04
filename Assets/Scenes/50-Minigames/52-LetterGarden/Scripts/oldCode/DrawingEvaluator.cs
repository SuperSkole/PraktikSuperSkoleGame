using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using Scenes.Minigames.LetterGarden.Scripts;
>>>>>>> origin/Project-Praktik-Main
using UnityEngine;

namespace Scenes._50_Minigames._52_LetterGarden.Scripts.oldCode
{
    public class DrawingEvaluator : MonoBehaviour
    {
        public BazierHandler bazierHandler;
        public Letters lettersData; 
        private int currentLetterIndex = 0; 
        private int currentSegmentIndex = 0; 

        public GameObject markerPrefab; 

        private List<GameObject> markers = new List<GameObject>(); 
        private float totalDistance = 0; 
        private bool letterComplete = false; 

        private DrawingHandler drawingHandler; 
        private CursorPositionDisplay cursorDisplay; 

        private bool markersVisible = true; 
        private int currentPointIndex = 0; 

        public float tolerance = 0.5f; 

        private Dictionary<(string, int), int> segmentMappings; 

        private void Start()
        {
            bazierHandler = GetComponent<BazierHandler>();
            drawingHandler = FindObjectOfType<DrawingHandler>();
            cursorDisplay = FindObjectOfType<CursorPositionDisplay>();

            // Ensure lettersData is initialized
            if (lettersData == null)
            {
                lettersData = FindObjectOfType<Letters>();
            }

            // Initialize the segment mappings
            segmentMappings = new Dictionary<(string, int), int>
            {
                { ("A", 0), 0 },
                { ("A", 1), 1 },
                { ("A", 2), 2 },
                { ("B", 0), 3 },
                { ("B", 1), 4 },
                { ("C", 0), 5 },
                { ("S", 0), 6 }
            };

            StartCoroutine(InitializeMarkersAndCheckSegment());
        }

        IEnumerator InitializeMarkersAndCheckSegment()
        {
            // Wait a frame to ensure initialization
            yield return null;

            // Create markers for the first segment of the first letter if available
            if (lettersData != null && lettersData.letters.Count > 0)
            {
                CreateMarkersForCurrentSegment();

                // Check the initial letter and segment
                CheckAndActivateSpecialSegment();
            }
            else
            {
                Debug.LogError("lettersData is not initialized or contains no letters.");
            }
        }

        private void Update()
        {
            // Toggle marker visibility with the G key
            if (Input.GetKeyDown(KeyCode.G))
            {
                markersVisible = !markersVisible;
                UpdateMarkersForCurrentSegment();
            }
        }

        public void EvaluateDrawing(LineRenderer lineRenderer)
        {
            if (lineRenderer == null || lineRenderer.positionCount < 3)
            {
                Debug.Log("Not enough points to evaluate.");
                return;
            }

            List<Vector3> drawnPoints = new List<Vector3>();
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                drawnPoints.Add(lineRenderer.GetPosition(i));
            }

            bool orderIsValid = ValidateOrder(drawnPoints, lettersData.letters[currentLetterIndex].segments[currentSegmentIndex]);
            float segmentDistance = CalculateTotalDistance(drawnPoints, lettersData.letters[currentLetterIndex].segments[currentSegmentIndex]);

            segmentDistance -= 0.5f;

            if (!orderIsValid)
            {
                Debug.Log($"Segment {currentSegmentIndex + 1} of letter {lettersData.letters[currentLetterIndex].letterName} is in the wrong direction. Try again.");
                ResetSegment();
                return;
            }

            if (segmentDistance > 0.2f)
            {
                Debug.Log($"Segment {currentSegmentIndex + 1} of letter {lettersData.letters[currentLetterIndex].letterName} is too far off. Accuracy: {segmentDistance:F2}. Try again.");
                ResetSegment();
                return;
            }

            totalDistance += segmentDistance;

            // Switch to the next segment
            currentSegmentIndex++;
            if (currentSegmentIndex >= lettersData.letters[currentLetterIndex].segments.Count)
            {
                float averageDistance = totalDistance / lettersData.letters[currentLetterIndex].segments.Count;
                totalDistance = 0; // Reset total distance for the next letter

                bool isCorrect = averageDistance <= 0.2f;

                cursorDisplay.DisplayLetterCompletion(averageDistance, isCorrect);

                if (!isCorrect)
                {
                    // Retry the current letter
                    currentSegmentIndex = 0;
                    totalDistance = 0;
                    drawingHandler.ClearDrawnSegments();
                    CreateMarkersForCurrentSegment();
                
                    // Ensure the segment is correctly updated
                    CheckAndActivateSpecialSegment();
                }
                else
                {
                    // Move to the next letter
                    currentSegmentIndex = 0; // Reset to the first segment

                    currentLetterIndex++; // Move to the next letter

                    if (currentLetterIndex >= lettersData.letters.Count)
                    {
                        currentLetterIndex = 0; // Loop back to the first letter or handle completion
                        // Disable drawing as all letters are completed
                        drawingHandler.isPainting = false;
                        letterComplete = true;
                        Debug.Log("All letters completed. Drawing is now disabled.");
                        ClearMarkers(); // Clear the markers
                    }
                    else
                    {
                        // Clear drawn segments for the new letter
                        drawingHandler.ClearDrawnSegments();
                        // Update markers for the first segment of the next letter
                        UpdateMarkersForCurrentSegment();
                    }

                    // Ensure the segment is correctly updated
                    CheckAndActivateSpecialSegment();
                }
            }
            else
            {
                // Update markers for the next segment
                UpdateMarkersForCurrentSegment();

                // Ensure the segment is correctly updated
                CheckAndActivateSpecialSegment();
            }
        }

        void CheckAndActivateSpecialSegment()
        {
            // Get the current letter name and segment index
            string currentLetterName = lettersData.letters[currentLetterIndex].letterName;
            int currentSegmentIndex = this.currentSegmentIndex;

            // Check if the mapping exists and activate the corresponding segment
            if (segmentMappings.TryGetValue((currentLetterName, currentSegmentIndex), out int segmentIndex))
            {
                bazierHandler.SelectSegment(segmentIndex);
            }
        }

        void ResetSegment()
        {
            // Retry the current segment
            currentSegmentIndex = 0;
            totalDistance = 0;
            drawingHandler.ClearDrawnSegments();
            CreateMarkersForCurrentSegment();

            // Check and activate the special segment after reset
            CheckAndActivateSpecialSegment();
        }

        bool ValidateOrder(List<Vector3> drawnPoints, LineSegment segment)
        {
            int pointIndex = 0;

            foreach (var point in drawnPoints)
            {
                if (pointIndex >= segment.points.Count)
                {
                    return true;
                }

                if (Vector3.Distance(point, segment.points[pointIndex]) < tolerance) // Use tolerance variable
                {
                    pointIndex++;
                }
            }

            return pointIndex >= segment.points.Count;
        }

        float CalculateTotalDistance(List<Vector3> drawnPoints, LineSegment segment)
        {
            float totalDistance = 0;

            foreach (var point in segment.points)
            {
                totalDistance += CalculateClosestDistance(drawnPoints, point);
            }

            return totalDistance / segment.points.Count; // Average distance to the segment points
        }

        float CalculateClosestDistance(List<Vector3> points, Vector3 targetPoint)
        {
            float closestDistance = float.MaxValue;
            foreach (var point in points)
            {
                float distance = Vector3.Distance(point, targetPoint);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                }
            }
            return closestDistance;
        }

        void CreateMarkersForCurrentSegment()
        {
            ClearMarkers();
            if (lettersData != null && lettersData.letters.Count > 0 && markersVisible)
            {
                LineSegment currentSegment = lettersData.letters[currentLetterIndex].segments[currentSegmentIndex];
                foreach (var point in currentSegment.points)
                {
                    //Debug.Log($"Creating marker at {point}");
                    markers.Add(CreateMarker(point));
                }
            }
        }

        void UpdateMarkersForCurrentSegment()
        {
            ClearMarkers();
            CreateMarkersForCurrentSegment();
        }

        void ClearMarkers()
        {
            foreach (var marker in markers)
            {
                Destroy(marker);
            }
            markers.Clear();
        }

        GameObject CreateMarker(Vector3 position)
        {
            return Instantiate(markerPrefab, position, Quaternion.identity);
        }

        public int GetCurrentSegmentIndex()
        {
            return currentSegmentIndex;
        }

        public int GetCurrentLetterIndex()
        {
            return currentLetterIndex;
        }

        public bool IsLetterComplete()
        {
            return letterComplete;
        }

        public float GetOverallDistanceAverage()
        {
            return totalDistance / (lettersData.letters.Count * lettersData.letters[0].segments.Count);
        }
    }
}

using UnityEngine;

public class BazierHandler : MonoBehaviour
{
    public Transform objectToMove; // The object to move along the curve
    public float speed = 5f; // Speed of the object

    private Vector3[][] allSegments; // Array to hold all segments
    private Vector3[] points;
    private float t;
    private int segmentCount;
    private int currentSegmentIndex = -1; // No segment selected initially
    private float[] segmentLengths;
    private float totalLength;

    void Start()
    {
        // Initialize all segments
        allSegments = new Vector3[][]
        {
            // A segment1
            new Vector3[]
            {
                new Vector3(0, 2.7f, 0.16f),
                new Vector3(0, 0, 0.9f),
                new Vector3(0, 0.01f, 0.9f),
                new Vector3(0, -2.25f, 1.8f)
            },
            // A segment2
            new Vector3[]
            {
                new Vector3(0, 2.7f, 0.16f),
                new Vector3(0, 0, -0.9f),
                new Vector3(0, 0.01f, -0.9f),
                new Vector3(0, -2.25f, -1.8f)
            },
            // A segment3
            new Vector3[]
            {
                new Vector3(0, 0, 0.9f),
                new Vector3(0, 0, 0),
                new Vector3(0, 0.01f, 0),
                new Vector3(0, 0, -0.9f)
            },
            // B segment1
            new Vector3[]
            {
                new Vector3(0, 2.7f, 1.85f),
                new Vector3(0, 0.35f, 1.85f),
                new Vector3(0, 0.11f, 1.85f),
                new Vector3(0, -2.2f, 1.85f)
            },
            // B segment2
            new Vector3[]
            {
                new Vector3(0, 2.7f, 1.85f),
                new Vector3(0, 2.20f, -1.5f), // Different
                new Vector3(0, 0.89f, -1.5f), // Different
                new Vector3(0, 0.35f, 1.8f),
                new Vector3(0, 0.35f, -2.35f), // Different
                new Vector3(0, -2.2f, -2.35f), // Different
                new Vector3(0, -2.2f, 1.85f)
            },
             // C
            new Vector3[]
            {
                new Vector3(0, 1.23f, -1.4f),
                new Vector3(0, 2.78f, -0.34f),
                new Vector3(0, 2.78f, 0.52f),
                new Vector3(0, 1.23f, 1.56f),
                new Vector3(0, 0.40f, 1.83f),
                new Vector3(0, -0.40f, 1.83f),
                new Vector3(0, -1.23f, 1.56f),
                new Vector3(0, -2.36f, 0.52f),
                new Vector3(0, -2.36f, -0.34f),
                new Vector3(0, -0.95f, -1.4f)
            },
            // S new
            new Vector3[]
            {
                new Vector3(0, 1.07f, -1.07f),
                new Vector3(0, 2.58f, -0.59f),
                new Vector3(0, 2.58f, 0.59f),
                new Vector3(0, 1.07f, 1.07f),
                new Vector3(0, 0.17f, 0.70f),
                new Vector3(0, -0.59f, -0.89f),
                new Vector3(0, -1.60f, -1.07f),
                new Vector3(0, -2.34f, -0.59f),
                new Vector3(0, -2.34f, 0.59f),
                new Vector3(0, -1.60f, 1.07f)
            }
       
        };

        // Initialize the first segment if you want to start with one segment
        SelectSegment(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSegment(0); //A1
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSegment(1); //A2
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSegment(2); //A3
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSegment(3); //B1
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSegment(4); //B2
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectSegment(5); //C
        if (Input.GetKeyDown(KeyCode.Alpha7)) SelectSegment(6); //S
        // Add more key checks for other segments if necessary

        if (objectToMove != null && points != null && points.Length >= 4)
        {
            // Increment t correctly over time
            float distanceToTravel = speed * Time.deltaTime;
            t += distanceToTravel / totalLength;
            if (t > 1f)
            {
                t = 0f; // Loop the animation
            }

            // Determine which segment of the curve we are in
            float totalT = t * segmentCount;
            int segmentIndex = Mathf.FloorToInt(totalT);
            float segmentT = totalT - segmentIndex;

            // Ensure segment index is within bounds
            if (segmentIndex < segmentCount)
            {
                Vector3 p0 = points[segmentIndex * 3];
                Vector3 p1 = points[segmentIndex * 3 + 1];
                Vector3 p2 = points[segmentIndex * 3 + 2];
                Vector3 p3 = points[segmentIndex * 3 + 3];

                Vector3 newPosition = BazierLogic.GetPoint(p0, p1, p2, p3, segmentT);
                objectToMove.position = newPosition;
                objectToMove.LookAt(newPosition + BazierLogic.GetFirstDerivative(p0, p1, p2, p3, segmentT));
            }
        }
    }

    public void SelectSegment(int index)
    {
        if (index < 0 || index >= allSegments.Length)
        {
            Debug.LogError("Invalid segment index");
            return;
        }

        points = allSegments[index];
        segmentCount = (points.Length - 1) / 3;
        t = 0f; // Reset t when switching segments
        currentSegmentIndex = index;
        //Debug.Log("Selected Segment: " + index + ", Segment Count: " + segmentCount);

        // Calculate the length of each segment and the total length
        segmentLengths = new float[segmentCount];
        totalLength = 0f;
        for (int i = 0; i < segmentCount; i++)
        {
            Vector3 p0 = points[i * 3];
            Vector3 p1 = points[i * 3 + 1];
            Vector3 p2 = points[i * 3 + 2];
            Vector3 p3 = points[i * 3 + 3];
            segmentLengths[i] = CalculateBezierLength(p0, p1, p2, p3);
            totalLength += segmentLengths[i];
        }
    }

    float CalculateBezierLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int numSamples = 10)
    {
        float length = 0f;
        Vector3 previousPoint = p0;
        for (int i = 1; i <= numSamples; i++)
        {
            float t = i / (float)numSamples;
            Vector3 currentPoint = BazierLogic.GetPoint(p0, p1, p2, p3, t);
            length += Vector3.Distance(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
        return length;
    }
}
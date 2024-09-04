using System.Collections.Generic;
using UnityEngine;

namespace Scenes._50_Minigames._52_LetterGarden.Scripts.oldCode
{
    [System.Serializable]
    public class LineSegment
    {
        public List<Vector3> points; // List of points in the segment

        public LineSegment(params Vector3[] points)
        {
            this.points = new List<Vector3>(points);
        }
    }

    [System.Serializable]
    public class Letter
    {
        public string letterName;
        public List<LineSegment> segments;

        public Letter(string letterName, List<LineSegment> segments)
        {
            this.letterName = letterName;
            this.segments = segments;
        }
    }

    public class Letters : MonoBehaviour
    {
        public List<Letter> letters = new List<Letter>(); // List of letters

        private void Start()
        {
            // Add predefined letters and their segments
            letters.Add(new Letter("A", new List<LineSegment>
            {
                new LineSegment(new Vector3(0, 2.7f, 0.16f), new Vector3(0, 0, 0.9f), new Vector3(0, -2.25f, 1.8f)),
                new LineSegment(new Vector3(0, 2.7f, 0.16f), new Vector3(0, 0, -0.9f), new Vector3(0, -2.25f, -1.8f)),
                new LineSegment(new Vector3(0, 0, 0.9f), new Vector3(0, 0, 0), new Vector3(0, 0, -0.9f))
            }));


            letters.Add(new Letter("B", new List<LineSegment> //fitted for Bezier
            {
                new LineSegment(new Vector3(0, 2.7f, 1.85f), new Vector3(0, 0.35f, 1.85f), new Vector3(0, -2.2f, 1.85f)),
                new LineSegment(
                    new Vector3(0, 2.7f, 1.85f),
                    new Vector3(0, 2.20f, -0.95f),
                    new Vector3(0, 0.89f, -0.95f),
                    new Vector3(0, 0.35f, 1.8f),
                    new Vector3(0, -0.47f, -1.5f),
                    new Vector3(0, -1.47f, -1.5f),
                    new Vector3(0, -2.2f, 1.85f))
            }));


            letters.Add(new Letter("C", new List<LineSegment>  //fitted for Bezier
            {
                new LineSegment(
                    new Vector3(0, 1.23f, -1.4f),
                    new Vector3(0, 2.78f, -0.34f),
                    new Vector3(0, 2.78f, 0.52f),
                    new Vector3(0, 1.23f, 1.56f),
                    new Vector3(0, 0.40f, 1.83f),
                    new Vector3(0, -0.40f, 1.83f),
                    new Vector3(0, -1.23f, 1.56f),
                    new Vector3(0, -2.36f, 0.52f),
                    new Vector3(0, -2.36f, -0.34f),
                    new Vector3(0, -0.95f, -1.4f))
            }));
            letters.Add(new Letter("S", new List<LineSegment> //fitted for Bezier
            {
                new LineSegment(
                    new Vector3(0, 1.07f, -1.07f),
                    new Vector3(0, 2.58f, -0.59f),
                    new Vector3(0, 2.58f, 0.59f),
                    new Vector3(0, 1.07f, 1.07f),
                    new Vector3(0, 0.17f, 0.70f),
                    new Vector3(0, -0.59f, -0.89f),
                    new Vector3(0, -1.60f, -1.07f),
                    new Vector3(0, -2.34f, -0.59f),
                    new Vector3(0, -2.34f, 0.59f),
                    new Vector3(0, -1.60f, 1.07f))
            }));

            // Shuffle the letters list to ensure random order
            ShuffleLetters();
        }

        void ShuffleLetters()
        {
            for (int i = 0; i < letters.Count; i++)
            {
                Letter temp = letters[i];
                int randomIndex = Random.Range(i, letters.Count);
                letters[i] = letters[randomIndex];
                letters[randomIndex] = temp;
            }
        }
    }
}
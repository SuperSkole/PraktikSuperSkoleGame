using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Splines;
using Vector3 = UnityEngine.Vector3;

namespace Scenes.Minigames.LetterGarden.Scripts
{
    public static class LineSegmentEvaluator
    {
        [SerializeField] private static float maxDist = 4;
        [SerializeField] private static float ispointAtEndOfSplineCutoff = 0.8f;
        [SerializeField] private static float ispointAtStartOfSplineCutoff = 0.2f;

        /// <summary>
        /// evaluates the given drawing compared to the spline
        /// </summary>
        /// <param name="spline">the spline of a letter</param>
        /// <param name="drawing">the drawing the player has made</param>
        /// <returns>true if the drawing is good enough. false if there are any conflict or wrong thing</returns>
        public static bool EvaluateSpline(Spline spline, LineRenderer drawing, Vector3 offset)
        {
            float totalDist = 0;
            bool isPointAtEndOfSpline = false;
            bool isPointAtStartOfSpline = false;

            for (int i = 0; i < drawing.positionCount; i++)
            {
                Vector3 currentDrawingPoint = drawing.GetPosition(i);
                currentDrawingPoint -= offset;
                currentDrawingPoint.x = 0;

                float distToSpline = SplineUtility.GetNearestPoint(spline, currentDrawingPoint, out float3 currentNearestSplinePoint, out float currentT);

                distToSpline = Mathf.Clamp(distToSpline, 0, 5);
                totalDist += distToSpline -0.2f;

                if (currentT >= ispointAtEndOfSplineCutoff)
                {
                    isPointAtEndOfSpline = true;
                }
                if(currentT <= ispointAtStartOfSplineCutoff)
                {
                    isPointAtStartOfSpline = true;
                }
            }


            if (!isPointAtEndOfSpline || !isPointAtStartOfSpline) 
            {
                return false;//checks that the end of the drawing is within the last 20% of the spline
            }

            if(totalDist >= maxDist)
            {
                return false;
            }

            return true;
        }
    }
}
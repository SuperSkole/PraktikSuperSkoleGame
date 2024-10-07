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
        [SerializeField] private static float maxDist = 1;

        [SerializeField] private static float totalCorrectDirectionPrecentageCutoff = 0.8f;
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
            float totalCorrectDirectionPercentage = 0;
            Vector3 previousDrawingPoint = drawing.GetPosition(0) - offset;
            bool isPointAtEndOfSpline = false;
            bool isPointAtStartOfSpline = false;

            SplineUtility.GetNearestPoint(spline, previousDrawingPoint, out float3 previousNearestSplinePoint, out float previousT);


            for (int i = 0; i < drawing.positionCount; i++)
            {
                Vector3 currentDrawingPoint = drawing.GetPosition(i);
                currentDrawingPoint -= offset;
                currentDrawingPoint.x = 0;

                float distToSpline = SplineUtility.GetNearestPoint(spline, currentDrawingPoint, out float3 currentNearestSplinePoint, out float currentT);

                distToSpline = Mathf.Clamp(distToSpline, 0, 5);
                totalDist += distToSpline -0.2f;

                // Validate the direction between consecutive points
                Vector3 splineDirection = ((Vector3)currentNearestSplinePoint - (Vector3)previousNearestSplinePoint).normalized;
                Vector3 drawingDirection = (currentDrawingPoint - previousDrawingPoint).normalized;

                float dotProduct = Vector3.Dot(splineDirection, drawingDirection);
                if (dotProduct > 0)
                {
                    totalCorrectDirectionPercentage += 1f / (float)drawing.positionCount;
                }
                else
                {
                    totalCorrectDirectionPercentage -= 1f / (float)drawing.positionCount;
                }

                previousDrawingPoint = currentDrawingPoint;
                previousNearestSplinePoint = currentNearestSplinePoint;

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

            if(totalCorrectDirectionPercentage >= totalCorrectDirectionPrecentageCutoff)
            {
                return false;
            }

            if(totalDist >= maxDist)
            {
                return false;
            }

            return true;
        }
    }
}
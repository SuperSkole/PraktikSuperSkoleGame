using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Splines;

namespace Scenes.Minigames.LetterGarden.Scripts
{
    public static class LineSegmentEvaluator
    {
        static float maxDist = 5;
        static float totalMaxDist = 20;
        static float totalDist = 0;
        /// <summary>
        /// evaluates the given drawing compared to the spline
        /// </summary>
        /// <param name="spline">the spline of a letter</param>
        /// <param name="dwaing">the drawing the player has made</param>
        /// <returns>true if the drawing is good enough. false if there are any conflict or wrong thing</returns>
        public static bool EvaluateSpline(Spline spline, LineRenderer dwaing)
        {
            float totalDist = 0;
            float oldT = -0.1f;
            Vector3 temp = dwaing.GetPosition(0);
            
            SplineUtility.GetNearestPoint(spline, temp, out float3 nearest, out float firstT);
            Vector3 offSet = new Vector3(temp.x - nearest.x, temp.y - nearest.y, temp.z - nearest.z);
            for (int i = 0; i < dwaing.positionCount; i++)
            {
                Vector3 dwaingPoint = dwaing.GetPosition(i) + offSet;
                dwaingPoint.x = 0;
                float distToSpline = SplineUtility.GetNearestPoint(spline,dwaingPoint,out _,out float t);
                if (oldT > (t + 0.25f))
                {
                    return false;//makes sure you are drawing in the correct direction
                }
                oldT = t;
                distToSpline -= 02f;
                distToSpline = Mathf.Clamp(distToSpline, 0,5);
                totalDist += distToSpline;
            }
            temp = dwaing.GetPosition(dwaing.positionCount-1) + offSet;
            //temp.x = 0;
            //temp.y -= 2.308069f;
            SplineUtility.GetNearestPoint(spline, temp, out _, out float lastT);
            if (lastT < 0.8f) 
            {
                return false;//checks that the end of the drawing is within the last 5% of the spline
            }
            bool testResult = totalDist <= maxDist;
            if (testResult)
            {
                LineSegmentEvaluator.totalDist += totalDist;
            }
            return true;
        }

        /// <summary>
        /// evaluates the combined accuracy of all accepted slpines since last time this was called. also resets the total distance so restart the current letter after calling this, or go to the next one.
        /// </summary>
        /// <returns>true if the letter is good enough. false if not.</returns>
        public static bool EvaluateLetter()
        {
            bool testResult = totalDist <= totalMaxDist;
            totalDist = 0;
            return testResult;
        }
    }
}
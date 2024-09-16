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
        /// <summary>
        /// evaluates the given drawing compared to the spline
        /// </summary>
        /// <param name="spline">the spline of a letter</param>
        /// <param name="dwaing">the drawing the player has made</param>
        /// <returns>true if the drawing is good enough. false if there are any conflict or wrong thing</returns>
        public static bool EvaluateSpline(Spline spline, LineRenderer dwaing)
        {
            float totalDist = 0;
            float totalCorrectDirectionPrecentage = 0;
            float oldT = -0.1f;
            bool ispointAtEndOfSpline = false;
            Vector3 temp = dwaing.GetPosition(0);
            
            SplineUtility.GetNearestPoint(spline, temp, out float3 nearest, out float _);
            Vector3 offSet = new Vector3(temp.x - nearest.x, temp.y - nearest.y, temp.z - nearest.z);
            for (int i = 0; i < dwaing.positionCount; i++)
            {
                Vector3 dwaingPoint = dwaing.GetPosition(i) + offSet;
                dwaingPoint.x = 0;
                float distToSpline = SplineUtility.GetNearestPoint(spline,dwaingPoint,out _,out float t);
                if (oldT > (t + 0.25f))
                {
                    totalCorrectDirectionPrecentage -= 1f / (float)dwaing.positionCount;
                }
                else
                {
                    totalCorrectDirectionPrecentage += 1f / (float)dwaing.positionCount;
                }
                if(t >= ispointAtEndOfSplineCutoff) ispointAtEndOfSpline = true;
                oldT = t;
                distToSpline -= 02f;
                distToSpline = Mathf.Clamp(distToSpline, 0,5);
                totalDist += distToSpline;
            }


            if (!ispointAtEndOfSpline) 
            {
                Debug.Log("end of dwaing is not at end of line");
                return false;//checks that the end of the drawing is within the last 20% of the spline
            }

            if(totalCorrectDirectionPrecentage <= totalCorrectDirectionPrecentageCutoff)
            {
                Debug.Log("too mutch of the dwaing was in the worng direction");
                return false;
            }

            if(totalDist >= maxDist)
            {
                Debug.Log("You were to inacriate");
                return false;
            }

            return true;
        }
    }
}
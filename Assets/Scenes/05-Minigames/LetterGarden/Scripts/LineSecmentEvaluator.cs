using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Splines;

namespace Scenes.Minigames.LetterGarden.Scrips
{
    public static class LineSecmentEvaluator
    {
        static float maxDist = 5;
        static float totalMaxDist = 20;
        static float totalDist = 0;
        /// <summary>
        /// evaluates the given dwaing compaired to the spline
        /// </summary>
        /// <param name="spline">the spilne of a letter</param>
        /// <param name="dwaing">the dwaing the player has made</param>
        /// <returns>true if the dwaing is good enught. false if there are any conflict or worng thing</returns>
        public static bool EvaluateSpline(Spline spline, LineRenderer dwaing)
        {
            float totalDist = 0;
            float oldT = -0.1f;
            Vector3 temp = dwaing.GetPosition(0);
            temp.x = 0;
            temp.y -= 2.308069f;
            SplineUtility.GetNearestPoint(spline, temp, out _, out float firstT);
            if (firstT > 0.05f) return false;//checks that the start of the dwaing is within the first 5% of the spline
            for (int i = 0; i < dwaing.positionCount; i++)
            {
                Vector3 dwaingPoint = dwaing.GetPosition(i);
                dwaingPoint = new(0,dwaingPoint.y - 2.308069f,dwaingPoint.z);
                float distToSpline = SplineUtility.GetNearestPoint(spline,dwaingPoint,out _,out float t);
                if (oldT > (t + 0.05f))return false;//makes sure you are dwaing in the currect direction
                oldT = t;
                distToSpline -= 1f;
                distToSpline = Mathf.Clamp(distToSpline, 0,5);
                totalDist += distToSpline;
            }
            temp = dwaing.GetPosition(dwaing.positionCount-1);
            temp.x = 0;
            temp.y -= 2.308069f;
            SplineUtility.GetNearestPoint(spline, temp, out _, out float lastT);
            if (lastT < 0.95f) return false;//checks that the end of the dwaing is within the last 5% of the spline
            bool testResult = totalDist <= maxDist;
            if (testResult)
            {
                LineSecmentEvaluator.totalDist += totalDist;
            }
            return testResult;
        }

        /// <summary>
        /// evaluates the combind accresy of all acsepted spines since last time this was called. also resets the total distenc so restart the current letter after calling this, or go to the next one.
        /// </summary>
        /// <returns>true if the letter is good enugth. false if not.</returns>
        public static bool EvaluateLetter()
        {
            bool testResult = totalDist <= totalMaxDist;
            totalDist = 0;
            return testResult;
        }
    }
}

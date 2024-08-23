using UnityEngine;
using UnityEngine.Splines;

namespace Scenes.Minigames.LetterGarden.Scrips
{
    public class LineSecmentEvaluator
    {
        static float maxDist = 1;
        static float totalMaxDist = 2;
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
            float oldT = 0;

            SplineUtility.GetNearestPoint(spline, dwaing.GetPosition(0), out _, out float firstT);
            if(firstT > 0.05f) return false;//checks that the start of the dwaing is within the first 5% of the spline
            for (int i = 0; i < dwaing.positionCount; i++)
            {
                Vector3 dwaingPoint = dwaing.GetPosition(i);
                float distToSpline = SplineUtility.GetNearestPoint(spline,dwaingPoint,out _,out float t);
                if(oldT > t) return false;//makes sure you are dwaing in the currect direction
                oldT = t;
                distToSpline -= 0.25f;
                distToSpline = Mathf.Clamp(distToSpline, 0,5);
                totalDist += distToSpline;
            }
            SplineUtility.GetNearestPoint(spline, dwaing.GetPosition(dwaing.positionCount-1), out _, out float lastT);
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

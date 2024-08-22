using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.Mathematics;
using Unity.Splines.Examples;
using UnityEngine;
using UnityEngine.Splines;

namespace Scenes.Minigames.LetterGarden.Scrips
{
    public class LineSecmentEvaluator
    {
        static float maxDist = 1;
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
                if(oldT > t) return false;
                oldT = t;
                distToSpline -= 0.25f;
                distToSpline = Mathf.Clamp(distToSpline, 0,5);
                totalDist += distToSpline;
            }
            SplineUtility.GetNearestPoint(spline, dwaing.GetPosition(0), out _, out float lastT);
            if (lastT < 0.95f) return false;//checks that the end of the dwaing is within the last 5% of the spline
            return totalDist <= maxDist;
        }
    }
}

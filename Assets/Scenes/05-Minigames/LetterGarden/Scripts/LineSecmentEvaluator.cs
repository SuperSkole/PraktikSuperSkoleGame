using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.Mathematics;
using Unity.Splines.Examples;
using UnityEngine;
using UnityEngine.Splines;

namespace Scenes.Minigames.LetterGarden.Scrips
{
    public class LineSecmentEvaluator : MonoBehaviour
    {
        float maxDist = 1;
        public bool EvaluateSpline(Spline spline, LineRenderer dwaing)
        {
            float totalDist = 0;
            for (int i = 0; i < dwaing.positionCount; i++)
            {
                Vector3 dwaingPoint = dwaing.GetPosition(i);
                float distToSpline = SplineUtility.GetNearestPoint(spline,dwaingPoint,out float3 nerrest,out float t);
                distToSpline -= 0.25f;
                totalDist += distToSpline;
            }
            return totalDist <= maxDist;
        }
    }
}

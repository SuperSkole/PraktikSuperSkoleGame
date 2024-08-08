using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode()]
public class SplineSampler : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private int splineIndex;
    [SerializeField][Range(0f, 1f)] private float time;

    float3 postion;
    float3 forward;
    float3 upVector;

    private void Update()
    {
        splineContainer.Evaluate(splineIndex, time, out postion, out forward, out upVector);

        //Tangent is the (forward) direction of travel along the spline to the next point
        //Find the *right* direction based on this
        float3 right = Vector3.Cross(forward, upVector).normalized;

    }
    private void OnDrawGizmos()
    {
        Handles.matrix = transform.localToWorldMatrix;
        Handles.SphereHandleCap(0,postion, quaternion.identity, 1f,EventType.Repaint);
    }


}

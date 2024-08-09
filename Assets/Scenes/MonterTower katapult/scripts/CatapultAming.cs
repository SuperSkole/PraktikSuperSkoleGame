using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Unity.Mathematics;
using UnityEngine;

public class CatapultAming : MonoBehaviour
{
    [SerializeField] GameObject prjectipePrefab;
    [SerializeField] GameObject Catapult;
    [SerializeField] Transform shootPos;
    [SerializeField] float hight = 1;

    /// <summary>
    /// calcolates the terejectory to hit at point and returnes a velosity to hit it
    /// </summary>
    /// <param name="target">the target you want to hit</param>
    /// <returns></returns>
    Vector3 CalcolateAngle(Vector3 target)
    {
        Vector3 output = Vector3.zero;
        hight = 4;
        do
        {
            hight++;
            float displaymentY = target.y - shootPos.position.y;
            Vector3 displaymentXZ = new Vector3(target.x - shootPos.position.x, 0, target.z - shootPos.position.z);

            Vector3 velocetyY = Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * hight);
            Vector3 velocetyXZ = displaymentXZ / (Mathf.Sqrt(-2 * hight / Physics.gravity.y) + Mathf.Sqrt(2 * (displaymentY - hight) / Physics.gravity.y));
            output = velocetyXZ + velocetyY;
        }
        while (output.IsNaN());
        return output;
    }

    /// <summary>
    /// the func that is called when we want to shoot at a target.
    /// it spawns and fires a projectile
    /// </summary>
    /// <param name="target">the target you want to hit</param>
    public void Shoot(Vector3 target)
    {
        GameObject temp = Instantiate(prjectipePrefab,shootPos.position,quaternion.identity);
        Rigidbody rb = temp.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = CalcolateAngle(target);
    }
}

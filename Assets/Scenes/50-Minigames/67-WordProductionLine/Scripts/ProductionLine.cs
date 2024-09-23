using System.Collections;
using UnityEngine;

public class ProductionLine : MonoBehaviour
{
    public float speed;
    Rigidbody rBody;

    public bool conveyerBeltOn = true;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        if (conveyerBeltOn)
        {
            MoveConveyerBelt();
        }

    }
    /// <summary>
    /// Shakes the Belt. making it look like it moves the object straight.
    /// </summary>
    private void MoveConveyerBelt()
    {
        Vector3 pos = rBody.position;
        rBody.position += Vector3.right * speed * Time.fixedDeltaTime;
        rBody.MovePosition(pos);
    }
}



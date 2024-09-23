using System.Collections;
using System.Collections.Generic;
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

    private void MoveConveyerBelt()
    {
        Vector3 pos = rBody.position;
        rBody.position += Vector3.right * speed * Time.fixedDeltaTime;
        rBody.MovePosition(pos);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMoveSpaceShip : MonoBehaviour
{
    [SerializeField] int speed;
    [SerializeField] float shipAcceleration;
    [SerializeField] Rigidbody rigidBody;

    Vector3 forceOnPlayer;
    Vector3 rotation=Vector3.zero;
    private bool isAccelerating;

    // Start is called before the first frame update
    void Start()
    {
        shipAcceleration = 100;
    }

    private void Update()
    {
        isAccelerating = Input.GetAxis("Vertical") > 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (isAccelerating)
        {
            rigidBody.AddForce(transform.up*shipAcceleration);
            rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, 100);
            
        }
        

        if(Input.GetAxis("Horizontal")>0f)
        { 
           
            transform.Rotate(new Vector3(0,0,-2f));
        }
        else if (Input.GetAxis("Horizontal") < 0f)
        {
            transform.Rotate(new Vector3(0,0,2f));
        }





    }


    
}

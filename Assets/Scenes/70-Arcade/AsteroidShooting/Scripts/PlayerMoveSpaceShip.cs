using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMoveSpaceShip : MonoBehaviour
{
    [SerializeField] int speed;
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] int shipAcelleration;

    // Start is called before the first frame update
    void Start()
    {
        shipAcelleration = 100;
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") > 0f)
        {
            rigidBody.AddForce(new Vector2(transform.up.x,transform.up.y) * shipAcelleration);
            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity, 100);
        }


    }

    // Update is called once per frame
    void Update()
    {

   

        if (Input.GetAxis("Horizontal") < 0f)
        {
            transform.Rotate(0, 0, -1);
        }

        if (Input.GetAxis("Horizontal") > 0f)
        {
            transform.Rotate(0, 0, 1);
        }





    }

}

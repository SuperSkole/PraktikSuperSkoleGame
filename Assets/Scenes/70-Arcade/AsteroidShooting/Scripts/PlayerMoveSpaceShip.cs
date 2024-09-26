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
    [SerializeField] int shipAccelleration;
    [SerializeField] GameObject engineFX;

    // Start is called before the first frame update
    void Start()
    {
        shipAccelleration = 200;
    }

    private void FixedUpdate()
    {
        // Adds a force based on the transform.up vector and the shipaccelleration when the up input is used on the used device. 
        if (Input.GetAxis("Vertical") > 0f)
        {
            engineFX.SetActive(true);
            rigidBody.AddForce(new Vector2(transform.up.x,transform.up.y) * shipAccelleration);
            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity, 200);
        }
        else
        {
            engineFX.SetActive(false);
        }


    }

    // Update is called once per frame
    void Update()
    {

   
        //Rotates the PlayerSpaceShip based on the left and right input of the used device. 
        if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            transform.Rotate(0, 0, 1.5f);
        }

        if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            transform.Rotate(0, 0, -1.5f);
        }





    }

}

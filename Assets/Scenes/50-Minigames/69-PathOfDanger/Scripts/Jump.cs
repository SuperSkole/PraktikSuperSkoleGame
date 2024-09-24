using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Jump : MonoBehaviour
{
    public Rigidbody rigidbody;

    [SerializeField] float yForce=15;

    private bool isJumping = false;

    private bool canJump = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump == true)
        {
            isJumping = true;
            canJump = false;
        }
    }

    void FixedUpdate()
    {
        if(isJumping==true)
        { 
            rigidbody.AddForce(new Vector3(0, yForce, 0),ForceMode.Impulse);
            isJumping = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
    }
}

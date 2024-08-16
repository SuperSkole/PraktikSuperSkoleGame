using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Adjust the speed as needed
    public static bool allowedToMove = true;
    
    //private Rigidbody rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
       // rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (allowedToMove)
        {
            HandleMovement();
        }
    }
    void HandleMovement()
    {
        //Remove Raw to add inertia
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        //float tmpVal = horizontalInput + verticalInput;
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        //print(horizontalInput);
        // Move the player
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        // Flip the player based on the horizontal input
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Moving right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // Moving left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }
}

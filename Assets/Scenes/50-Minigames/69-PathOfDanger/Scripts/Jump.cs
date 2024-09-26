using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Jump : MonoBehaviour
{
    public Rigidbody rigidbody;

    [SerializeField] float yForce=10;

    private bool isJumping = false;

    private bool canJump = true;

    public PathOfDangerManager manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    private void Update()
    {
        //checks spacebar input and if canJump is true which is only the case when the player is colliding with another collider. 
        // That makes is there to make sure you can't jump mid air. 
        if (Input.GetKeyDown(KeyCode.Space) && canJump == true)
        {
            isJumping = true;
            canJump = false;
            manager.PlaySoundFromHearLetterButton();
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

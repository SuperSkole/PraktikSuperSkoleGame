using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System.Net.Mail;
using Spine;

public class SpinePlayerMovement : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset walk;
    public AnimationReferenceAsset idle;
    public string currentState;

    //Blend paramters
    public float walkThreshold = 0.1f;
    public float blendDuration = 0.2f;

    public float moveSpeed = 5.0f;

    public bool facingRight = true;

    void Start()
    {
        currentState = "idle";
        SetCharacterState("Idle");

        //skeletonAnimation.Skeleton.SetAttachment("Monster TOP1", "Monster TOP1");
    }
    void Update()
    {
        //moveSpeed = Mathf.Abs(Input.GetAxis("Horizontal"));
         //Remove Raw to add inertia
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //float tmpVal = horizontalInput + verticalInput;
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        //print(horizontalInput);
        // Move the player
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        ////Flip left or right
        //if (Input.GetAxis("Horizontal") < 0 && !facingRight)
        //{
        //    Flip();
        //}
        //else if (Input.GetAxis("Horizontal") > 0 && facingRight)
        //{
        //    Flip();
        //}

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
            SetCharacterState("Walk");
        }
        else
        {
            SetCharacterState("Idle");
        }

        // Blend between animations based on movement speed
        //if (horizontalInput > walkThreshold && currentState != "Walk")
        //{
        //    SetCharacterState("Walk");
        //}
        //else if (horizontalInput <= walkThreshold && currentState != "Idle")
        //{
        //    SetCharacterState("Idle");
        //}
    }
    //Setting animation function
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        skeletonAnimation.state.SetAnimation(0,animation,loop).TimeScale = timeScale;
    }

    //Simplyfied set animation
    public void SetCharacterState(string state)
    {
        if (state.Equals("Idle") && currentState != "Idle")
        {
            //Blending animations walk - idle
            skeletonAnimation.state.SetAnimation(0, idle, true).MixDuration = blendDuration;
            currentState = "Idle";
        }
        else if (state.Equals("Walk") && currentState != "Walk")
        {
            //Blending animations idle - walk
            skeletonAnimation.state.SetAnimation(0, walk, true).MixDuration = blendDuration;
            currentState = "Walk";
        }
    }

    //void Flip()
    //{
    //    facingRight = !facingRight;
    //    Vector3 theScale = transform.localScale;
    //    theScale.x *= -1;
    //    transform.localScale = theScale;
    //}
}

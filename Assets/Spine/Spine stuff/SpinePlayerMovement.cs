using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System.Net.Mail;

public class SpinePlayerMovement : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset walk;
    public AnimationReferenceAsset idle;
    public string currentState;

    //Blend paramters
    public float walkThreshold = 0.1f;
    public float blendDuration = 0.2f;

    public float moveSpeed = 0f;

    public bool facingRight = true;

    void Start()
    {
        currentState = "idle";
        SetCharacterState("Idle");
        

        skeletonAnimation.Skeleton.SetAttachment("Monster TOP1", "Monster TOP1");
    }
    void Update()
    {
        moveSpeed = Mathf.Abs(Input.GetAxis("Horizontal"));

        //Flip left or right
        if (Input.GetAxis("Horizontal") < 0 && !facingRight)
        {
            Flip();
        }
        else if (Input.GetAxis("Horizontal") > 0 && facingRight)
        {
            Flip();
        }

        // Blend between animations based on movement speed
        if (moveSpeed > walkThreshold && currentState != "Walk")
        {
            SetCharacterState("Walk");
        }
        else if (moveSpeed <= walkThreshold && currentState != "Idle")
        {
            SetCharacterState("Idle");
        }
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

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

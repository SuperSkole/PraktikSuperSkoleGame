using System.Collections;
using System.Collections.Generic;

using Spine;
using Spine.Unity;

using UnityEngine;

namespace Scenes._10_PlayerScene.Scripts
{
    public class PlayerAnimatior : MonoBehaviour
    {
        public SkeletonAnimation skeletonAnimation;
        public AnimationReferenceAsset walk;
        public AnimationReferenceAsset throwing;
        public AnimationReferenceAsset idle;
        public string currentState;

        public float blendDuration = 0.2f;

        private void Start()
        {
            StartUp();
        }
        public void StartUp()
        {
            currentState = "idle";
            SetCharacterState("Idle");
        }
        /// <summary>
        /// Sets the player's animation based on the specified parameters.
        /// </summary>
        /// <param name="animation">The animation to set.</param>
        /// <param name="loop">Whether the animation should loop.</param>
        /// <param name="timeScale">The speed at which the animation should play.</param>
        public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
        {
            skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        }

        /// <summary>
        /// Sets the player's animation state to either idle or walk, with blending between states.
        /// </summary>
        /// <param name="state">The desired animation state ("Idle" or "Walk").</param>
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
            else if (state.Equals("Throw") && currentState != "Throw")
            {
                //Blending animations idle - walk
                skeletonAnimation.state.SetAnimation(0, throwing, false).MixDuration = blendDuration;
                currentState = "Throw";
            }
        }
    }
}

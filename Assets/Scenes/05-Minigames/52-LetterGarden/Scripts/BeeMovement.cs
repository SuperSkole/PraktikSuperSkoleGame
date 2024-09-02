using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace Scenes.Minigames.LetterGarden.Scrips
{
    public class BeeMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 1;
        [SerializeField] private GameObject SplineParent;

        public SplineContainer letterSpline;

        private Vector3 currentPos;
        private Vector3 direction;
        private float distancePercentage = 0;
        private float spineLeangth;
        public int splineIndex = 0;


        /// <summary>
        /// Called to switch to the next letter, once the previous one has been completed.
        /// </summary>
        public void NextLetter(SplineContainer currentLetter)
        {
            letterSpline = currentLetter;
            splineIndex = 0;
            spineLeangth = letterSpline.CalculateLength(splineIndex);
        }

        private void Update()
        {
            if (letterSpline != null)
            {
                MoveOnSpline();
                CheckDistance();
            }
        }

        /// <summary>
        /// Checks how close to completion of the current path the bee is.
        /// TODO: Remove this once merged with the code to check if the previous line is done.
        /// </summary>
        private void CheckDistance()
        {
            if (distancePercentage >= 1.05)
            {
                distancePercentage = -0.05f;
            }
        }

        /// <summary>
        /// sends the bee to the next line of the letter.
        /// </summary>
        /// <returns>returns false if you cant go to the next line(we are out of lines) and returns true if it sucsesfully moves on to the next line</returns>
        public bool NextSplineInLetter()
        {
            bool result = splineIndex < letterSpline.Splines.Count - 1;
            if (result)
            {
                splineIndex++;
                spineLeangth = letterSpline.CalculateLength(splineIndex);
            }
            distancePercentage = 0f;
            return result;
        }

        /// <summary>
        /// call onece pr frame to move the bee on the current path.
        /// </summary>
        public void MoveOnSpline()
        {
            distancePercentage += speed * Time.deltaTime / spineLeangth;

            currentPos = letterSpline.EvaluatePosition(splineIndex, distancePercentage);
            transform.position = currentPos;

            direction = (Vector3)letterSpline.EvaluatePosition(splineIndex, distancePercentage + 0.05f) - currentPos;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction, Vector3.back);
            }
        }
    }
}
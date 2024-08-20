using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Splines;

namespace Scenes.Minigames.LetterGarden.Scrips
{
    public class BeeMovement : MonoBehaviour
    {
        [SerializeField] private SplineContainer letterSpline;
        [SerializeField] private float speed = 1;

        private Vector3 currentPos;
        private Vector3 direction;
        private float distancePercentage = 0;
        private float spineLeangth;
        private int splineIndex = 0;


        void Start()
        {
            spineLeangth = letterSpline.CalculateLength(splineIndex);
            
        }


        void Update()
        {
            if (letterSpline != null) MoveOnSpline();
        }

        /// <summary>
        /// sends the bee to the next line of the letter.
        /// </summary>
        /// <returns>returns false if you cant go to the next line(we are out of lines) and returns true if it sucsesfully moves on to the next line</returns>
        public bool NextSplineInLetter()
        {
            if(splineIndex >= letterSpline.Splines.Count - 1) 
                return false;
            splineIndex++;
            distancePercentage = 0;
            spineLeangth = letterSpline.CalculateLength(splineIndex);
            return true;
        }

        /// <summary>
        /// call onece pr frame to move the bee on the current path.
        /// </summary>
        public void MoveOnSpline()
        {
            distancePercentage += speed * Time.deltaTime / spineLeangth;

            currentPos = letterSpline.EvaluatePosition(splineIndex,distancePercentage);
            transform.position = currentPos;
            if(distancePercentage >= 1) distancePercentage = 0;

            direction = (Vector3)letterSpline.EvaluatePosition(splineIndex, distancePercentage + 0.05f) - currentPos;
            transform.rotation = Quaternion.LookRotation(direction,Vector3.back);
        }
    }
}
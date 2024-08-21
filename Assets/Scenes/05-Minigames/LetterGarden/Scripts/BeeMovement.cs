using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Splines;

namespace Scenes.Minigames.LetterGarden.Scrips
{
    public class BeeMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 1;
        [SerializeField] private GameObject SplineParent;
        
        private SplineContainer letterSpline;
        private List<SplineContainer> letterList;

        private Vector3 currentPos;
        private Vector3 direction;
        private float distancePercentage = 0;
        private float spineLeangth;
        private int splineIndex = 0;

        /// <summary>
        /// Runs at start and dynamically fetches all splines used to draw letters/symbols, then selects one at random.
        /// </summary>
        void Start()
        {
            letterList = new();
            foreach (Transform spline in SplineParent.GetComponentInChildren<Transform>())
            {
                letterList.Add(spline.gameObject.GetComponent<SplineContainer>());
            }
            letterSpline = letterList[Random.Range(0, letterList.Count)];
            spineLeangth = letterSpline.CalculateLength(splineIndex);
        }


        void Update()
        {
            if (letterSpline != null)
            {
                MoveOnSpline();
                CheckDistance();
            }
        }

        /// <summary>
        /// sends the bee to the next line of the letter.
        /// </summary>
        /// <returns>returns false if you cant go to the next line(we are out of lines) and returns true if it sucsesfully moves on to the next line</returns>
        public bool NextSplineInLetter()
        {
            if(splineIndex >= letterSpline.Splines.Count - 1)
            {
                splineIndex = 0;
            }
            else
                splineIndex++;
            distancePercentage = 0;
            spineLeangth = letterSpline.CalculateLength(splineIndex);
            if(splineIndex == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Checks how close to completion of the current path the bee is.
        /// </summary>
        private void CheckDistance()
        {
            if (distancePercentage >= 1)
            {
                NextSplineInLetter();
            }
        }

        /// <summary>
        /// call onece pr frame to move the bee on the current path.
        /// </summary>
        public void MoveOnSpline()
        {
            distancePercentage += speed * Time.deltaTime / spineLeangth;

            currentPos = letterSpline.EvaluatePosition(splineIndex,distancePercentage);
            transform.position = currentPos;

            direction = (Vector3)letterSpline.EvaluatePosition(splineIndex, distancePercentage + 0.05f) - currentPos;
            transform.rotation = Quaternion.LookRotation(direction,Vector3.back);
        }
    }
}
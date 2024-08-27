using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Minigames.WordFactory.Scripts
{
    public class ClosestTeethFinder : MonoBehaviour
    {
        public Transform centerPoint; 

        /// <summary>
        /// Finds the closest teeth to the center point.
        /// </summary>
        /// <param name="gears">List of gears</param>
        /// <returns>List of the closest teeth</returns>
        public List<Transform> FindClosestTeeth(List<GameObject> gears)
        {
            List<Transform> closestTeeth = new List<Transform>();

            foreach (GameObject gear in gears)
            {
                Transform teethContainer = gear.transform.Find("TeethContainer");
                Transform closestTooth = GetClosestTooth(teethContainer);
                if (closestTooth != null)
                {
                    closestTeeth.Add(closestTooth);
                }
            }
            return closestTeeth;
        }

        /// <summary>
        /// Gets the closest tooth to the center point from a given teeth container.
        /// </summary>
        /// <param name="teethContainer">Transform containing all the teeth</param>
        /// <returns>Transform of the closest tooth</returns>
        private Transform GetClosestTooth(Transform teethContainer)
        {
            Transform closestTooth = null;
            float minDistance = float.MaxValue;

            foreach (Transform tooth in teethContainer)
            {
                float distance = Vector3.Distance(tooth.position, centerPoint.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTooth = tooth;
                }
            }

            return closestTooth;
        }
    }
}
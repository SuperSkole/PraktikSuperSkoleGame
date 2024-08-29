using System.Collections.Generic;
using System.Linq;
using Scenes._05_Minigames.WordFactory.Scripts;
using Scenes._05_Minigames.WordFactory.Scripts.Managers;
using UnityEngine;

namespace Scenes._05_Minigames._56_WordFactory.Scripts
{
    public class ClosestTeethFinder : MonoBehaviour
    {
        public Transform CenterPoint;

        /// <summary>
        /// Finds the closest teeth to the center point, considering game mode requirements.
        /// </summary>
        /// <param name="gears">List of gears</param>
        /// <param name="mode">Game mode</param>
        /// <returns>List of the closest teeth</returns>
        public List<Transform> FindClosestTeeth(List<GameObject> gears)
        {
            // Get the current gear strategy from the game manager
            var gearStrategy = WordFactoryGameManager.Instance.GetGearStrategy();

            // Determine game mode based on gear strategy
            int mode = gearStrategy is SingleGearStrategy ? 1 : 2; 

            switch (mode)
            {
                case 1: // Mode where one specific gear and its associated block are needed
                    return FindTeethForSingleGear(gears);
                case 2: // Default mode processing
                    return FindTeethForMultipleGears(gears);
                default:
                    return new List<Transform>();
            }
        }

        /// <summary>
        /// Find the teeth closest to the center from a specific single gear with a prefab block.
        /// </summary>
        /// <param name="gears">List of gears</param>
        /// <returns>List of the closest teeth</returns>
        private List<Transform> FindTeethForSingleGear(List<GameObject> gears)
        {
            // Find the consonant prefab in scene
            GameObject singleGearWordBlockPrefab
                = WordFactoryGameManager.Instance.GetWordBlock();


            var closestTeeth = new List<Transform>();
            
            // Check if the prefab exists and add all children to the list
            if (singleGearWordBlockPrefab != null)
            {
                foreach (Transform child in singleGearWordBlockPrefab.transform)
                {
                    closestTeeth.Add(child);
                }
            }

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
        /// Finds the closest teeth for each gear in the list.
        /// </summary>
        /// <param name="gears">List of gears</param>
        /// <returns>List of the closest teeth</returns>
        private List<Transform> FindTeethForMultipleGears(List<GameObject> gears)
        {
            var closestTeeth = new List<Transform>();

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
                float distance = Vector3.Distance(tooth.position, CenterPoint.position);
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

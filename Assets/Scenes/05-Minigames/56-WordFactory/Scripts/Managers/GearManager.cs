using System.Collections.Generic;
using Scenes._05_Minigames.WordFactory.Scripts;
using Scenes._05_Minigames.WordFactory.Scripts.Managers;
using Scenes.Minigames.WordFactory.Scripts;
using UnityEngine;

namespace Scenes._05_Minigames._56_WordFactory.Scripts.Managers
{
    public class GearManager : MonoBehaviour
    {
        // List of all gears in the game
        public List<GameObject> Gears; 
        
        // Reference to the LetterGearGenerator object, that store all gears
        public GameObject LetterGearGenerator;
        
        [SerializeField] private ClosestTeethFinder closestTeethFinder;
        private List<Transform> previousClosestTeeth = new List<Transform>();
        private GameObject singleGearWordBlockPrefab;
        private List<Transform> prefabBlocks = new List<Transform>();
        

        private void Start()
        {
            PopulateGearsList();
            if (WordFactoryGameManager.Instance.GetGearStrategy() is SingleGearStrategy)
            {
                LoadPrefabBlocks();
            }
            
            PopulateGearsList();
        }

        /// <summary>
        /// Populate the gears list by getting all child objects of the LetterGearGenerator.
        /// </summary>
        private void PopulateGearsList()
        {
            Gears = new List<GameObject>();
            foreach (Transform child in LetterGearGenerator.transform)
            {
                if (child.CompareTag("Gear")) 
                {
                    Gears.Add(child.gameObject);
                }
            }
        }
        
        private void LoadPrefabBlocks()
        {
            GameObject singleGearWordBlockPrefab = GameObject.Find("ConsonantWordBlock");
            
            if (singleGearWordBlockPrefab != null)
            {
                // Find the consonant prefab in scene
                foreach (Transform child in singleGearWordBlockPrefab.transform)
                {
                    prefabBlocks.Add(child); 
                }
            }
            else
            {
                Debug.LogError("SingleGearWordBlockPrefab is not assigned in the inspector!");
            }
        }
        
        /// <summary>
        /// Subscribe to the rotation event when the object is enabled
        /// </summary>
        private void OnEnable()
        {
            GearRotationController.OnRotationComplete += UpdateClosestTeethColor;
            ColorTooth.OnBlinkingCompleted += StartGame;
        }

        /// <summary>
        /// Unsubscribe from the rotation event when the object is disabled
        /// </summary>
        private void OnDisable()
        {
            GearRotationController.OnRotationComplete -= UpdateClosestTeethColor;
            ColorTooth.OnBlinkingCompleted -= StartGame;
        }

        /// <summary>
        /// Method that starts the game by updating the closest teeth color and subscribing to relevant events.
        /// </summary>
        private void StartGame()
        {
            // Direct call to update the closest teeth color
            UpdateClosestTeethColor(); 
        }
        
        private void UpdateClosestTeethColor()
        {
            List<Transform> currentClosestTeeth = closestTeethFinder.FindClosestTeeth(Gears);
            
            // Include prefab blocks if in SingleGearStrategy
            if (WordFactoryGameManager.Instance.GetGearStrategy() is SingleGearStrategy)
            {
                currentClosestTeeth.AddRange(prefabBlocks);
            }

            // Uncolor previous closest teeth
            foreach (Transform tooth in previousClosestTeeth)
            {
                ColorTooth.RaiseColorChangeEvent(tooth, Color.white);  
            }

            // Color current closest teeth
            foreach (Transform tooth in currentClosestTeeth)
            {
                ColorTooth.RaiseColorChangeEvent(tooth, Color.cyan);  
            }

            // Update the list for next rotation
            previousClosestTeeth = currentClosestTeeth;  
        }
    }
}

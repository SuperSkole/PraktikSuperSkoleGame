using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Minigames.RulleMarie.Managers
{
    public class WordCheckManager : MonoBehaviour
    {
        // Event to notify subscribers of a valid word
        public static event Action<string> OnValidWord;

        [SerializeField] private GameManager gameManager;
        [FormerlySerializedAs("closestLetterFinder")] [SerializeField] private ClosestTeethFinder closestTeethFinder;
        [SerializeField] private WordBuilder wordBuilder;
        [SerializeField] private WordValidation wordValidation;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private BlockCreator blockCreator;

        [SerializeField] private GameObject notificationTextObject;
        private INotificationDisplay notificationDisplay;

        // Public boolean to allow unlimited blocks for testing
        public bool unlimitedBlocks = false;

        // Boolean flag to restrict creating more than one block per valid word
        private bool canCreateWordBlock = true;

        // HashSet to keep track of created words
        private HashSet<string> createdWords = new HashSet<string>();

        private void Awake()
        {
            // Ensure the notificationTextObject is assigned and active to get the component
            if (notificationTextObject != null)
            {
                bool wasActive = notificationTextObject.activeSelf;
                notificationTextObject.SetActive(true); 

                notificationDisplay = notificationTextObject.GetComponent<INotificationDisplay>();

                notificationTextObject.SetActive(wasActive); 
            }
        }

        private void Update()
        {
            // Check if the spacebar is pressed
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckForWord();
            }
        }

        private void OnEnable()
        {
            OnValidWord += HandleValidWord;
        }

        private void OnDisable()
        {
            OnValidWord -= HandleValidWord;
        }

        /// <summary>
        /// Checks if the formed word is valid.
        /// </summary>
        public void CheckForWord()
        {
            List<Transform> closestTeeth = closestTeethFinder.FindClosestTeeth(GameManager.Instance.GetGears());
            string formedWord = wordBuilder.BuildWord(closestTeeth);

            if (wordValidation.IsValidWord(formedWord))
            {
                if (unlimitedBlocks || (!createdWords.Contains(formedWord) && canCreateWordBlock))
                {
                    notificationDisplay.DisplayNotification("Valid word: " + formedWord);
                    Debug.Log("Valid word: " + formedWord);
    
                    scoreManager.AddScore(formedWord.Length);
                    OnValidWord?.Invoke(formedWord);
                    createdWords.Add(formedWord);

                    // Blink each closest tooth green using the event system
                    foreach (Transform tooth in closestTeeth)
                    {
                        ColorTooth.RaiseColorChangeEvent(tooth, Color.green, 3, 0.25f);
                    }
                }
                else
                {
                    notificationDisplay.DisplayNotification("Word already used: " + formedWord);
                    Debug.Log("Word already used: " + formedWord);

                    // Blink each closest tooth yellow if the word is repeated using the event system
                    foreach (Transform tooth in closestTeeth)
                    {
                        ColorTooth.RaiseColorChangeEvent(tooth, Color.yellow, 3, 0.25f);
                    }
                }
            }
            else
            {
                notificationDisplay.DisplayNotification("Invalid word: " + formedWord);
                Debug.Log("Invalid word: " + formedWord);

                // Blink each closest tooth red using the event system
                foreach (Transform tooth in closestTeeth)
                {
                    ColorTooth.RaiseColorChangeEvent(tooth, Color.red, 3, 0.25f);
                }
            }
        }


        /// <summary>
        /// Handles the valid word event to reset the flag.
        /// </summary>
        private void HandleValidWord(string word)
        {
            blockCreator.HandleValidWord(word);
            // Reset the flag to allow block creation for the next word
            canCreateWordBlock = true;
        }
    }
}

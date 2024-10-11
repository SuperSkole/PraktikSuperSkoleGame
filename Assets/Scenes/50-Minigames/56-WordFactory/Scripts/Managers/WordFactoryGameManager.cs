using System;
using System.Collections.Generic;
using System.Linq;
using Analytics;
using CORE;
using CORE.Scripts;
using Letters;
using Scenes._10_PlayerScene.Scripts;
using Scenes._50_Minigames._56_WordFactory.Scripts.GameModeStrategy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Words;
using Random = UnityEngine.Random;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.Managers
{
    /// <summary>
    /// Manages the Word Factory game, handling gear addition, player positioning, and scene transitions.
    /// </summary>
    public class WordFactoryGameManager : PersistentSingleton<WordFactoryGameManager>
    {
        [SerializeField] private UIFactoryManager uiFactoryManager;
        public event Action<GameObject> OnGearAdded;

        // public so other can access the "game settings"
        public int Playerlevel { get; private set; }
        public int NumberOfGears = 2; // Default to 2 gears
        public int NumberOfTeeth = 8; // Default to 8 teeth per gear
        public int DifficultyLevel = 1; // Default difficulty level
        public GameObject CoinPrefab;
        public GameObject PlayerSpawnPoint;
        [SerializeField] private GameObject dropOffPoint;
        
        private GameObject wordBlockPrefabForSingleGearMode;
        private List<GameObject> gears = new List<GameObject>();
        private IGearStrategy gearStrategy;
        private WordValidator wordValidator;
        private WordBuilder wordBuilder;
        
        // Lists for storing words and letters
        public List<string> WordList { get; private set; } = new List<string>();
        public List<string> LetterList { get; private set; } = new List<string>();
        
        // track guesses
        public int CorrectWordCount { get; private set; } = 0;   
        public int CorrectWordCountTotal { get; private set; } = 0;   
        public int WrongWordCount { get; private set; } = 0;  
        public int MaxWrongGuesses { get; private set; } = 3;  

        protected override void Awake()
        {
            base.Awake();
            
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            Playerlevel = PlayerManager.Instance.PlayerData.PlayerLanguageLevel;
            //Playerlevel = 3;
            if (Playerlevel < 3)
            {
                // Player level too low, load the main scene
                SceneManager.LoadScene(SceneNames.Main);
    
                // If player has an AutoMovePlayerInFactory component, destroy it
                var autoMoveComponent = PlayerManager.Instance.SpawnedPlayer.GetComponent<AutoMovePlayerInFactory>();
                if (autoMoveComponent != null)
                {
                    Destroy(autoMoveComponent);
                }
                
                // Early return to stop further execution
                return;
            }
            
            IntializeFactoryManager();
        }

        /// <summary>
        /// Initializes the factory manager by setting up the gear strategy.
        /// </summary>
        private void IntializeFactoryManager()
        {
            Debug.Log("IntializeFactoryManager");
            
            // Retrieve the number of gears from GameConfig
            NumberOfGears = GameConfig.NumberOfGears;
            
            // Set the gear strategy based on the number of gears
            SetGearStrategy();
        }

        private void Start()
        {
            wordValidator = this.AddComponent<WordValidator>();
            wordBuilder = this.AddComponent<WordBuilder>();
            
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.PositionPlayerAt(PlayerSpawnPoint);
                PlayerManager.Instance.SpawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = false;
                PlayerManager.Instance.SpawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;
                
                PlayerManager.Instance.SpawnedPlayer.AddComponent<AutoMovePlayerInFactory>();
                PlayerManager.Instance.SpawnedPlayer.GetComponent<AutoMovePlayerInFactory>().DropOffPoint = dropOffPoint;
                PlayerManager.Instance.SpawnedPlayer.GetComponent<AutoMovePlayerInFactory>().PlayerSpawnPoint = PlayerSpawnPoint;
                PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerAnimatior>().SetCharacterState("Idle");
                PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerFloating>().enabled = false;
            }
            else
            {
                Debug.Log("WordFactory GM.Start(): Player Manager is null");
            }
            
            CalculateValidWords();
            uiFactoryManager.UpdateWordCounts(CorrectWordCount, CorrectWordCountTotal, WrongWordCount);
        }
        
        /// <summary>
        /// Find all valid words by combining letters from different gears and checking them with the WordValidator.
        /// </summary>
        public void CalculateValidWords()
        {
            int totalCombinations = 0;
            int validWords = 0;
    
            // Fetch the gears and their teeth
            var gears = GetGears();
    
            if (gears == null || gears.Count < 2)
            {
                Debug.LogError("Need at least two gears to form words.");
                return;
            }

            // Loop through teeth of Gear 1 and Gear 2 to form words
            Transform gear1TeethContainer = gears[0].transform.Find("TeethContainer");
            Transform gear2TeethContainer = gears[1].transform.Find("TeethContainer");

            if (gear1TeethContainer == null || gear2TeethContainer == null)
            {
                Debug.LogError("Teeth containers missing in one or both gears.");
                return;
            }

            List<Transform> teethCombination = new List<Transform>();
            WordBuilder wordBuilder = GetComponent<WordBuilder>();

            // Loop through all teeth in Gear 1 and Gear 2
            foreach (Transform tooth1 in gear1TeethContainer)
            {
                foreach (Transform tooth2 in gear2TeethContainer)
                {
                    // Combine teeth from Gear 1 and Gear 2
                    teethCombination.Clear();
                    teethCombination.Add(tooth1);
                    teethCombination.Add(tooth2);

                    // Build the word
                    string formedWord = wordBuilder.BuildWord(teethCombination);
                    int wordLength = formedWord.Length;

                    // Validate the word
                    bool isValid = wordValidator.IsValidWord(formedWord, wordLength);
                    totalCombinations++;

                    if (isValid)
                    {
                        validWords++;
                    }
                }
            }

            // Log the result and update the UI
            Debug.Log($"Valid words: {validWords} out of {totalCombinations}");
            CorrectWordCountTotal += validWords;
        }
        
        /// <summary>
        /// Retrieves the current gear strategy.
        /// </summary>
        public IGearStrategy GetGearStrategy() => gearStrategy;
        
        public void ResetWordCounts()
        {
            CorrectWordCount = 0;
            WrongWordCount = 0;
        }

        // Methods to increase word counts
        public void IncrementCorrectCount() => CorrectWordCount++;
        public void IncrementWrongCount() => WrongWordCount++;

        /// <summary>
        /// Adds a gear to the game and invokes the OnGearAdded event.
        /// </summary>
        public void AddGear(GameObject gear)
        {
            gears.Add(gear);
            OnGearAdded?.Invoke(gear);
        }

        public List<GameObject> GetGears() => gears;
        public GameObject GetWordBlock() => wordBlockPrefabForSingleGearMode;
        public GameObject SetWordBlock(GameObject prefab) => wordBlockPrefabForSingleGearMode = prefab;

        public int GetNumberOfGears() => NumberOfGears;

        public int GetNumberOfTeeth() => NumberOfTeeth;

        public int GetDifficultyLevel() => DifficultyLevel;
        
        // Methods to clear and populate word and letter lists
        public void ClearWordAndLetterLists()
        {
            WordList.Clear();
            LetterList.Clear();
        }

        public void AddWordsToList(IEnumerable<string> words)
        {
            WordList.AddRange(words);
        }

        public void AddLettersToList(IEnumerable<string> letters)
        {
            LetterList.AddRange(letters);
        }

        public void SetDifficultyLevel(int level) => DifficultyLevel = level;
        
        /// <summary>
        /// Sets the gear strategy based on the number of gears.
        /// </summary>
        private void SetGearStrategy()
        {
            // GameManager.Instance.PerformanceWeightManager.SetEntityWeight("klø", 60);
            // GameManager.Instance.PerformanceWeightManager.SetEntityWeight("klo", 60);
            
            // Clear existing lists
            ClearWordAndLetterLists();

            // Fetch language units from DDA system
            var languageUnits = DynamicDifficultyAdjustmentManager.Instance.GetNextLanguageUnitsBasedOnLevel(20);
            
            // If not enough units are found, handle fallback logic
            if (languageUnits.Count < 5)
            {
                Debug.LogWarning("Not enough language units found, fetching additional units.");
                languageUnits.AddRange(DynamicDifficultyAdjustmentManager.Instance.GetNextLanguageUnitsBasedOnLevel(20));
            }
            
            // Add words and letters to the lists
            AddWordsToList(languageUnits.Where(u => u is WordData)
                .Select(u => u.Identifier.ToUpper()));

            AddLettersToList(languageUnits.Where(u => u is LetterData)
                .Select(u => u.Identifier.ToUpper()));

            // Get the highest-weighted word
            ILanguageUnit highestWeightedUnit = languageUnits
                .Where(unit => unit is WordData) // Ensure you're only looking at words
                .OrderByDescending(unit => unit.CompositeWeight)
                .FirstOrDefault();

            if (highestWeightedUnit != null)
            {
                WordData wordData = highestWeightedUnit as WordData;

                if (wordData != null)
                {
                    if (wordData.Length == WordLength.TwoLetters)
                    {
                        gearStrategy = new MultiGearStrategy();
                        NumberOfGears = 2;
                    }
                    else if (wordData.Length == WordLength.ThreeLetters)
                    {
                        gearStrategy = new SingleGearStrategy();
                        NumberOfGears = 1;
                    }
                }
            }
            else
            {
                Debug.LogError("No valid word data found to determine gear strategy.");
            }

            // Additional setup or error handling
            if (gearStrategy == null)
            {
                Debug.LogError("Failed to initialize gear strategy.");
            }
        }

        
        /// <summary>
        /// If Next scene is main re-enable player movement and destroy factory singleton managers.
        /// </summary>
        private void OnSceneUnloaded(Scene scene)
        {
            if (scene.name == SceneNames.Factory)
            {
                // remove automove
                PlayerManager.Instance.SpawnedPlayer
                    .GetComponent<PlayerFloating>()
                    .enabled = true;
                //StopCoroutine(PlayerManager.Instance.SpawnedPlayer.GetComponent<AutoMovePlayerInFactory>().MoveToPositionCoroutine(null));
                Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<AutoMovePlayerInFactory>());
        
                // Clean up the game manager and sound manager when transitioning to the main scene
                Destroy(Instance);
                instance = null; 
                Destroy(WordFactorySoundManager.Instance);
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}
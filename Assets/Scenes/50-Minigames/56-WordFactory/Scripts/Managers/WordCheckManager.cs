using System;
using System.Collections;
using System.Collections.Generic;
using Analytics;
using CORE.Scripts;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.Managers
{
    public class WordCheckManager : MonoBehaviour
    {
        // Event to notify subscribers of a valid word
        public static event Action<string> OnValidWord;

        [SerializeField] private WordFactoryGameManager wordFactoryGameManager;
        [SerializeField] private UIFactoryManager uiFactoryManager;
        [SerializeField] private ClosestTeethFinder closestTeethFinder;
        [SerializeField] private WordBuilder wordBuilder;
        [SerializeField] private WordValidator wordValidator;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private BlockCreator blockCreator;
        [SerializeField] private AudioSource pullHandleAudioSource;
        
        //private bool hasPlayedPullHandleSound = false;
        public bool waitingForInput = false;
        // Public boolean to allow unlimited blocks for testing
        public bool unlimitedBlocks = false;

        // Boolean flag to restrict creating more than one block per valid word
        private bool canCreateWordBlock = true;

        // HashSet to keep track of created wordsOrLetters
        private HashSet<string> createdWords = new HashSet<string>();
        private bool isWordValid;
        
        // created word queue
        private Queue<string> wordQueue = new Queue<string>();
        private bool isProcessingWord = false;
        private int wrongWordCount;
        public bool isTutorialOver = false;

        private void Start()
        {
            // Fetch the current word counts from WordFactoryGameManager when the scene starts
            int correct = WordFactoryGameManager.Instance.CorrectWordCount;
            int total = WordFactoryGameManager.Instance.CorrectWordCountTotal;
            int wrong = WordFactoryGameManager.Instance.WrongWordCount;

            // Update the UI with the fetched values
            uiFactoryManager.UpdateWordCounts(correct, total, wrong);
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
            AutoMovePlayerInFactory.OnBlockDroppedOff += ProcessNextWord;
        }

        private void OnDisable()
        {
            OnValidWord -= HandleValidWord;
            AutoMovePlayerInFactory.OnBlockDroppedOff -= ProcessNextWord;
        }

        /// <summary>
        /// Checks if the formed word is valid.
        /// </summary>
        public void CheckForWord()
        {
            // Play PullHandle sound if it's not already playing
            WordFactorySoundManager.Instance.PlaySound(WordFactorySoundManager.SoundEvent.PullHandle);

            
            List<Transform> closestTeeth = closestTeethFinder.FindClosestTeeth(WordFactoryGameManager.Instance.GetGears());
            string formedWord = wordBuilder.BuildWord(closestTeeth);
            int wordlength = formedWord.Length;

            isWordValid = WordFactoryGameManager.Instance.NumberOfGears >= 2
                ? wordValidator.IsValidWord(formedWord,
                    wordlength)
                : wordValidator.IsValidCombinationWord(formedWord,
                    formedWord.Substring(0,
                        2));

            if (isWordValid)
            {
                if (unlimitedBlocks || (!createdWords.Contains(formedWord) && canCreateWordBlock))
                {
                    Debug.Log("Valid word: " + formedWord);
                    
                    // Report correct guess to DDA system
                    DynamicDifficultyAdjustmentManager.Instance.UpdateLanguageUnitWeight(formedWord.ToLower(), true);
    
                    isTutorialOver = true;
                    scoreManager.AddScore(formedWord.Length);
                    OnValidWord?.Invoke(formedWord);
                    createdWords.Add(formedWord);

                    // Blink each closest tooth green using the event system
                    foreach (Transform tooth in closestTeeth)
                    {
                        ColorTooth.RaiseColorChangeEvent(tooth, Color.green, 3, 0.25f);
                    }
                    
                    WordFactoryGameManager.Instance.IncrementCorrectCount();
                    
                    uiFactoryManager.UpdateInfoBoard(WordCheckState.Correct);
                    uiFactoryManager.TriggerLightBlink(WordCheckState.Correct, 3);
                    int correct = WordFactoryGameManager.Instance.CorrectWordCount;
                    int total = WordFactoryGameManager.Instance.CorrectWordCountTotal;
                    int wrong = WordFactoryGameManager.Instance.WrongWordCount;
                    uiFactoryManager.UpdateWordCounts(correct, total, wrong);
                }
                else
                {
                    Debug.Log("Word already used: " + formedWord);

                    // Blink each closest tooth yellow if the word is repeated using the event system
                    foreach (Transform tooth in closestTeeth)
                    {
                        ColorTooth.RaiseColorChangeEvent(tooth, Color.yellow, 3, 0.25f);
                    }
                    
                    uiFactoryManager.UpdateInfoBoard(WordCheckState.Repeated);
                    uiFactoryManager.TriggerLightBlink(WordCheckState.Repeated, 3);
                }
                
                if (WordFactoryGameManager.Instance.CorrectWordCount >= 3)
                {
                    TriggerSuccessEffect();
                }
                
                // if we want to let them find all words 
                if (WordFactoryGameManager.Instance.CorrectWordCount == WordFactoryGameManager.Instance.CorrectWordCountTotal)
                {
                    TriggerSuccessEffect();
                }
            }
            else
            {
                Debug.Log("Invalid word: " + formedWord);
                
                // Report each letter in the word as incorrect to DDA
                foreach (char letter in formedWord)
                {
                    DynamicDifficultyAdjustmentManager.Instance.UpdateLanguageUnitWeight(letter.ToString(), false);  
                }

                // Blink each closest tooth red using the event system
                foreach (Transform tooth in closestTeeth)
                {
                    ColorTooth.RaiseColorChangeEvent(tooth, Color.red, 3, 0.25f);
                }
                
                WordFactoryGameManager.Instance.IncrementWrongCount(); 
                
                uiFactoryManager.UpdateInfoBoard(WordCheckState.Wrong);
                uiFactoryManager.TriggerLightBlink(WordCheckState.Wrong, 3);
                int correct = WordFactoryGameManager.Instance.CorrectWordCount;
                int total = WordFactoryGameManager.Instance.CorrectWordCountTotal;
                int wrong = WordFactoryGameManager.Instance.WrongWordCount;
                int maxWrongGuesses = WordFactoryGameManager.Instance.MaxWrongGuesses;
                uiFactoryManager.UpdateWordCounts(correct, total, wrong);
                
                if (wrong >= maxWrongGuesses)
                {
                    TriggerFailureEffect();
                }
            }
        }


        /// <summary>
        /// Handles the valid word event to reset the flag.
        /// </summary>
        /// <param name="word">The valid word</param>
        private void HandleValidWord(string word)
        {
            // add newest word to queue
            wordQueue.Enqueue(word);
            
            blockCreator.HandleValidWord(word);
        
            // process the word queue
            if (!isProcessingWord)
            {
                ProcessNextWord();
            }
        }

        private void ProcessNextWord()
        {
            if (wordQueue.Count > 0)
            {
                isProcessingWord = true;
                string word = wordQueue.Dequeue();        
                
                GameObject createdBlock = WordBlockUtilities.FindCurrentBlock(word);
                if (createdBlock != null)
                {
                    PlayerEvents.RaiseMovePlayerToBlock(createdBlock);
                }
                else
                {
                    Debug.LogError("Word block not found.");
                }
        
                Instantiate(WordFactoryGameManager.Instance.CoinPrefab);
                PlayerEvents.RaiseGoldChanged(1);
                PlayerEvents.RaiseXPChanged(1);
                WordFactorySoundManager.Instance.PlaySound(WordFactorySoundManager.SoundEvent.GainGold);
                waitingForInput = true;
                AddWordToPlayerData(word);
                AddWordToHighScore(word);        
                
                canCreateWordBlock = true;
            }
            else
            {
                isProcessingWord = false;
                
                // Move player back to the starting position if no more wordsOrLetters are to be processed
                PlayerEvents.RaiseMovePlayerToPosition(WordFactoryGameManager.Instance.PlayerSpawnPoint);
            }
        }
        
        private void AddWordToPlayerData(string word)
        {
            //Debug.Log($"WordCheckManager.AddWordToPlayerData: added {word} to playerdata list");
            
            // Raise the event to send the word to other parts of the game that manage player data
            PlayerEvents.RaiseAddWord(word);
        }
        
        private void AddWordToHighScore(string word)
        {
            // GameManager.Instance.HighScore.AddWord(word);
        }
        
        // Trigger fireworks and exit the game on 3 correct words
        private void TriggerSuccessEffect()
        {
            WordFactorySoundManager.Instance.PlaySound(WordFactorySoundManager.SoundEvent.GainXP);
            // Blink gears green for success
            StartCoroutine(BlinkGearsAndExit(Color.green));  
        }

        private void TriggerFailureEffect()
        {
            WordFactorySoundManager.Instance.PlaySound(WordFactorySoundManager.SoundEvent.PullHandle);
            // Blink gears red for failure
            StartCoroutine(BlinkGearsAndExit(Color.red)); 
        }

        private IEnumerator BlinkGearsAndExit(Color blinkColor)
        {
            // Blink all gears in the specified color for celebration
            foreach (var gear in WordFactoryGameManager.Instance.GetGears())
            {
                // Blink the gear's teeth in the given color
                BlinkGearTeeth(gear, blinkColor);
            }

            // Wait for blinking to complete (you can adjust timing if needed)
            yield return new WaitForSeconds(3);

            // Exit the game after the blinking effect
            ExitFactory();
        }
        
        private void BlinkGearTeeth(GameObject gear, Color color)
        {
            Transform teethContainer = gear.transform.Find("TeethContainer");
            if (teethContainer != null)
            {
                foreach (Transform tooth in teethContainer)
                {
                    // Use the method you already have to raise a color change event for each tooth
                    ColorTooth.RaiseColorChangeEvent(tooth, color, 3, 0.25f);  // Blinks 3 times at 0.25s interval
                }
            }
        }

        private void ExitFactory()
        {
            SceneLoader.Instance.LoadScene(SceneNames.Main); 
        }
    }
}
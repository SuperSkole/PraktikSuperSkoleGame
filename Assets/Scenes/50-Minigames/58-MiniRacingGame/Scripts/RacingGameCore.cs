using System.Collections;
using System.Collections.Generic;
using _99_Legacy;
using Minigames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    public class RacingGameCore : MonoBehaviour
    {
        #region variables

        private readonly AudioSource gameManagerAudioSource;
        public EndGameUI endGameUI;
        public GameObject StartUI;
        public GameObject playerCar;
        public CarController carController;
        public RacingGameManager racingGameManager;
        private bool showEndUi = false;

        private bool imageInitialized = false;
        private bool hasStartedRace = false;
        public bool ShowEndUi
        {
            get { return showEndUi; }
            set { showEndUi = value; }
        }

        public float Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        private static bool raceActive = true;
        public bool RaceActive
        {
            get { return raceActive; }
            set { raceActive = value; }
        }

        public bool letterDisplayActive;
        public bool imageDisplayActive;
        public bool audioActive;
        public bool timerDisplayActive;

        public enum Checkpoint { CStart, BPn, Cne, Cse, BPs, Csw, Cnw, CEnd }
        public enum Branch { Left, Right }

        public Branch correctBranch; // is set randomly at the start of each lap :)

        // Additions for checkpoint ordering
        private readonly List<Checkpoint> checkpointOrder = new() {
            Checkpoint.CStart, Checkpoint.BPn, Checkpoint.Cne, Checkpoint.Cse, Checkpoint.BPs, Checkpoint.Csw, Checkpoint.Cnw, Checkpoint.CEnd
        };
        private int currentCheckpointIndex = 0;

        //Displays:
        public TextMeshProUGUI leftTextN;
        public TextMeshProUGUI rightTextN;
        public TextMeshProUGUI leftTextS;
        public TextMeshProUGUI rightTextS;

        public TextMeshProUGUI timerTextN;
        public TextMeshProUGUI timerTextS;


        public TextMeshProUGUI wordTextN;
        public TextMeshProUGUI wordTextS;

        private bool displayToggle = false;
        //private List<string> wordsList = new List<string>() { "F" };
        private readonly List<string> wordsList = new() { "FLY", "BIL" };
        //private List<string> wordsList = new List<string>() { "FLY", "BIL", "B�D" };


        private readonly List<string> spelledWordsList = new(); // Tracks spelled words
        private string targetWord = "";
        private int currentIndex = 0;

        private bool timerRunning = false;
        private float timer = 0f;

        public Dictionary<string, AudioClip> wordsAudioMap = new();
        public Dictionary<string, Sprite> wordsImageMap = new(); //sprite? Or texture2D?

        public Image wordImageDisplayN;
        public Image wordImageDisplayS;

        public Transform correctCheckpointTrans;
        public Transform passedCheckpointTrans;
        private string currentMode;
        #endregion

        #region setup
        /// <summary>
        /// Gamemode selection for players.
        /// </summary>
        public void GameMode_Three_Words()
        {
            Setup(GameModes.Mode1);
        }
        public void GamemMode_One_Word()
        {
            Setup(GameModes.Mode2);
        }

        /// <summary>
        /// Ensures the player car is not active until setup is done
        /// </summary>
        void Start()
        {
            playerCar.SetActive(false);
        }

        /// <summary>
        /// Sets up everything to race once the player picks a gamemode.
        /// </summary>
        private void Setup(string gameMode)
        {
            currentMode = gameMode;
            playerCar.SetActive(true);
            StartUI.SetActive(false);
            RaceActive = true;
            raceActive = true;


            carController.GetComponent<CarController>();

            //Set map conditions: (To do : make in seperate script)
            letterDisplayActive = true;
            imageDisplayActive = true;
            audioActive = true;
            timerDisplayActive = true;


            DetermineWordToUse(); // Select a random word from the list
            UpdateBranchAndLetters();
            if (letterDisplayActive == true)
            {
                wordTextN.text = targetWord;
                wordTextS.text = targetWord;
            }
            if (imageDisplayActive == true)
            {
                InitializeWordImageMap();
                UpdateWordImageDisplay(targetWord);
            }
            if (audioActive == true)
            {
                InitializeWordAudioMap();
                PlayWordAudio(targetWord);
            }
            carController.Setup();
        }
        /// <summary>
        /// Picks a random word from the list.
        /// </summary>
        private void SelectRandomWord()
        {
            List<string> availableWords = new(wordsList); // Copy the original list
            availableWords.RemoveAll(word => spelledWordsList.Contains(word)); // Remove already spelled words

            // Check if there are any available words left
            if (availableWords.Count > 0)
            {
                // Randomly select a word from the remaining available words
                targetWord = availableWords[Random.Range(0, availableWords.Count)];
                PlayWordAudio(targetWord);
                UpdateWordImageDisplay(targetWord);
            }
            else
            {
                EndGame();
            }
        }
        #endregion

        #region timer
        /// <summary>
        /// Updates the timer on the billboards.
        /// </summary>
        private void UpdateTimerDisplay()
        {
            if (timerDisplayActive == true)
            {

                timerTextN.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timer / 60), Mathf.FloorToInt(timer) % 60);
                timerTextS.text = timerTextN.text;
            }

        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        private void StartTimer()
        {

            timerRunning = true;
            timer = 0f; // Reset the timer
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        private void StopTimer()
        {
            timerRunning = false;
        }
        #endregion

        #region controls
        /// <summary>
        /// Keeps the timer up-to-date and handles the player input.
        /// </summary>
        private void Update()
        {
            if (timerRunning)
            {
                timer += Time.deltaTime;
                UpdateTimerDisplay();
            }

            if (Input.GetKeyDown(KeyCode.K)) //bare test
            {
                PlayWordAudio(targetWord);
                UpdateWordImageDisplay("Bil");
            }

            if (Input.GetKeyDown(KeyCode.F)) //Respawn
            {
                FlipCar();

            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                this.GetComponent<StateNameController>().ChangeToTownScene();
            }
        }
        /// <summary>
        /// Flips the car, in case the player is stuck.
        /// </summary>
        private void FlipCar()
        {

            Vector3 newPosition = playerCar.transform.position + new Vector3(0, 1.0f, 0);
            playerCar.transform.position = newPosition;

            float yRotation = playerCar.transform.eulerAngles.y;
            playerCar.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
        #endregion

        #region other
        /// <summary>
        /// Checks which game mode is in use
        /// </summary>
        /// <param name="gameMode">The gamemode the racing game is using</param>
        private void DetermineWordToUse()
        {
            if (currentMode == GameModes.Mode1)
            {
                if (spelledWordsList.Count < 2) // TODO: Temporary fix, change to 3 later
                {
                    SelectRandomWord();
                }
                else
                    EndGame();
            }
            else if (currentMode == GameModes.Mode2)
            {
                if (spelledWordsList.Count < 1)
                {
                    SelectRandomWord();
                }
                else
                    EndGame();
            }
        }

        /// <summary>
        /// Ends the game and give players their reward
        /// </summary>
        private void EndGame()
        {
            targetWord = "End";

            leftTextS.text = "";
            rightTextS.text = "";
            leftTextN.text = "";
            rightTextN.text = "";
            raceActive = false;
            showEndUi = true;
            carController.CarActive = false;

            StopTimer();
            racingGameManager = GetComponent<RacingGameManager>();
            racingGameManager.EndGame();
            endGameUI = GetComponent<EndGameUI>();
            endGameUI.ToggleEndGameUI(true);
        }
        #endregion

        #region letters and branches
        /// <summary>
        /// Updates the billboards with the letters and images
        /// that the player have to spell.
        /// </summary>
        public void UpdateBranchAndLetters()
        {
            if (raceActive == true)
            {
                // Randomly decide which branch is correct for this lap
                correctBranch = (Random.Range(0, 2) == 0) ? Branch.Left : Branch.Right;

                char correctLetter = targetWord[currentIndex];
                char wrongLetter = RandomWrongLetter(targetWord);

                // Toggle the display of letters between North and South branching points
                if (displayToggle == false)
                {
                    // Update North branching points
                    leftTextN.text = (correctBranch == Branch.Left) ? correctLetter.ToString() : wrongLetter.ToString();
                    rightTextN.text = (correctBranch == Branch.Right) ? correctLetter.ToString() : wrongLetter.ToString();
                    // Clear South texts 
                    leftTextS.text = "";
                    rightTextS.text = "";
                }
                else
                {
                    // Update South branching points
                    leftTextS.text = (correctBranch == Branch.Left) ? correctLetter.ToString() : wrongLetter.ToString();
                    rightTextS.text = (correctBranch == Branch.Right) ? correctLetter.ToString() : wrongLetter.ToString();
                    // Clear North texts to maintain focus
                    leftTextN.text = "";
                    rightTextN.text = "";
                }
            }
        }
        /// <summary>
        /// Shows a random wrong letter for the wrong choice.
        /// </summary>
        /// <param name="correctLetter"></param>
        /// <returns></returns>
        private char RandomWrongLetter(string correctLetter)
        {
            char wrongLetter;
            do
            {
                wrongLetter = (char)('A' + Random.Range(0, 26));
            } while (CheckLetter(wrongLetter, correctLetter));

            return wrongLetter;
        }

        /// <summary>
        /// Checks that no letter from the correct word is shown as a wrong letter.
        /// </summary>
        private bool CheckLetter(char wrongLetter, string correctWord)
        {
            foreach (char letter in correctWord)
            {
                if(wrongLetter == letter)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// CHecks when a checkpoint is hit.
        /// </summary>
        /// <param name="triggeredCheckpoint">The hit checkpoint</param>
        public void CheckpointTriggered(Checkpoint triggeredCheckpoint)
        {
            // Start the timer when the start checkpoint is triggered
            if (triggeredCheckpoint == Checkpoint.CStart)
            {
                if (raceActive == true)
                {
                    if (!timerRunning)
                    {
                        StartTimer();
                    }
                }
                hasStartedRace = true;

            }

            // Check if the triggered checkpoint is the next in order
            if (triggeredCheckpoint == checkpointOrder[currentCheckpointIndex])
            {
                correctCheckpointTrans = passedCheckpointTrans;

                // Move to the next checkpoint
                currentCheckpointIndex++;
                if (currentCheckpointIndex >= checkpointOrder.Count)
                {


                    currentCheckpointIndex = 0;
                }
            }
            else if (hasStartedRace)
            {
                Debug.Log($"Incorrect checkpoint {triggeredCheckpoint}. Expected {checkpointOrder[currentCheckpointIndex]}.");
            }
        }
        /// <summary>
        /// Triggered when a player picks a branch.
        /// Checks if the choice is correct and sets up what happens after.
        /// </summary>
        /// <param name="branchName">The triggered branch to compare with</param>
        public void BranchTriggered(string branchName)
        {
            if (raceActive == true)
            {
                if (checkpointOrder[currentCheckpointIndex] == Checkpoint.BPn || checkpointOrder[currentCheckpointIndex] == Checkpoint.BPs)
                {
                    Branch chosenBranch = branchName.Contains("LeftC") ? Branch.Left : Branch.Right;

                    if (chosenBranch == correctBranch)
                    {
                        currentIndex++;

                        if (currentIndex >= targetWord.Length)
                        {
                            currentIndex = 0; // Reset for the next game or end game
                            spelledWordsList.Add(targetWord); // Add the spelled word to the list

                            DetermineWordToUse(); // Select a new random word for the next game
                            if (letterDisplayActive == true)
                            {
                                wordTextN.text = targetWord;
                                wordTextS.text = targetWord;
                            }

                            displayToggle = !displayToggle;
                            UpdateBranchAndLetters(); // Update the letters for the new word
                        }
                        else
                        {
                            displayToggle = !displayToggle; // Toggle for the next choice
                            UpdateBranchAndLetters(); // Prepare for the next letter
                        }
                    }
                    else
                    {
                        displayToggle = !displayToggle;
                        UpdateBranchAndLetters(); // Repeat the current letter
                    }
                }
            }
        }
        #endregion

        #region image
        /// <summary>
        /// Sets up the image to be used.
        /// </summary>
        public void InitializeWordImageMap()
        {
            foreach (string word in wordsList)
            {
                string imageFileName = word.ToLower() + "_image";

                Sprite image = Resources.Load<Sprite>($"Pictures/{imageFileName}");
                if (image != null)
                {
                    wordsImageMap[word] = image;
                }
                else
                {
                    Debug.LogWarning($"Image for word '{word}' not found in 'pictures'!");
                }
            }
            imageInitialized = true;
        }

        /// <summary>
        /// Updates the word display.
        /// </summary>
        /// <param name="word"></param>
        private void UpdateWordImageDisplay(string word)
        {
            if (imageDisplayActive == true)
            {
                StartCoroutine(UpdateWordImageDisplay2(word));
            }
        }

        /// <summary>
        /// Updates the word display.
        /// Done as a coroutine to ensure it waits until images are initialized
        /// due to trouble with many things doing startup setups.
        /// </summary>
        /// <param name="word">The word to use to fetch images</param>
        /// <returns></returns>
        private IEnumerator UpdateWordImageDisplay2(string word)
        {
            yield return new WaitUntil(() => imageInitialized == true);
            if (wordsImageMap.TryGetValue(word, out Sprite image))
            {

                wordImageDisplayN.sprite = image;
                wordImageDisplayS.sprite = wordImageDisplayN.sprite;
            }
            else
            {
                Debug.LogWarning($"Failed to update image display for word: {word}");
            }
        }
        #endregion

        #region audio
        /// <summary>
        /// Sets up audio files
        /// </summary>
        public void InitializeWordAudioMap()
        {
            foreach (string word in wordsList)
            {
                string audioFileName = word.ToLower() + "_audio";
                AudioClip clip = Resources.Load<AudioClip>($"AudioWords/{audioFileName}");
                if (clip != null)
                {
                    wordsAudioMap[word] = clip;
                }
                else
                {
                    Debug.LogWarning($"Audio clip for word '{word}' not found!");
                }
            }
        }

        /// <summary>
        /// Plays audio files
        /// </summary>
        /// <param name="word"></param>
        public void PlayWordAudio(string word)
        {
            if (audioActive == true)
            {
                if (wordsAudioMap.TryGetValue(word, out AudioClip clip))
                {
                    AudioSource.PlayClipAtPoint(clip, playerCar.transform.position); // Playing at the camera's position for testing
                }
            }
        }
        #endregion
    }
}
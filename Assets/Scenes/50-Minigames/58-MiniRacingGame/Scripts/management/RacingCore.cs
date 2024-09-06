using Minigames;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    public class RacingCore : MonoBehaviour
    {
        #region variables
        private readonly AudioSource gameManagerAudioSource;
        [SerializeField]
        private EndGameUI endGameUI;
        [SerializeField]
        private GameObject StartUI;
        [SerializeField]
        private GameObject playerCar;
        [SerializeField]
        private CarController carController;
        [SerializeField]
        private RacingGameManager racingGameManager;
        [SerializeField]
        private GameObject coinEffect;

        private bool imageInitialized = false;

        private static bool raceActive = true;

        private bool imageDisplayActive;
        private bool audioActive;

        private enum Branch { Left, Right }

        private Branch correctBranch; // is set randomly at the start of each lap :)

        //Displays:
        private readonly List<TextMeshProUGUI> leftText = new();
        private readonly List<TextMeshProUGUI> rightText = new();
        private readonly List<TextMeshProUGUI> timerText = new();
        private readonly List<Image> billBoard = new();

        private readonly List<TextMeshProUGUI> removeText = new();
        private readonly List<Image> removebillBoard = new();

        private bool displayToggle = false;
        private readonly List<string> wordsList = new() { "FLY", "BIL" };

        private readonly List<string> spelledWordsList = new(); // Tracks spelled words
        private string targetWord = "";
        private int currentIndex = 0;

        private bool timerRunning = false;
        private float timer = 0f;

        public Dictionary<string, AudioClip> wordsAudioMap = new();
        public Dictionary<string, Sprite> wordsImageMap = new(); //sprite? Or texture2D?

        private string currentMode;

        public float Timer { get => timer; set => timer = value; }
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
        private void Start()
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
            raceActive = true;


            carController.GetComponent<CarController>();

            //Set map conditions: (To do : make in seperate script)
            imageDisplayActive = true;
            audioActive = true;


            DetermineWordToUse(); // Select a random word from the list
            InitializeWordImageMap();

            UpdateBillBoard();
            InitializeWordAudioMap();
            PlayWordAudio(targetWord);
            carController.Setup();
            StartTimer();
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

            foreach (TextMeshProUGUI text in leftText)
            {
                text.text = "";
            }
            foreach (TextMeshProUGUI text in rightText)
            {
                text.text = "";
            }
            raceActive = false;

            StopTimer();
            racingGameManager = GetComponent<RacingGameManager>();
            racingGameManager.EndGame();
            endGameUI = GetComponent<EndGameUI>();
            endGameUI.ToggleEndGameUI(true);
        }
        #endregion

        #region letters and branches
        private void AddToList(string listToUse, TextMeshProUGUI newText)
        {
            if (listToUse == "left")
                leftText.Add(newText);
            else if (listToUse == "right")
                rightText.Add(newText);
            else if (listToUse == "timer")
                timerText.Add(newText);
            else
                Debug.LogWarning("Add to list failed");
        }
        private void AddToList(Image newBillboard)
        {
            billBoard.Add(newBillboard);
            UpdateBillBoard();
        }

        /// <summary>
        /// Updates the billboards with the letters and images
        /// that the player have to spell.
        /// </summary>
        private void UpdateBillBoard()
        {
            if (targetWord != "")
            {
                char correctLetter = targetWord[currentIndex];
                char wrongLetter = RandomWrongLetter(targetWord);

                foreach (TextMeshProUGUI text in leftText)
                {
                    if (!text.IsActive())
                        removeText.Add(text);
                    else
                        text.text = (correctBranch == Branch.Left) ? correctLetter.ToString() : wrongLetter.ToString();
                }
                foreach (TextMeshProUGUI text in rightText)
                {
                    if (!text.IsActive())
                        removeText.Add(text);
                    else
                        text.text = (correctBranch == Branch.Right) ? correctLetter.ToString() : wrongLetter.ToString();
                }
                foreach (TextMeshProUGUI text in timerText)
                {
                    if (!text.IsActive())
                        removeText.Add(text);
                    else
                        text.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timer / 60), Mathf.FloorToInt(timer) % 60);
                }
                foreach (Image image in billBoard)
                {
                    if (!image.IsActive())
                        removebillBoard.Add(image);
                }
                ClearLists();
            }
        }

        private void UpdateTimerDisplay()
        {
            foreach (TextMeshProUGUI text in timerText)
            {
                text.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timer / 60), Mathf.FloorToInt(timer) % 60);
            }
        }

        private void ClearLists()
        {
            foreach (TextMeshProUGUI text in removeText)
            {
                leftText.Remove(text);
                rightText.Remove(text);
                timerText.Remove(text);
            }
            foreach (Image image in removebillBoard)
            {
                billBoard.Remove(image);
            }
            removeText.Clear();
            removebillBoard.Clear();
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
                if (wrongLetter == letter)
                {
                    return true;
                }
            }
            return false;
        }

        private void OnEnable()
        {
            {
                RacingBranch.OnBranchTriggered += BranchTriggered;
                BranchAwake.OnBranchImageAwaken += AddToList;
                BranchAwake.OnBranchTextAwaken += AddToList;
            }
        }

        private void OnDisable()
        {
            RacingBranch.OnBranchTriggered -= BranchTriggered;
            BranchAwake.OnBranchImageAwaken -= AddToList;
            BranchAwake.OnBranchTextAwaken -= AddToList;
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
                Branch chosenBranch = branchName.Contains("Left") ? Branch.Left : Branch.Right;

                if (chosenBranch == correctBranch)
                {
                    Instantiate(coinEffect);
                    racingGameManager.xp++;
                    racingGameManager.gold++;
                    currentIndex++;

                    if (currentIndex >= targetWord.Length)
                    {
                        currentIndex = 0; // Reset for the next game or end game
                        spelledWordsList.Add(targetWord); // Add the spelled word to the list

                        DetermineWordToUse(); // Select a new random word for the next game
                    }
                }
                displayToggle = !displayToggle;
                correctBranch = (Random.Range(0, 2) == 0) ? Branch.Left : Branch.Right;
                UpdateBillBoard();
                UpdateWordImageDisplay(targetWord);
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
                foreach (Image image in billBoard)
                {
                    StartCoroutine(UpdateWordImageDisplay2(word, image));
                }
            }
        }

        /// <summary>
        /// Updates the word display.
        /// Done as a coroutine to ensure it waits until images are initialized
        /// due to trouble with many things doing startup setups.
        /// </summary>
        /// <param name="word">The word to use to fetch images</param>
        /// <returns></returns>
        private IEnumerator UpdateWordImageDisplay2(string word, Image billBoard)
        {
            if (word != "End")
            {
                yield return new WaitUntil(() => imageInitialized == true);
                if (wordsImageMap.TryGetValue(word, out Sprite image))
                {
                    billBoard.sprite = image;
                }
                else
                {
                    Debug.LogWarning($"Failed to update image display for word: {word}");
                }
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
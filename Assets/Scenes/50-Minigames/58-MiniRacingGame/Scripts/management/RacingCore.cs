using Analytics;
using CORE;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Minigames;
using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    public class RacingCore : MonoBehaviour, IMinigameSetup
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
        [SerializeField]
        private GameObject levelCreator;
        public readonly FindLetterInPicture gameRuleVocal = new();
        private readonly FindConsonant gameRuleConsonant = new();
        public readonly DynamicGameRules dynamicGameRules = new();
        private new AudioSource audio;

        public bool imageInitialized = false;

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
        [SerializeField]
        private Text objectiveText;

        private readonly List<TextMeshProUGUI> removeText = new();
        private readonly List<Image> removebillBoard = new();

        private bool displayToggle = false;
        //private bool finalStretch = false;

        public readonly List<string> spelledWordsList = new(); // Tracks spelled wordsOrLetters
        public string targetWord = "";
        public string imageWord = "";
        private int currentIndex = 0;
        public readonly string[] level5Consonants = { "f", "m", "n", "s" };

        public List<ILanguageUnit> languageUnits = new List<ILanguageUnit>();

        private bool timerRunning = false;
        private float timer = 0f;

        public Dictionary<string, AudioClip> wordsAudioMap = new();
        public Dictionary<string, Sprite> wordsImageMap = new(); //sprite? Or texture2D?

        public string currentMode;

        public IRacingGameMode racingGameMode;

        private bool setCorrectAnswerHasBeenRan = false;

        public float Timer { get => timer; set => timer = value; }
        public bool isTutorialOver = false;
        #endregion

        #region setup
        /// <summary>
        /// Sets up everything to race once the player picks a gamemode.
        /// </summary>
        public void SetupGame(IGenericGameMode gameMode, IGameRules rule)
        {
            racingGameMode = (IRacingGameMode)gameMode;
            
            if(rule.GetType() == typeof(DynamicGameRules))
            {
                dynamicGameRules.SetCorrectAnswer();
                

                languageUnits = GameManager.Instance.DynamicDifficultyAdjustmentManager
                    .GetNextLanguageUnitsBasedOnLevel(1);
            }
            StartUI.SetActive(false);
            raceActive = true;
            audio = playerCar.GetComponent<AudioSource>();

            //Set map conditions: (To do : make in seperate script)
            imageDisplayActive = true;
            audioActive = true;

            currentMode = racingGameMode.returnMode();
            objectiveText.text = racingGameMode.displayObjective();

            racingGameMode.DetermineWordToUse(this); // Select a random word from the list
            levelCreator.GetComponent<LevelLayoutGenerator>().mapSeedSuggestion = targetWord;
            levelCreator.SetActive(true);
            playerCar.SetActive(true);

            UpdateBillBoard();
            PlayWordAudio(targetWord);
            StartTimer();
        }


        /// <summary>
        /// Picks a random word from the list.
        /// </summary>
        private void SelectRandomWord()
        {
            System.Tuple<Texture2D, string> rndImageWithKey1 = ImageManager.GetRandomImageWithKey();
            while (rndImageWithKey1.Item2.Length != 3)
            {
                rndImageWithKey1 = ImageManager.GetRandomImageWithKey();
            };
            wordsImageMap.Add(rndImageWithKey1.Item2, Sprite.Create(rndImageWithKey1.Item1, new Rect(0, 0, rndImageWithKey1.Item1.width, rndImageWithKey1.Item1.height), new Vector2(0.5f, 0.5f)));
            if (!imageInitialized)
                imageInitialized = true;
            targetWord = rndImageWithKey1.Item2;
            imageWord = targetWord;
            PlayWordAudio(targetWord);
            UpdateWordImageDisplay();
        }
        

        /// <summary>
        /// Selects a random vocal letter
        /// </summary>
        private void SelectRandomVocal()
        {
            do
            {
                gameRuleVocal.SetCorrectAnswer();
                targetWord = gameRuleVocal.GetCorrectAnswer();
            } while (spelledWordsList.Contains(targetWord));
            if (currentMode != GameModes.Mode2)
            {
                imageWord = gameRuleVocal.GetDisplayAnswer();
                Texture2D image = ImageManager.GetImageFromWord(imageWord);
                wordsImageMap.Add(imageWord, QuickSprite(image));
            }
            else
                imageWord = "";


            if (!imageInitialized)
                imageInitialized = true;
            PlayWordAudio(targetWord);
            UpdateWordImageDisplay();
        }
        /// <summary>
        /// Selects a random consonant
        /// </summary>
        private void SelectRandomConsonant()
        {
            do
            {
                targetWord = level5Consonants[Random.Range(0, level5Consonants.Length)];
            } while (spelledWordsList.Contains(targetWord));
            imageWord = "";

            if (!imageInitialized)
                imageInitialized = true;
            PlayWordAudio(targetWord);
            UpdateWordImageDisplay();
        }
        /// <summary>
        /// Called to set up sprites for the billboards
        /// </summary>
        public Sprite QuickSprite(Texture2D image)
        {
            return Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
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
        /// Ends the game and give players their reward
        /// </summary>
        public void EndGame()
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
            //finalStretch = true;
            levelCreator.GetComponent<LevelLayoutGenerator>().finalStretch = true;
        }

        /// <summary>
        /// Activates the ending ui so the player can see their rewards and return to main world.
        /// </summary>
        private void FinalEnd()
        {
            racingGameManager = GetComponent<RacingGameManager>();
            racingGameManager.EndGame();
            endGameUI = GetComponent<EndGameUI>();
            endGameUI.ToggleEndGameUI(true);
        }
        #endregion

        #region letters and branches
        /// <summary>
        /// Add various billboards to the list to be updated
        /// </summary>
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
        /// <summary>
        /// Adds image billboard to the list to be updated
        /// </summary>
        private void AddToList(Image newBillboard)
        {
            billBoard.Add(newBillboard);
            UpdateBillBoard();
        }

        /// <summary>
        /// Updates the billboards with the letters and images
        /// that the player have to spell.
        /// </summary>
        public void UpdateBillBoard()
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
                    else
                        UpdateWordImageDisplay();
                }
                ClearLists();
            }
        }

        /// <summary>
        /// Keeps the timer displays updated
        /// </summary>
        private void UpdateTimerDisplay()
        {
            foreach (TextMeshProUGUI text in timerText)
            {
                text.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timer / 60), Mathf.FloorToInt(timer) % 60);
            }
        }
        /// <summary>
        /// Clears the removal lists
        /// </summary>
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
            if (currentMode == GameModes.Mode3 || currentMode == GameModes.Mode2)
            {
                wrongLetter = gameRuleVocal.GetWrongAnswer().ToCharArray()[0];
            }
            else if (currentMode == GameModes.Mode5)
            {
                wrongLetter = gameRuleConsonant.GetWrongAnswer().ToCharArray()[0];
            }
            else
            {
                do
                {
                    wrongLetter = (char)('a' + Random.Range(0, 26));
                } while (CheckLetter(wrongLetter, correctLetter));
            }

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
                    isTutorialOver = true;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Sets up the listeners
        /// </summary>
        private void OnEnable()
        {
            {
                RacingBranch.OnBranchTriggered += BranchTriggered;
                BranchAwake.OnBranchImageAwaken += AddToList;
                BranchAwake.OnBranchTextAwaken += AddToList;
                TriggerExit.OnFinaleExited += FinalEnd;
                SayWordAgain.OnWordTriggered += RepeatWord;
            }
        }
        /// <summary>
        /// Disables the listeners
        /// </summary>
        private void OnDisable()
        {
            RacingBranch.OnBranchTriggered -= BranchTriggered;
            BranchAwake.OnBranchImageAwaken -= AddToList;
            BranchAwake.OnBranchTextAwaken -= AddToList;
            TriggerExit.OnFinaleExited -= FinalEnd;
            SayWordAgain.OnWordTriggered -= RepeatWord;
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
                    PlayerEvents.RaiseGoldChanged(1);
                    PlayerEvents.RaiseXPChanged(1);
                    
                    currentIndex++;

                    if (currentIndex >= targetWord.Length)
                    {
                        GameManager.Instance.DynamicDifficultyAdjustmentManager.UpdateLanguageUnitWeight(targetWord, true);
                        currentIndex = 0; // Reset for the next game or end game
                        spelledWordsList.Add(targetWord); // Add the spelled word to the list

                        racingGameMode.DetermineWordToUse(this); // Select a new random word for the next game
                    }
                }
                else 
                {
                    GameManager.Instance.DynamicDifficultyAdjustmentManager.UpdateLanguageUnitWeight(targetWord, false);
                }
                displayToggle = !displayToggle;
                correctBranch = (Random.Range(0, 2) == 0) ? Branch.Left : Branch.Right;
                UpdateBillBoard();
                UpdateWordImageDisplay();
            }
        }
        #endregion

        #region image
        /// <summary>
        /// Updates the word display.
        /// </summary>
        /// <param name="word"></param>
        public void UpdateWordImageDisplay()
        {
            if (imageDisplayActive == true)
            {
                foreach (Image image in billBoard)
                {
                    StartCoroutine(UpdateWordImageDisplay2(image));
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
        private IEnumerator UpdateWordImageDisplay2(Image billBoard)
        {
            if (imageWord is not "End" and not "")
            {
                yield return new WaitUntil(() => imageInitialized == true);
                if (wordsImageMap.TryGetValue(imageWord, out Sprite image))
                {
                    billBoard.sprite = image;
                }
                else
                {
                    Debug.LogWarning($"Failed to update image display for word: {imageWord}");
                }
            }
            else if (imageWord == "")
            {
                billBoard.sprite = null;
                billBoard.gameObject.SetActive(false);
            }
        }
        #endregion

        #region audio
        /// <summary>
        /// A simple trigger to be used for repeating the current word
        /// </summary>
        private void RepeatWord()
        {
            PlayWordAudio(targetWord);
        }

        /// <summary>
        /// Plays audio files
        /// </summary>
        /// <param name="word"></param>
        public void PlayWordAudio(string word)
        {
            if (audioActive == true && (currentMode == GameModes.Mode2 || currentMode == GameModes.Mode5))
            {
                AudioClip clip = LetterAudioManager.GetAudioClipFromLetter(word + 1);
                if (clip && audio.isActiveAndEnabled)
                {
                    audio.clip = clip;
                    audio.Play();
                }
            }
        }
        #endregion
    }
}
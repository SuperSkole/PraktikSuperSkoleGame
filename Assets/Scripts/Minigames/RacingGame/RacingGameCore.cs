using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class RacingGameCore : MonoBehaviour
{

    
    int sceneID = 0;

    private AudioSource gameManagerAudioSource;
    public EndGameUI endGameUI;
    public GameObject playerCar;
    public CarController carController;
    public RacingGameManager racingGameManager;
    private bool showEndUi = false;
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
    private List<Checkpoint> checkpointOrder = new List<Checkpoint>() {
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
    private List<string> wordsList = new List<string>() { "FLY" };
    //private List<string> wordsList = new List<string>() { "FLY", "BIL", "BÅD" };


    private List<string> spelledWordsList = new List<string>(); // Tracks spelled words
    private string targetWord = "";
    private int currentIndex = 0;

    private bool timerRunning = false;
    private float timer = 0f;

    public Dictionary<string, AudioClip> wordsAudioMap = new Dictionary<string, AudioClip>();
    public Dictionary<string, Sprite> wordsImageMap = new Dictionary<string, Sprite>(); //sprite? Or texture2D?

    public Image wordImageDisplayN;
    public Image wordImageDisplayS;

    public Transform correctCheckpointTrans;
    public Transform passedCheckpointTrans;


    private void Start()
    {
        sceneID = SceneManagerScript.Instance.SceneID;

        if (sceneID == 1)
        {
            RaceActive = true;
            raceActive = true;
        }

            carController.GetComponent<CarController>();

        //Set map conditions: (To do : make in seperate script)
        letterDisplayActive = true;
        imageDisplayActive = true;
        audioActive = true;
        timerDisplayActive = true;
        

        SelectRandomWord(); // Select a random word from the list
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
        
    }

    private void Update()
    {
        Debug.Log($"{RaceActive}");

        if (timerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerDisplay();
        }

        if (Input.GetKeyDown(KeyCode.K)) //bare test
        {
            Debug.Log("K key pressed."); 
            PlayWordAudio(targetWord);
            UpdateWordImageDisplay("Bil");
        }

        if (Input.GetKeyDown(KeyCode.R)) //Respawn
        {
            //FlipCar();
            Respawn(correctCheckpointTrans);
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

    private void Respawn(Transform checkpointTransform) //Virker ikke xD

    {
        carController.CarActive = false;
        //int respawnTimer = 3;
        

            // Get the current z position of the car to maintain it
            float currentZ = playerCar.transform.position.z;

            // Set the new position to the checkpoint's x and y, but keep the car's current z
            Vector3 newPosition = new Vector3(checkpointTransform.position.x,
                                              checkpointTransform.position.y + 1.0f, // Adding 1 meter to y for elevation
                                              currentZ);
            playerCar.transform.position = newPosition;

            // Adjust the rotation to make the car upright, preserving the y-axis rotation from the checkpoint
            float yRotation = checkpointTransform.eulerAngles.y;
            playerCar.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        carController.CarActive = true;
    }

    private void FlipCar()
    {

        Vector3 newPosition = playerCar.transform.position + new Vector3(0, 1.0f, 0);
        playerCar.transform.position = newPosition;

        float yRotation = playerCar.transform.eulerAngles.y;
        playerCar.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void SelectRandomWord()
    {
        List<string> availableWords = new List<string>(wordsList); // Copy the original list
        availableWords.RemoveAll(word => spelledWordsList.Contains(word)); // Remove already spelled words

        // Check if there are any available words left
        if (availableWords.Count > 0)
        {
            // Randomly select a word from the remaining available words
            targetWord = availableWords[Random.Range(0, availableWords.Count)];
            Debug.Log($"Selected word: {targetWord}, attempting to play audio.");
            PlayWordAudio(targetWord);
            UpdateWordImageDisplay(targetWord);
        }
        else
        {
            targetWord = "End";
            Debug.Log($"Word pile empty! your time is: {timer}! The game is over");
            
            
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

    }

    public void UpdateBranchAndLetters()
    {
        if (raceActive == true)
        {


            // Randomly decide which branch is correct for this lap
            correctBranch = (Random.Range(0, 2) == 0) ? Branch.Left : Branch.Right;

            char correctLetter = targetWord[currentIndex];
            char wrongLetter = RandomWrongLetter(correctLetter);

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

    private void UpdateTimerDisplay()
    {
        if (timerDisplayActive == true)
        {
         
            timerTextN.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timer / 60), Mathf.FloorToInt(timer) % 60);
            timerTextS.text = timerTextN.text;
        }
       
    }

    private char RandomWrongLetter(char correctLetter)
    {
        char wrongLetter;
        do
        {
            wrongLetter = (char)('A' + Random.Range(0, 26));
        } while (wrongLetter == correctLetter);

        return wrongLetter;
    }

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

        }

        // Check if the triggered checkpoint is the next in order
        if (triggeredCheckpoint == checkpointOrder[currentCheckpointIndex])
        {
            


            Debug.Log($"Checkpoint {triggeredCheckpoint} passed.");

            correctCheckpointTrans = passedCheckpointTrans;

            // Move to the next checkpoint
            currentCheckpointIndex++;
            if (currentCheckpointIndex >= checkpointOrder.Count)
            {
               

               currentCheckpointIndex = 0;
            }
        }
        else
        {
            Debug.Log($"Incorrect checkpoint {triggeredCheckpoint}. Expected {checkpointOrder[currentCheckpointIndex]}.");
        }
    }

    private void StartTimer()
    {

        timerRunning = true;
        timer = 0f; // Reset the timer
        Debug.Log("Timer started.");
    }

    private void StopTimer()
    {
        timerRunning = false;
        Debug.Log($"Timer stopped at: {timer} seconds.");
    }

    public void InitializeWordImageMap()
    {
        foreach (string word in wordsList)
        {
            string imageFileName = word.ToLower() + "_image";
            
            Sprite image = Resources.Load<Sprite>($"Pictures/{imageFileName}");
            if (image != null)
            {
                wordsImageMap[word] = image;
                Debug.Log($"Successfully loaded image for word: {word}");
            }
            else
            {
                Debug.LogWarning($"Image for word '{word}' not found in 'pictures'!");
            }
        }
    }

    private void UpdateWordImageDisplay(string word)
    {
        if (imageDisplayActive == true)
        {
            Debug.Log($"Attempting to update image display for word: {word}");
            if (wordsImageMap.TryGetValue(word, out Sprite image))
            {

                wordImageDisplayN.sprite = image;
                wordImageDisplayS.sprite = wordImageDisplayN.sprite;
                Debug.Log($"Image should be changed for word: {word}");
            }
            else
            {
                Debug.LogWarning($"Failed to update image display for word: {word}");
            }
        }
    }

    public void InitializeWordAudioMap()
    {
        foreach (string word in wordsList)
        {
            string audioFileName = word.ToLower() + "_audio"; 
            AudioClip clip = Resources.Load<AudioClip>($"AudioWords/{audioFileName}");
            if (clip != null)
            {
                wordsAudioMap[word] = clip;
                Debug.Log($"Successfully loaded audio clip for word: {word}");
            }
            else
            {
                Debug.LogWarning($"Audio clip for word '{word}' not found!");
            }
        }
    }

    public void PlayWordAudio(string word)
    {
        if (audioActive == true)
        {
            Debug.LogWarning("*trying to enter condition*");
            if (wordsAudioMap.TryGetValue(word, out AudioClip clip))
            {
                Debug.LogWarning("*Inside condition: Attempting to play sound clip*");
                AudioSource.PlayClipAtPoint(clip, playerCar.transform.position); // Playing at the camera's position for testing
                Debug.LogWarning("*PlayClipAtPoint called. Leaving condiotion*");

            }
            Debug.LogWarning("*past condition. Éxiting method*");
        }
    }

    public void BranchTriggered(string branchName)
    {
        if (raceActive == true)
        {
            if (checkpointOrder[currentCheckpointIndex] == Checkpoint.BPn || checkpointOrder[currentCheckpointIndex] == Checkpoint.BPs)
            {
                Branch chosenBranch = branchName.Contains("LeftC") ? Branch.Left : Branch.Right;

                if (chosenBranch == correctBranch)
                {
                    Debug.Log($"Correct choice! Letter: {targetWord[currentIndex]}. Your time is now: {timer}");
                    currentIndex++;

                    if (currentIndex >= targetWord.Length)
                    {
                        Debug.Log($"Congratulations! You spelled the word: {targetWord}! Your time is now: {timer}!");
                        currentIndex = 0; // Reset for the next game or end game
                        spelledWordsList.Add(targetWord); // Add the spelled word to the list

                        SelectRandomWord(); // Select a new random word for the next game
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
                    Debug.Log("Wrong path chosen. Try the letter again!");

                    displayToggle = !displayToggle;
                    UpdateBranchAndLetters(); // Repeat the current letter
                }
            }
        }
    }
}
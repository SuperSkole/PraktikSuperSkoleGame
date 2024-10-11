using Analytics;
using Cinemachine;
using CORE;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes;
using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PathOfDangerManager : MonoBehaviour, IMinigameSetup
{
    // Start is called before the first frame update
    [SerializeField] GameObject playerSpawnPoint;
    [SerializeField] GameObject DeathPlatforms;
    [SerializeField] GameObject platformPrefab;
    public GameObject spawnedPlayer;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] LayerMask playerPlacementLayerMask;
    public int playerLifePoints = 3;
    [SerializeField] int z_AmountOfPlatforms;
    [SerializeField] int x_AmountOfPlatforms;
    [SerializeField] float z_distanceBetweenPlatforms;
    [SerializeField] float x_distanceBetweenPlatforms;
    private PlatformFalling platformPrefabFallingScript;
    private GameObject[,] spawnedPlatforms;
    public GameObject answerHolderPrefab;
    public GameObject singleImageHolderPrefab;
    public GameObject twoImageHolderPrefab;
    public GameObject textHolderPrefab;
    public RawImage topImage;
    public RawImage bottomImage;
    public RawImage soloImage;
    public TextMeshProUGUI textOnPlatform;
    public string imageKey;

  

    public AudioClip hearLetterButtonAudioClip;

    public TextMeshProUGUI descriptionText;
    private IPODGameMode gameMode;
    private string[] questions;
    private int currentQuestionIndex = 0;
    private string currentQuestion;
    [SerializeField] private GameObject coinPrefab;
    public bool correctAnswer = false;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject endPlane;
    private float originalCapsuleColiderHeight;
    [SerializeField] GameObject shadowPrefab;
    [SerializeField] GameObject safePlatform;
    [SerializeField] GameObject healthUI;


    [SerializeField] GameObject mapSection;
    [SerializeField] GameObject planes;
    private bool mapLoaded = false;
    public bool wrongAnswer;
    public bool hasAnsweredWrong=false;
    public bool isTutorialOver = false;

    private LanguageUnit letterModeType;

    [SerializeField] AudioClip backGroundMusic;
    void Start()
    {
        AudioManager.Instance.PlaySound(backGroundMusic, SoundType.Music, true);

        letterModeType=GameManager.Instance.DynamicDifficultyAdjustmentManager.GetNextLanguageUnitsBasedOnLevel(1)[0].LanguageUnitType;

        if (PlayerManager.Instance != null)
        {
            platformPrefabFallingScript = platformPrefab.transform.GetComponent<PlatformFalling>();
            platformPrefabFallingScript.manager = this;
            SetUpPlayerForPathOfDanger();
        }
        else
        {
            Debug.Log("Player Manager is null");
        }
        //Starting a coroutine that is gonna build and setup the planes the player will be jumping on. 
        StartCoroutine(WaitUntillDataIsLoaded());
    }

    /// <summary>
    /// Sets up all the neccessary components on the playercharacter that are needed for this minigame. 
    /// </summary>
    private void SetUpPlayerForPathOfDanger()
    {
        PlayerManager.Instance.PositionPlayerAt(playerSpawnPoint);
        spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
        SpinePlayerMovement playerSpinePlayerMovement = spawnedPlayer.GetComponent<SpinePlayerMovement>();
        playerSpinePlayerMovement.enabled = true;
        playerSpinePlayerMovement.sceneCamera = Camera.main;
        Rigidbody playerRigidBody = spawnedPlayer.GetComponent<Rigidbody>();
        playerRigidBody.useGravity = true;
        CapsuleCollider capsuleCollider= spawnedPlayer.GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = true;
        originalCapsuleColiderHeight = capsuleCollider.height;
        capsuleCollider.height = 12.3f;
        spawnedPlayer.GetComponent<PlayerFloating>().enabled = false;
        PlayerAnimatior playerAnimator = spawnedPlayer.GetComponent<PlayerAnimatior>();
        virtualCamera.Follow = spawnedPlayer.transform;
        virtualCamera.LookAt = spawnedPlayer.transform;
        playerAnimator.SetCharacterState("Idle");
        spawnedPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Jump jumpComp = spawnedPlayer.AddComponent<Jump>();
        jumpComp.playerRigidBody = playerRigidBody;
        jumpComp.manager = this;
        jumpComp.shadowPrefab = shadowPrefab;
        OutOfBounds outOfBouncePComp = spawnedPlayer.AddComponent<OutOfBounds>();
        outOfBouncePComp.startPosition = playerSpawnPoint;
        outOfBouncePComp.manager = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (correctAnswer)
        {
            GameManager.Instance.DynamicDifficultyAdjustmentManager.UpdateLanguageUnitWeight(questions[currentQuestionIndex], true);
            if (letterModeType == LanguageUnit.Letter)
            {
                PlayerEvents.RaiseAddLetter(questions[currentQuestionIndex][0]);
            }

            if (letterModeType == LanguageUnit.Word)
            {
                PlayerEvents.RaiseAddWord(questions[currentQuestionIndex]);
            }

            PlayerEvents.RaiseGoldChanged(1);
            PlayerEvents.RaiseXPChanged(1);

            isTutorialOver = true;
            currentQuestionIndex++;
            SetNextQuestion();           
            correctAnswer = false;
        }

        if(wrongAnswer)
        {
            GameManager.Instance.DynamicDifficultyAdjustmentManager.UpdateLanguageUnitWeight(questions[currentQuestionIndex], false);
            wrongAnswer = false;
        }
        UpdatePlayerHealthUI();
    }

    /// <summary>
    /// Destroys the added Components so it it isn't used outside of pathOfdanger. 
    /// Is used on the back to MainWorld button and restart gamebuttons. 
    /// </summary>
    public void SetupPlayerToDefaultComponents()
    {
        spawnedPlayer.GetComponent<CapsuleCollider>().height = originalCapsuleColiderHeight;
        Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<Jump>());
        Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<OutOfBounds>());   
    }

    public void BuildMap()
    {
        for (int z = 0; z < z_AmountOfPlatforms / 2-1; z++)
        {
            Vector3 spawnPos = new Vector3(-22, 0, 506.5f + z * 59.5f);
            Instantiate(mapSection,spawnPos,mapSection.transform.rotation);
        }
    }

    /// <summary>
    /// Builds the platforms based on the fields x_AmountOfPlatforms and z_AmountOfPlatforms.
    /// Also uses fields that define the distance between the platforms in the x and z axes. 
    /// The correct and incorrect text/image is set and instantiated on the plane based on the gamemode
    /// Then at the end there is a final platform for the endgoal which shows the winUI when colliding with it. 
    /// </summary>
    public void BuildPlatforms()
    {             
        for (int z = 0; z < z_AmountOfPlatforms; z++)
        {
            // Random correct image index is used so the right answer is put randomly between the posible positions. 
            int correctImageIndex = Random.Range(0, x_AmountOfPlatforms);
            for (int x = 0; x < x_AmountOfPlatforms; x++)
            {
                //Making sure that the actual falling platforms are spawned on all the even z values. 
                if (z % 2 == 0)
                {                   
                    //setting the correct and incorrect answer
                    if (x == correctImageIndex)
                    {
                        //dividing z/2 due the fact the z_amount ofplatforms is twice the amount of actual falling platforms. 
                        // that is because the safeplatforms in between is also taken into account. 
                      
                        gameMode.SetCorrectAnswer(questions[z/2], this);
                        platformPrefabFallingScript.isCorrectAnswer = true;
                    }
                    else
                    {
                        
                        gameMode.SetWrongAnswer(this, questions[z/2]);
                        platformPrefabFallingScript.isCorrectAnswer = false;
                    }
                }

               // instantiating the platform and the answerholder. 
                Vector3 pos = new Vector3(x*x_distanceBetweenPlatforms, 0, z*z_distanceBetweenPlatforms)+DeathPlatforms.transform.position;

                // When the number is an uneven number only one platform is spawned and that is the safeplatform. 
                // else the number is even and a falling platform is spawned with an answerholder. 
                if(z%2!=0 && x==0)
                {
                    spawnedPlatforms[x, z] = Instantiate(safePlatform, pos, Quaternion.identity);
                    spawnedPlatforms[x, z].transform.parent = DeathPlatforms.transform;
                }
                else if(z%2==0)
                {
                    spawnedPlatforms[x, z] = Instantiate(platformPrefab, pos, Quaternion.identity);
                    spawnedPlatforms[x, z].transform.parent = DeathPlatforms.transform;                                     
                    GameObject imageholder = Instantiate(answerHolderPrefab, pos + answerHolderPrefab.transform.position, answerHolderPrefab.transform.rotation, spawnedPlatforms[x, z].transform);
                }                                                          
            }          
        }

        // Spawning the end platform
        Vector3 posOffset = new Vector3(4, 0, 0);
        Vector3 endGoalPos = new Vector3(0 * x_distanceBetweenPlatforms, 0, z_AmountOfPlatforms * z_distanceBetweenPlatforms) + DeathPlatforms.transform.position+posOffset;
        spawnedPlatforms[0, z_AmountOfPlatforms] = Instantiate(endPlane, endGoalPos, Quaternion.identity);
        spawnedPlatforms[0, z_AmountOfPlatforms].GetComponent<ShowYouWinUI>().WinUI = winUI;
        spawnedPlatforms[0, z_AmountOfPlatforms].GetComponent<ShowYouWinUI>().manager = this;       
    }

    /// <summary>
    /// Setups the player to defaultComponents and starts a coroutine for switching the scene to the lose screen. 
    /// </summary>
    public void StartGoToLoseScreenCoroutine()
    {
        SetupPlayerToDefaultComponents();
        StartCoroutine(GoToLoseScreen());
    }

    /// <summary>
    /// WaitsForFixedUpdate is used because the setupPlayerToDefaultComponents need to execute before switching scene. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator GoToLoseScreen()
    {
        yield return new WaitForFixedUpdate();
        SwitchScenes.SwitchToPathOfDangerLoseScene();        
    }

    /// <summary>
    /// Destroys all the panels.
    /// </summary>
    public void DestroyAllPanels()
    {
        if (spawnedPlatforms != null)
        {
            foreach (var item in spawnedPlatforms)
            {
                Destroy(item);
            }
            //Times 2 to z_AmountOfPlatforms because there is need for space for the last endgoal platform and the platforms in between the trap platforms. 
            spawnedPlatforms = new GameObject[x_AmountOfPlatforms,z_AmountOfPlatforms+1];
        }
    }


    /// <summary>
    /// updates the display to show the next question
    /// </summary>
    void SetNextQuestion()
    {
        Instantiate(coinPrefab);
        PlayerEvents.RaiseGoldChanged(1);
        PlayerEvents.RaiseXPChanged(1);
        if (questions.Length >= currentQuestionIndex)
        {
            if (questions[currentQuestionIndex-1].Length == 1) PlayerEvents.RaiseAddLetter(questions[currentQuestionIndex - 1][0]);
            else PlayerEvents.RaiseAddWord(questions[currentQuestionIndex-1]);
        }
        if (questions.Length <= currentQuestionIndex) return;
        gameMode.GetDisplayAnswer(questions[currentQuestionIndex], this);       
    }

    /// <summary>
    /// Waits until the imagemanager has loaded its data.
    /// Then generates answers and sets the answerprefab based on the gamemode.
    /// Then also sets the display answer based on the gamemode.
    /// And finally setsup 2D array of platforms and builds the platforms. 
    /// </summary>
    /// <returns></returns>
   public IEnumerator WaitUntillDataIsLoaded()
    {        
        //Both the images needds loaded before the platforms are built 
        //reinsert: || !IsSaveDataLoaded
        while (!ImageManager.IsDataLoaded)
        {
            yield return null;
        }
        currentQuestionIndex = 0;
        questions = gameMode.GenerateAnswers(3);
        //The amounts of platforms is the questions.lenght*2 because the need for safeplatforms in between the falling platforms. 
        z_AmountOfPlatforms = questions.Length*2;
        gameMode.SetAnswerPrefab(this);
        currentQuestion = questions[currentQuestionIndex];
        gameMode.GetDisplayAnswer(currentQuestion, this);
        spawnedPlatforms = new GameObject[x_AmountOfPlatforms, z_AmountOfPlatforms+1];

       if(mapLoaded==false)
       {
            BuildMap();
            mapLoaded = true;
       }
        BuildPlatforms();
    }

    /// <summary>
    /// Sets the gameMode for the game. 
    /// </summary>
    /// <param name="gameMode"></param>
    /// <param name="gameRules"></param>
    public void SetupGame(IGenericGameMode gameMode, IGameRules gameRules)
    {
        this.gameMode = (IPODGameMode)gameMode;     
    }


    /// <summary>
    /// Plays the sound that has been set onto the hearletter button. 
    /// </summary>
    public void PlaySoundFromHearLetterButton()
    {
        AudioManager.Instance.PlaySound(hearLetterButtonAudioClip, SoundType.Voice, false);
    }

    /// <summary>
    /// Updates the playerhealth UI by removing a heart from the screen. 
    /// </summary>
    public void UpdatePlayerHealthUI()
    {
        switch (playerLifePoints)
        {
            case 2:
                healthUI.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case 1:
                healthUI.transform.GetChild(1).gameObject.SetActive(false);
                break;
            case 0:
                healthUI.transform.GetChild(2).gameObject.SetActive(false);
                break;
        }
    }
}

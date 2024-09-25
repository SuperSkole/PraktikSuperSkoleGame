using Cinemachine;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes;
using Scenes._10_PlayerScene.Scripts;
using Scenes._50_Minigames._65_MonsterTower.Scrips.MTGameModes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PathOfDangerManager : MonoBehaviour, IMinigameSetup
{
    // Start is called before the first frame update
    [SerializeField] GameObject playerSpawnPoint;
    [SerializeField] GameObject DeathPlatforms;

    [SerializeField] GameObject platformPrefab;
    private GameObject spawnedPlayer;
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

    public GameObject hearLetterButton;

    public AudioSource hearLetterButtonAudioSource;

    public TextMeshProUGUI descriptionText;

    private IPODGameMode gameMode;
    private string[] questions;
    private int currentQuestionIndex = 0;
    private string currentQuestion;
    private int amountOfOptions;

    [SerializeField] private GameObject coinPrefab;
    public bool correctAnswer = false;

    [SerializeField] GameObject winUI;

    [SerializeField] GameObject endPlane;

    

    void Start()
    {
        hearLetterButtonAudioSource = hearLetterButton.GetComponent<AudioSource>();

        if (PlayerManager.Instance != null)
        {
            platformPrefabFallingScript = platformPrefab.transform.GetChild(0).GetComponent<PlatformFalling>();

            PlayerManager.Instance.PositionPlayerAt(playerSpawnPoint);
            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;

            SpinePlayerMovement playerSpinePlayerMovement = spawnedPlayer.GetComponent<SpinePlayerMovement>();

            playerSpinePlayerMovement.enabled = true;
            playerSpinePlayerMovement.sceneCamera = Camera.main;

            Rigidbody playerRigidBody = spawnedPlayer.GetComponent<Rigidbody>();
            playerRigidBody.useGravity = true;
            spawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;
            spawnedPlayer.GetComponent<PlayerFloating>().enabled = true;

            PlayerAnimatior playerAnimator = spawnedPlayer.GetComponent<PlayerAnimatior>();

            virtualCamera.Follow = spawnedPlayer.transform;
            virtualCamera.LookAt = spawnedPlayer.transform;

            playerAnimator.SetCharacterState("Idle");

            spawnedPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;

            Jump jumpComp=spawnedPlayer.AddComponent<Jump>();
            jumpComp.rigidbody = playerRigidBody;

            OutOfBounce outOfBouncePComp= spawnedPlayer.AddComponent<OutOfBounce>();
            outOfBouncePComp.startPosition = playerSpawnPoint;
            outOfBouncePComp.manager = this;

            platformPrefab.transform.GetChild(0).GetComponent<PlatformFalling>().manager = this;


        }
        else
        {
            Debug.Log("WordFactory GM.Start(): Player Manager is null");
        }


        StartCoroutine(WaitUntillDataIsLoaded());


    }

    // Update is called once per frame
    void Update()
    {
        if (correctAnswer)
        {
            currentQuestionIndex++;
            SetNextQuestion();
           
            correctAnswer = false;
        }

    }

    /// <summary>
    /// Destroys the added Components so it it isn't used outside of pathOfdanger. 
    /// Is used on the back to MainWorld button and restart gamebuttons. 
    /// </summary>
    public void SetupPlayerToDefaultComponents()
    {
        Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<Jump>());
        Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<OutOfBounce>());
     

    }



    public void BuildPlatforms()
    {
        
       
        for (int z = 0; z < z_AmountOfPlatforms; z++)
        {
            // Random correct image index is used so the right answer is put randomly between the posible positions. 
            int correctImageIndex = Random.Range(0, x_AmountOfPlatforms);
            for (int x = 0; x < x_AmountOfPlatforms; x++)
            {

                if (x == correctImageIndex)
                {

                    gameMode.SetCorrectAnswer(questions[z], this);

                    Debug.Log(questions[z]);

                    platformPrefabFallingScript.isCorrectAnswer = true;
                }
                else
                {
                    gameMode.SetWrongAnswer(this, questions[z]);
                    platformPrefabFallingScript.isCorrectAnswer = false;
                }

                Vector3 pos = new Vector3(x*x_distanceBetweenPlatforms, 0, z*z_distanceBetweenPlatforms)+DeathPlatforms.transform.position;

                spawnedPlatforms[x,z]= Instantiate(platformPrefab, pos, Quaternion.identity);
                spawnedPlatforms[x,z].transform.parent = DeathPlatforms.transform;

                Vector3 offset = new Vector3(0, 0.6f, 0);
                GameObject imageholder = Instantiate(answerHolderPrefab,pos+offset,answerHolderPrefab.transform.rotation, spawnedPlatforms[x, z].transform);
               
            }
          
        }
        Vector3 endGoalPos = new Vector3(0 * x_distanceBetweenPlatforms, 0, z_AmountOfPlatforms * z_distanceBetweenPlatforms) + DeathPlatforms.transform.position;
        spawnedPlatforms[0, z_AmountOfPlatforms] = Instantiate(endPlane, endGoalPos, Quaternion.identity);

        spawnedPlatforms[0, z_AmountOfPlatforms].GetComponent<ShowYouWinUI>().WinUI = winUI;
        spawnedPlatforms[0, z_AmountOfPlatforms].GetComponent<ShowYouWinUI>().manager = this;




    }

    public void StartGoToLoseScreenCoroutine()
    {
        SetupPlayerToDefaultComponents();
        StartCoroutine(GoToLoseScreen());
    }

    private IEnumerator GoToLoseScreen()
    {
        yield return new WaitForSeconds(1);
        SwitchScenes.SwitchToPathOfDangerLoseScene();
        
    }

    public void DestroyAllPanels()
    {
        if (spawnedPlatforms != null)
        {
            foreach (var item in spawnedPlatforms)
            {

                Destroy(item);

            }
            spawnedPlatforms = new GameObject[x_AmountOfPlatforms,z_AmountOfPlatforms+1];

        }
    }


    /// <summary>
    /// updates the display to show the next question
    /// </summary>
    void SetNextQuestion()
    {
        if (questions.Length <= currentQuestionIndex) return;
        gameMode.GetDisplayAnswer(questions[currentQuestionIndex], this);
        Instantiate(coinPrefab);

    }

   public IEnumerator WaitUntillDataIsLoaded()
    {
        Debug.Log("Wait untilDatais loaded");
        //Both the images needds loaded before the platforms are built 
        //reinsert: || !IsSaveDataLoaded
        while (!ImageManager.IsDataLoaded)
        {
            yield return null;
        }

        currentQuestionIndex = 0;
        questions = gameMode.GenerateAnswers(3);
        z_AmountOfPlatforms = questions.Length;

        gameMode.SetAnswerPrefab(this);

        currentQuestion = questions[currentQuestionIndex];

        gameMode.GetDisplayAnswer(currentQuestion, this);



        spawnedPlatforms = new GameObject[x_AmountOfPlatforms, z_AmountOfPlatforms+1];

        BuildPlatforms();

    }



    public void SetupGame(IGenericGameMode gameMode, IGameRules gameRules)
    {
        this.gameMode = (IPODGameMode)gameMode;

      
    }


    /// <summary>
    /// Plays the sound that has been set onto the hearletter button. 
    /// </summary>
    public void PlaySoundFromHearLetterButton()
    {
        hearLetterButton.GetComponent<AudioSource>().Play();
    }
}

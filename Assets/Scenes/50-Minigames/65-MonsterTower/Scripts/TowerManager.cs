using System.Collections;
using System.Collections.Generic;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._50_Minigames._65_MonsterTower.Scrips.DataPersistence.Data;
using Scenes._50_Minigames._65_MonsterTower.Scrips.MTGameModes;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;
using System;
using Scenes._10_PlayerScene.Scripts;




namespace Scenes._50_Minigames._65_MonsterTower.Scrips
{
    
    public class TowerManager : MonoBehaviour, IDataPersistence, IMinigameSetup
    {

        public int towerHeight;

        private GameObject[,] tower;

        public int rowToDelete = 0;


        [SerializeField] GameObject brickPrefab;
        public GameObject hearLetterButton;

        public AudioSource towerAudioSource;

        public AudioSource hearLetterButtonAudioSource;

        private int towerRadius = 20;
        private int numberOfBricksInLane = 30;


        public bool correctAnswer = false;

        private Vector3 brickDimensions;

        private int amountOfOptions = 4;

        private IMTGameMode gameMode;

        private List<BrickLane> loadedBrickLanes = new List<BrickLane>();


        string currentQuestion;
        int currentQuestionIndex = 0;
        [SerializeField] public TextMeshProUGUI displayBox;
        [SerializeField] public GameObject imageHolderPrefab;
        [SerializeField] public GameObject singleImageHolderPrefab;
        public GameObject textHolderPrefab;
        [SerializeField] private GameObject coinPrefab;
        public GameObject answerHolderPrefab;
        string[] questions;
        [SerializeField] GameObject OrcPrefab;
        [SerializeField] Camera mainCamera;

        public RawImage topImage;
        public RawImage bottomImage;
        public RawImage soloImage;
        public TextMeshProUGUI textOnBrick;
       
        public string imageKey;

        private bool isSaveDataLoaded = false;
        
        private float yPosGoal;
        private bool falling = false;
        private bool waitForLastExplosionToFinish = true;
        private float goToWinScreenTimer = 0;

        void Start()
        {
            towerAudioSource = mainCamera.GetComponent<AudioSource>();
            hearLetterButtonAudioSource = hearLetterButton.GetComponent<AudioSource>();
        }
     
        



        /// <summary>
        /// if the images arent loaded, waits with setting tower data and building the tower until the images are loaded
        /// 
        /// ATTENTION!!!! WILL NOT LOAD FROM SCENE DIRECTLY. YOU NEED TO
        /// GO INTO TOWERSCENE FROM ANOTHER SCENE FIRST BECAUSE THE ASSETS
        /// NEED TO LOAD BEFORE THEY CAN BE USED
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitUntillDataIsLoaded()
        {

            //Both the images and the savedata needs to be loaded before the tower is built. 
            //reinsert: || !IsSaveDataLoaded
            while (!ImageManager.IsDataLoaded)
            {
                yield return null;
            }

            questions = gameMode.GenerateAnswers(3);
            towerHeight = questions.Length;


            gameMode.SetAnswerPrefab(this);

            brickDimensions = brickPrefab.GetComponent<MeshRenderer>().bounds.size;

            currentQuestion = questions[currentQuestionIndex];

            if (hearLetterButton.activeSelf == false)
            {
                displayBox.text = currentQuestion;
            }
            else
            {
                gameMode.GetDisplayAnswer(currentQuestion, this);
            }

            rowToDelete = 0;
            BuildTower();


        }

        /// <summary>
        /// starts the coroutine which sets up data for the tower once files have been loaded (used to set the data manually, but that is currently not possible as it now requires the ImageManager to have loaded data)
        /// </summary>
        public void SetupGame(IGenericGameMode genericGameMode, IGameRules gameRules)
        {
            gameMode = (IMTGameMode)genericGameMode;


            StartCoroutine(WaitUntillDataIsLoaded());
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


        /// <summary>
        /// Removes the PlayerMovement_MT component from the player characther. 
        /// </summary>
        public void SetupPlayerMovementToDefault()
        {
            Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerMovement_MT>());
        }



        // Update is called once per frame
        // The towerchecks if the right answer has been chosen and destroys the lowest tower lane. 
        void Update()
        {

            if (correctAnswer)
            {
                currentQuestionIndex++;
                SetNextQuestion();
                DestroyLowestTowerLane();
                correctAnswer = false;
            }

            if(falling)
            {
               
                TowerFallsAnimation();
            }

        }

        // the lowest tower lane is destroyed by knowing the numberOfBricksInLane and the accessing the 2d tower array that have all the bricks.
        // Lastly the whole tower is lowered the same amount as the height of a brick. 
        void DestroyLowestTowerLane()
        {

            if (rowToDelete >= towerHeight)
                return;

            for (int i = 0; i < numberOfBricksInLane; i++)
            {

                Destroy(tower[i, rowToDelete]);



            }

            //Adds gold and Xp To the player for destroying a lane of the tower. 

            PlayerEvents.RaiseGoldChanged(1);
            PlayerEvents.RaiseXPChanged(1);


            // list holding data on the lanes is also updated so the lowest lane is removed from the save data. 
            loadedBrickLanes.RemoveAt(0);



            rowToDelete++;


            // Checks if the next row to delete is outside of the zeroindexed 2d tower array. 
            if (rowToDelete != towerHeight)
            {

                // sets the next rows pictures active and shows them. 
                for (int i = 0; i < numberOfBricksInLane; i++)
                {
                    if (i <= amountOfOptions - 1)
                    {
                        tower[i, rowToDelete].transform.GetChild(0).gameObject.SetActive(true);
                        Brick brickComponent = tower[i, rowToDelete].GetComponent<Brick>();
                        brickComponent.isShootable = true;

                    }
                }
                
                // Falling animation starts and the new position that the tower neeeds to fall to is set. 
                falling = true;
                yPosGoal = gameObject.transform.position.y - brickDimensions.y;



            }
            else
            {
                //Goes to the win screen if there are no more bricks of the tower left. 
                SetupPlayerMovementToDefault();
                StartCoroutine(GoToWinScreen());
            }

            //Shaking the camera
            ShakeCamera();


            PlayAudioExplosion();

            // Gets a random audiocclip from the congratsAudioManager and plays it so the player is praised. 
           StartCoroutine( PlayAudioPraise());

           
            

            // zoom out when when a tower lane is destroyed
            mainCamera.GetComponent<ToggleZoom>().ZoomOutWhenTowerLaneIsDestroyed();

        }

        /// <summary>
        /// Shakes the main camera 
        /// </summary>
        void ShakeCamera()
        {
            StartCoroutine(mainCamera.GetComponent<CameraShake>().Shake(.30f, .4f));
        }
       

        
     

        /// <summary>
        /// Animation for the tower falling based on the yPosGoal which is the y position the tower needs to fall to. 
        /// falling is set to false when the tower has reached its yPosGoal. 
        /// </summary>
        void TowerFallsAnimation()
        {

                gameObject.transform.Translate(0, -0.1f, 0);
               
            
                
                if(gameObject.transform.position.y<=yPosGoal)
                {
                    falling = false;
                    
                }
            
           
        }


        /// <summary>
        ///  Gets a random audiocclip from the congratsAudioManager and plays it so the player is praised. 
        ///  Is set To Danish as default but can be changed if needed. 
        /// </summary>
        IEnumerator PlayAudioPraise()
        {
            while (towerAudioSource.isPlaying)
            {
                yield return null;
            }

            int rndIndex = UnityEngine.Random.Range(0, CongratsAudioManager.GetLenghtOfAudioClipDanishList());

            AudioClip CongratsAudio = CongratsAudioManager.GetAudioClipFromDanishSet(rndIndex);
            towerAudioSource.clip = CongratsAudio;

            towerAudioSource.Play();
        }

        /// <summary>
        /// 
        ///  Gets a explosion audioclip from the soundFXManager and plays it.
        /// </summary>
        void PlayAudioExplosion()
        {
            AudioClip explosionAudio = SoundFXManager.GetAudioClipFromExplosionsList(0);
            towerAudioSource.clip = explosionAudio;
            towerAudioSource.Play();
        }


        /// <summary>
        /// Loads the win screen sceene
        /// </summary>
         IEnumerator GoToWinScreen()
        { 
            yield return new WaitForSeconds(4);

            // saving the game so the fact that there are no lanes left is saved .
            //that will have the effect that the next time the monstertower scene is loaded a new tower is built because there are no lanes saved. 

            // DataPersistenceManager.instance.SaveGame();
           


            SceneManager.LoadScene("WinScene");
        }


        /// <summary>
        /// Method for building the tower based on numberOfBricksInLane, a list of brick data,towerHeight and radius. 
        /// The bricks are put into a 2D array with the parameteres x and z.
        /// x is the id of the brick in the x-axis. 
        /// z is the id of the height/lane the brick is in.  
        /// 
        /// </summary>
        /// 
        void BuildTower()
        {

            //The tower angle is a value that represents the angle between the bricks and center of the tower. 
            //The start angle is an angle i chose based on where i want the tower to start build. 
            // The reason for this is so the first bricks in the 2d tower array is the ones used for displaying the pictures.

            float towerAngle = 2 * Mathf.PI / numberOfBricksInLane;
            float startAngle = 180.3f;
            tower = new GameObject[numberOfBricksInLane, towerHeight];
            loadedBrickLanes = new List<BrickLane>();

            // the tower is built based on tower height and numberOfBricksInLane with a for loop. 
            for (int z = 0; z < towerHeight; z++)
            {

                // Random correct image index is used so the right answer is put randomly between the posible positions. 
                int correctImageIndex = UnityEngine.Random.Range(0, amountOfOptions - 1);

                // added lane to loadedBrickLanes with the correct image index which can be used next time the game is loaded
                // to place the image in the correct position.
                loadedBrickLanes.Add(new BrickLane(correctImageIndex));


                for (int x = 0; x < numberOfBricksInLane; x++)
                {
                    //the new position of each brick is calculated based on angle,tower radius and the dimension of the brick. 
                    //Calculating x and z with x=cos(v)*r and z=sin(v)*r
                    // y is calculated based on the y dimension of the brick so the next lane is directly on top of the previus one.  
                    float posX = Mathf.Cos(startAngle) * towerRadius;
                    float posY = Mathf.Sin(startAngle) * towerRadius;
                    Vector3 newPos = gameObject.transform.position + new Vector3(posX, z * brickDimensions.y, posY);

                    // brick is the instantiated and the angle set as startangle. 
                    // The brick is put into the 2d tower array. 
                    // the brick is rotated so the picture would be facing the right way.
                    // Then the towerobject is set as the parrent to the brick. 

                    tower[x, z] = Instantiate(brickPrefab, newPos, quaternion.Euler(0, -startAngle, 0));
                    tower[x, z].transform.Rotate(new Vector3(0, -90, 0));
                    tower[x, z].transform.parent = gameObject.transform;

                    // The amount of options is a value that can be set based on difficulty if more potenial options is needed.
                    if (x <= amountOfOptions - 1)
                    {
                        Brick brickComponent = tower[x, z].GetComponent<Brick>();
                        // The images are set here and instantiatetd on the right bricks. 
                        // and based on the value of correctImageIndex the right answer is set. 
                        if (x == correctImageIndex)
                        {
                            gameMode.SetCorrectAnswer(questions[z], this);

                            //setting up bricks for the particular lane.
                            // in this case im adding a new BrickData with the particule sentences that is coresponding to the brick. 
                            loadedBrickLanes[z].bricks.Add(new BrickData(questions[z]));
                            loadedBrickLanes[z].correctImageIndex = correctImageIndex;
                            brickComponent.isCorrect = true;

                        }
                        else
                        {
                            gameMode.SetWrongAnswer(this);

                            // the sentence for the random brick is also inputtet into the data on the particular lane. 
                            // the top and bottom image key is defined in the SetRandomImage
                            loadedBrickLanes[z].bricks.Add(new BrickData(imageKey));
                        }


                        GameObject imageholder = Instantiate(answerHolderPrefab, tower[x, z].transform);
                        imageholder.GetComponent<RectTransform>().localPosition = new(0, 1.58f, -1.4f);
                        if (z == 0)
                        {
                      
                            brickComponent.isShootable = true;
                            imageholder.SetActive(true);
                        }

                        //Spawns the Monster on top of the tower.
                        if(z==towerHeight-1 && x==2)
                        {
                            Vector3 orcPos = gameObject.transform.position + new Vector3(posX, z * brickDimensions.y+1.5f, posY);
                            var orc =Instantiate(OrcPrefab, orcPos, quaternion.Euler(0, startAngle-90, 0));


                            orc.transform.parent = gameObject.transform;
                        }
                        

                    }
                    // startAngle is updated so the next brick gets placed further along the circle.

                    startAngle += towerAngle;

                }
            }

        }





        /// <summary>
        /// The tower is loaded based on the loadedBrickLanes list which contains data on the bricklanes in the tower 
        /// and the pictures that needs to be loaded on to the bricks.
        /// 
        /// 
        /// </summary>


        void LoadTower()
        {

            //The tower angle is a value that represents the angle between the bricks and center of the tower. 
            //The start angle is an angle i chose based on where i want the tower to start build. 
            // The reason for this is so the first bricks in the 2d tower array is the ones used for displaying the pictures.

            // Tower height is set to the amount of lanes in the loadedBrickLanes list. 
            towerHeight = loadedBrickLanes.Count;

            float towerAngle = 2 * Mathf.PI / numberOfBricksInLane;
            float startAngle = 180.3f;
            tower = new GameObject[numberOfBricksInLane, towerHeight];

            // the tower is built based on tower height and numberOfBricksInLane with a for loop. 
            for (int z = 0; z < towerHeight; z++)
            {


                for (int x = 0; x < numberOfBricksInLane; x++)
                {
                    //the new position of each brick is calculated based on angle,tower radius and the dimension of the brick. 
                    //Calculating x and z with x=cos(v)*r and z=sin(v)*r
                    // y is calculated based on the y dimension of the brick so the next lane is directly on top of the previus one.  
                    float posX = Mathf.Cos(startAngle) * towerRadius;
                    float posY = Mathf.Sin(startAngle) * towerRadius;
                    Vector3 newPos = gameObject.transform.position + new Vector3(posX, z * brickDimensions.y, posY);

                    // brick is the instantiated and the angle set as startangle. 
                    // The brick is put into the 2d tower array. 
                    // the brick is rotated so the picture would be facing the right way.
                    // Then the towerobject is set as the parrent to the brick. 

                    tower[x, z] = Instantiate(brickPrefab, newPos, quaternion.Euler(0, -startAngle, 0));
                    tower[x, z].transform.Rotate(new Vector3(0, -90, 0));
                    tower[x, z].transform.parent = gameObject.transform;

                    // The amount of options is a value that can be set based on difficulty if more potenial options is needed.
                    if (x <= amountOfOptions - 1)
                    {
                        Brick brickComponent = tower[x, z].GetComponent<Brick>();
                        // The images are set here and instantiatetd on the right bricks. 
                        // and based on the value of correctImageIndex the right answer is set. 
                        if (x == loadedBrickLanes[z].correctImageIndex)
                        {
                            // the SetCorrectImage method is used in conjunction with the sentence that is made in the tower builder for the specific brick. 

                            SetCorrectImage(loadedBrickLanes[z].bricks[x].input);

                            brickComponent.isCorrect = true;
                        }
                        else
                        {
                            //The SetCorrectImage is also used when the answer is wrong because the wrong answers also neeed to be drawn based on a sentence corresponding to the brick. 
                            SetCorrectImage(loadedBrickLanes[z].bricks[x].input);
                        }

                        GameObject imageholder = Instantiate(answerHolderPrefab, tower[x, z].transform);
                        imageholder.GetComponent<RectTransform>().localPosition = new(0, 0, -1.4f);
                        if (z == 0)
                        {
                            brickComponent.isShootable = true;
                            imageholder.SetActive(true);
                        }

                        //Spawns the Monster on top of the tower.
                        if (z == towerHeight - 1 && x == 2)
                        {
                            Vector3 orcPos = gameObject.transform.position + new Vector3(posX, z * brickDimensions.y + 1.5f, posY);
                            var orc = Instantiate(OrcPrefab, orcPos, quaternion.Euler(0, startAngle - 90, 0));


                            orc.transform.parent = gameObject.transform;
                        }

                    }
                    // startAngle is updated so the next brick gets placed further along the circle.

                    startAngle += towerAngle;

                }
            }

        }



        /// <summary>
        /// sets the image up to match the sentens given
        /// </summary>
        /// <param name="sent">the sentens that the images is matching</param>
        void SetCorrectImage(string sent)
        {
            gameMode.SetCorrectAnswer(sent, this);
        }


       

        // The LoadData method is used when starting up the game
        // the bricklanes that are saved is loaded in and set. 
        // The currentQuestionIndex which has been saved is set so the right question can be displayed. 

        public void LoadData(GameData data)
        {
            if (data.BrickLanes != null)
            {
                this.loadedBrickLanes = data.BrickLanes;

                // i set it to minus 1 because the next question method is always setting the currentQuistionIndex to the next question
                // and not the actual current at the time of exiting the game
                this.currentQuestionIndex = data.currentQuestionIndex;

                this.questions = data.questions;
                //Debug.Log(data.BrickLanes.Count);


                // If loading in a save the sentence displayed is updated. 
                // and if there is no lanes in the save then the currentQuestionIndex is set to 0 so next time 
                // the application starts the quistions starts over from the beginning. 
                if (loadedBrickLanes.Count > 0)
                {
                    if (questions!=null)
                    {
                        //Debug.Log("Loaded bricklanes");
                        currentQuestion = questions[currentQuestionIndex];
                        displayBox.text = currentQuestion;
                    }
                    
                }
                else
                {
                    currentQuestionIndex = 0;
                }

                isSaveDataLoaded = true;

                Debug.Log("Data Loaded");
            }

        }



        // The SaveData method is used when exiting the game.
        // The bricklanes are saved and the currentQuestionIndex is saved. 

        public void SaveData(ref GameData data)
        {
            data.BrickLanes = this.loadedBrickLanes;


            data.currentQuestionIndex = currentQuestionIndex;

            data.questions = questions;

            Debug.Log("Data Saved");

        }
    }
}

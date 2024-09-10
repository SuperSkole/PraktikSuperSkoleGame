using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._10_PlayerScene.Scripts;
using Scenes._50_Minigames._65_MonsterTower.Scripts;
using Scenes._50_Minigames._65_MonsterTower.Scrips;
using Spine.Unity;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Scenes._50_Minigames._65_MonsterTower.Scripts
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,


    }


    public class MonsterTowerManager : MonoBehaviour
    {

        public int ammoCount;

        [SerializeField] Camera mainCamera;
        [SerializeField] Camera cameraBrain;
        public LayerMask AmmoLayermask;
         [SerializeField] LayerMask TowerLayermask;
        public SpinePlayerMovement mainWorldMovement;

        [SerializeField] GameObject hearLetterButton;

        [SerializeField] GameObject noAmmoText;
        public GameObject[,] ammoDisplay;
        [SerializeField] GameObject ammoToDisplayPrefab;
        [SerializeField] GameObject ammoPlatform;
        public CatapultAming catapultAming;
        [SerializeField] GameObject playerSpawnPoint;
        [SerializeField] GameObject startPoint;

        [SerializeField] AnimationReferenceAsset idle;
        [SerializeField] AnimationReferenceAsset walk;
        [SerializeField] ParticleSystem pointAndClickEffect;

        [SerializeField] GameObject dropOffPoint;
        RaycastHit hit;
        Ray ray;
        [SerializeField] AmmoPupUp pupUp;

        public TowerManager towerManager;

        public bool ammoLoaded = false;

        public Difficulty difficulty;

        Vector3 ammoDimensions; 

        //temp
        public List<string> words;

        public List<Vector3> ammoSpawnPoints=new List<Vector3>();

        private GameObject spawnedPlayer;
       

        /// <summary>
        /// Gets the words collected by the player from the playerManager so it can be used to display the ammunition and the amount of ammo the player has. 
        /// </summary>
        void SetupPlayerWords()
        {
            words = PlayerEvents.RaisePlayerDataWordsExtracted();
           
           

        }





        void Start()
        {

           // getting and setting the ammodimensions from the prefab 
            ammoDimensions=ammoToDisplayPrefab.GetComponent<MeshRenderer>().bounds.size;
            // setting up the main camera so it reflects the chosen difficulty. 
            mainCamera.GetComponent<ToggleZoom>().difficulty = difficulty;


            //Saving the ammoSpawnPoints positions that are children to the ammoPlatform in a list. 
            for (int i = 0; i < ammoPlatform.transform.childCount; i++)
            {

                ammoSpawnPoints.Add(ammoPlatform.transform.GetChild(0).transform.position);

            }

           
            
            //Gets the words the playermanager has gotten and copies it to a list of strings named words. 
            SetupPlayerWords();
            
            

            //Spawns the ammunition with with the words in the words list and is displayed beside the catapult. 
            // its set up in 4 points and then the ammo boxes is stacked on eachother. 
            SpawnAmmoForDisplay();

            // the ammocount is set to the amount of words that player has. 
            if (words != null)
            {
                ammoCount = words.Count;
            }

            
           
            if (ammoCount <= 0)
            {
                noAmmoText.SetActive(true);
            
                return;
            }




            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.PositionPlayerAt(playerSpawnPoint);
                spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
                spawnedPlayer.AddComponent<AutoMovePlayer>();
                spawnedPlayer.GetComponent<Rigidbody>().useGravity = false;
                spawnedPlayer.GetComponent<PlayerFloating>().enabled = false;
                spawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = false;
                spawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;
                spawnedPlayer.GetComponent<AutoMovePlayer>().DropOffPoint = dropOffPoint;
                spawnedPlayer.GetComponent<AutoMovePlayer>().PlayerSpawnPoint = startPoint;
                spawnedPlayer.GetComponent<AutoMovePlayer>().monsterTowerManager = this;
                spawnedPlayer.GetComponent<PlayerAnimatior>().SetCharacterState("Idle");

                SetupPlayerMovementForMonsterTower();
            }
            else
            {
                Debug.Log("WordFactory GM.Start(): Player Manager is null");
            }



        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                CheckWhatWasClickedOn();
        }

        /// <summary>
        /// Destroys the added PlayerMovement_MT component so it it isn't used outside of monsterTower. 
        /// Is used on the back to MainWorld button. 
        /// </summary>
        public void SetupPlayerMovementToDefault()
        {
            Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerMovement_MT>());
            Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<AutoMovePlayer>());
        }


        /// <summary>
        /// Adds the playermovement component that is used specifically  in monstertower to the player character . 
        /// Sets up the camera so it works with the players movements and the Player characthers StartPosition is set. 
        /// </summary>
        /// 
        private void SetupPlayerMovementForMonsterTower()
        {
            //spawnedPlayer.GetComponent<PlayerFloating>().enabled = false;
            spawnedPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;          
            spawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;



            PlayerMovement_MT pMovement = spawnedPlayer.AddComponent<PlayerMovement_MT>();
            pMovement.idle = idle;
            pMovement.walk = walk;
            pMovement.pointAndClickEffect = pointAndClickEffect;
            pMovement.sceneCamera = mainCamera;
            pMovement.placementLayermask = AmmoLayermask;
            pMovement.skeletonAnimation = spawnedPlayer.transform.GetChild(0).GetComponent<SkeletonAnimation>();
            pMovement.monsterTowerManager = this;
            spawnedPlayer.SetActive(true);
            spawnedPlayer.transform.position = playerSpawnPoint.transform.position;
            

            CinemachineVirtualCamera virtualCamera = mainCamera.GetComponent<CinemachineVirtualCamera>();
            virtualCamera.Follow = spawnedPlayer.transform;
            virtualCamera.LookAt = spawnedPlayer.transform;
        }





        /// <summary>
        /// Spawns the ammo with each block representing a word that the player has picked up from the other minigames. 
        /// </summary>
        void SpawnAmmoForDisplay()
        {
            //Calculating the size of the 2 dimensional array holding the ammoboxes based on the amount of words given. 
            if (words != null)
            {
                //dividing the amount of words with 4 because i have 4 spawnpoints . 
                // Then making sure its rounded up all the time. 
                int amountOfSpawnPoints = ammoPlatform.transform.childCount;
                int wordsMaxHeightIndex = (int)Math.Ceiling((double)words.Count / (double)amountOfSpawnPoints);

                ammoDisplay = new GameObject[amountOfSpawnPoints, wordsMaxHeightIndex];
            }


            int spawnIndex = 0;

            int spawnHeightIndex=0;

            int amountOfSpawnPositions = ammoPlatform.transform.childCount;

            
            //Putting the boxes in the right position and saving the box in ammoDisplay 2D array with a x,y position.
            // Also puts the words on the list of words on the box itself. 
            //Also setting the name of the box to the position it has and setting a tag on it which can be used to do collision detection. 

            if (words!=null)
            {
               
                for (int x = 0; x < words.Count; x++)
                {


                    for (int i = 0; i < ammoToDisplayPrefab.transform.childCount; i++)
                    {
                        ammoToDisplayPrefab.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = words[x];

                    }

                    if (spawnIndex >= amountOfSpawnPositions)
                    {
                        spawnIndex = 0;
                        spawnHeightIndex++;
                    }

                 

                    ammoToDisplayPrefab.tag = "ammo";

                    

                    Vector3 spawnPos = ammoPlatform.transform.GetChild(spawnIndex).transform.position + new Vector3(0, ammoDimensions.y * spawnHeightIndex);


                    
                    GameObject ammo = Instantiate(ammoToDisplayPrefab, spawnPos, Quaternion.identity);
                   

                    ammo.name = spawnIndex + "," + spawnHeightIndex;
                    ammoDisplay[spawnIndex,spawnHeightIndex]= ammo;


                    spawnIndex++;
                }
            }
          
        }


        /// <summary>
        /// sends out a ray to detect what was clicked on.
        /// also handels the check if you cliced on the right image/stone
        /// </summary>
        void CheckWhatWasClickedOn()
        {
            if(ammoCount <= 0) return;

            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);
            if (hit.transform == null) return;

            Brick comp = hit.transform.gameObject.GetComponent<Brick>();
          
         
            
            if (comp == null || comp.isShootable == false) return;
            StartCoroutine(catapultAming.Shoot(hit.point, comp, this));
        }


        /// <summary>
        /// Plays the sound that has been set onto the hearletter button. 
        /// </summary>
        public void PlaySoundFromHearLetterButton()
        {
            hearLetterButton.GetComponent<AudioSource>().Play();
        }


        /// <summary>
        /// removes ammoCount and updates the wordlist in playerdata and locally for the words List. 
        /// </summary>
        public void RemoveAmmo()
        {
            
            PlayerEvents.RaiseWordRemovedValidated((words[ammoCount-1]));
            words.RemoveAt(ammoCount-1);
         
            ammoCount--;


           
        }

        /// <summary>
        /// shows the tooltip for the amount of ammoCount the player has
        /// </summary>
        private void OnMouseEnter()
        {
            pupUp.SetAndShowToolTip(ammoCount.ToString());
        }

        /// <summary>
        /// hide the tooltip
        /// </summary>
        private void OnMouseExit()
        {
            pupUp.HideToolTip();
        }
    }

}
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
using System.Collections;
using System.ComponentModel;
using UnityEditor;
using LoadSave;

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
        public List<string> wordsOrLetters;

        public List<Vector3> ammoSpawnPoints=new List<Vector3>();

        private GameObject spawnedPlayer;

        public AudioSource flyingProjectileSound;

        private AudioSource hearletterButtonAudioSource;

        [SerializeField] AudioSource towerAudioSource;
        private List<char> letters;

        /// <summary>
        /// Gets the wordsOrLetters collected by the player from the playerManager so it can be used to display the ammunition and the amount of ammo the player has. 
        /// </summary>
        void SetupPlayerWords()
        {

            if (PlayerManager.Instance.PlayerData.CollectedLetters.Count>0)
            {
                letters = PlayerEvents.RaisePlayerDataLettersExtracted();

                foreach (var item in letters)
                {
                    wordsOrLetters.Add(item.ToString());
                }
            }
            else
            {
                wordsOrLetters = PlayerEvents.RaisePlayerDataWordsExtracted();
            }
           
           
           

        }





        void Start()
        {
            //Setting the audio source on the hearletterbutton.
            hearletterButtonAudioSource = hearLetterButton.GetComponent<AudioSource>();


            // getting and setting the ammodimensions from the prefab 
            ammoDimensions =ammoToDisplayPrefab.GetComponent<MeshRenderer>().bounds.size;
            // setting up the main camera so it reflects the chosen difficulty. 
            mainCamera.GetComponent<ToggleZoom>().difficulty = difficulty;


            //Saving the ammoSpawnPoints positions that are children to the ammoPlatform in a list. 
            for (int i = 0; i < ammoPlatform.transform.childCount; i++)
            {

                ammoSpawnPoints.Add(ammoPlatform.transform.GetChild(0).transform.position);

            }

           
            
            //Gets the wordsOrLetters the playermanager has gotten and copies it to a list of strings named wordsOrLetters. 
            SetupPlayerWords();
            
            

            //Spawns the ammunition with with the wordsOrLetters in the wordsOrLetters list and is displayed beside the catapult. 
            // its set up in 4 points and then the ammo boxes is stacked on eachother. 
            SpawnAmmoForDisplay();

           
            
           
            if (ammoCount <= 0)
            {
                noAmmoText.SetActive(true);
            
                return;
            }




            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.PositionPlayerAt(playerSpawnPoint);
                spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
                spawnedPlayer.AddComponent<AutoMovePlayer_MT>();
                spawnedPlayer.GetComponent<Rigidbody>().useGravity = false;
                spawnedPlayer.GetComponent<PlayerFloating>().enabled = false;
                spawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = false;
                spawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;
                spawnedPlayer.GetComponent<AutoMovePlayer_MT>().DropOffPoint = dropOffPoint;
                spawnedPlayer.GetComponent<AutoMovePlayer_MT>().PlayerSpawnPoint = startPoint;
                spawnedPlayer.GetComponent<AutoMovePlayer_MT>().monsterTowerManager = this;
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
            Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<AutoMovePlayer_MT>());
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
            //Calculating the size of the 2 dimensional array holding the ammoboxes based on the amount of wordsOrLetters given. 
            if (wordsOrLetters != null)
            {
                //dividing the amount of wordsOrLetters with 4 because i have 4 spawnpoints . 
                // Then making sure its rounded up all the time. 
                int amountOfSpawnPoints = ammoPlatform.transform.childCount;
                int wordsMaxHeightIndex = (int)Math.Ceiling((double)wordsOrLetters.Count / (double)amountOfSpawnPoints);

                ammoDisplay = new GameObject[amountOfSpawnPoints, wordsMaxHeightIndex];
            }


            int spawnIndex = 0;

            int spawnHeightIndex=0;

            int amountOfSpawnPositions = ammoPlatform.transform.childCount;

            
            //Putting the boxes in the right position and saving the box in ammoDisplay 2D array with a x,y position.
            // Also puts the wordsOrLetters on the list of wordsOrLetters on the box itself. 
            //Also setting the name of the box to the position it has and setting a tag on it which can be used to do collision detection. 

            if (wordsOrLetters!=null)
            {
               
                for (int x = 0; x < wordsOrLetters.Count; x++)
                {


                    for (int i = 0; i < ammoToDisplayPrefab.transform.childCount; i++)
                    {
                        ammoToDisplayPrefab.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = wordsOrLetters[x];

                    }

                    if (spawnIndex >= amountOfSpawnPositions)
                    {
                        spawnIndex = 0;
                        spawnHeightIndex++;
                    }

                 

                    //ammoToDisplayPrefab.tag = "ammo";

                    

                    Vector3 spawnPos = ammoPlatform.transform.GetChild(spawnIndex).transform.position + new Vector3(0, ammoDimensions.y * spawnHeightIndex);


                    
                    GameObject ammo = Instantiate(ammoToDisplayPrefab, spawnPos, Quaternion.identity);
                   

                    ammo.name = spawnIndex + "," + spawnHeightIndex;
                    ammoDisplay[spawnIndex,spawnHeightIndex]= ammo;


                    spawnIndex++;
                }

                // the ammocount is set to the amount of wordsOrLetters that player has. 
                if (wordsOrLetters != null)
                {
                    ammoCount = wordsOrLetters.Count;
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

           StartCoroutine(PlaySoundfromHearLetterButtonOrWait());
        }


        /// <summary>
        /// Waits and checks if the toweraudio source that play letters/words audio from the button is done playing. 
        /// </summary>
        /// <returns></returns>
        IEnumerator PlaySoundfromHearLetterButtonOrWait()
        {
            while (towerAudioSource.isPlaying)
            {
                yield return null;
            }

            hearletterButtonAudioSource.Play();
        }


        /// <summary>
        /// removes ammoCount and updates the wordlist in playerdata and locally for the wordsOrLetters List. 
        /// </summary>
        public void RemoveAmmo()
        {
            
            PlayerEvents.RaiseWordRemovedValidated((wordsOrLetters[ammoCount-1]));
            wordsOrLetters.RemoveAt(ammoCount-1);
         
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
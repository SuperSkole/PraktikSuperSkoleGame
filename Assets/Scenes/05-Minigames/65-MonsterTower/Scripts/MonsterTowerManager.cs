using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using CORE.Scripts;
using CORE.Scripts.GameRules;
using Scenes._10_PlayerScene.Scripts;
using Scenes.Minigames.MonsterTower.Scrips.DataPersistence;
using Scenes.Minigames.MonsterTower.Scrips.MTGameModes;
using Spine.Unity;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Minigames.MonsterTower.Scrips
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
        public LayerMask placementLayermask;
        public SpinePlayerMovement mainWorldMovement;

        [SerializeField] GameObject noAmmoText;
        public GameObject[,] ammoDisplay;
        [SerializeField] GameObject ammoToDisplayPrefab;
        [SerializeField] GameObject ammoPlatform;
        [SerializeField] CatapultAming catapultAming;
        [SerializeField] GameObject playerSpawnPosition;

        [SerializeField] AnimationReferenceAsset idle;
        [SerializeField] AnimationReferenceAsset walk;
        [SerializeField] ParticleSystem pointAndClickEffect;
        RaycastHit hit;
        Ray ray;
        [SerializeField] AmmoPupUp pupUp;

        [SerializeField] TowerManager towerManager;

        

        public Difficulty difficulty;

        Vector3 ammoDimensions; 

        //temp
        public List<string> words;

        public List<Vector3> ammoSpawnPoints=new List<Vector3>();

        private GameObject spawnedPlayer;
        /// <summary>
        /// used to setup sentanses TEMP make this better!!
        /// </summary>
        void SetupSentanses()
        {
            words = new List<string>()
            {
                "is på ko",
                "ko på is",
                "gås under ko"
            };
           
        }
        /// <summary>
        /// Gets the words collected by the player from the playerManager so it can be used to display the ammunition and the amount of ammo the player has. 
        /// </summary>
        void SetupPlayerWords()
        {
            words = PlayerEvents.RaisePlayerDataWordsExtracted();
           
           
           
        

        }

        /// <summary>
        /// call this to setup the minigame
        /// </summary>
        /// <param name="input">the dictionary that contains all the questions and images</param>




        void Start()
        {

           
            ammoDimensions=ammoToDisplayPrefab.GetComponent<MeshRenderer>().bounds.size;
            // setting up the main camera so it reflects the chosen difficulty. 
            mainCamera.GetComponent<ToggleZoom>().difficulty = difficulty;


            for (int i = 0; i < ammoPlatform.transform.childCount; i++)
            {

                ammoSpawnPoints.Add(ammoPlatform.transform.GetChild(0).transform.position);

            }

           
            
            //Gets the words the playermanager has gotten and copies it to a list of strings named words. 
            SetupPlayerWords();
            
            

            //Spawns the ammunition that needed to be displayed beside the catapult. 
            SpawnAmmoForDisplay();

            // the ammocount is set to the amount of words that player has. 
            if (words != null)
            {
                ammoCount = words.Count;
            }

            SetupPlayerMovementForMonsterTower();
            
           
            if (ammoCount <= 0)
            {
                noAmmoText.SetActive(true);
            
                return;
            }

           
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                CheckWhatWasClickedOn();
        }


        public void SetPlayerMovementToDefault()
        {
            
            Destroy(spawnedPlayer.GetComponent<PlayerMovement_MT>());
        

        }


        private void SetupPlayerMovementForMonsterTower()
        {


            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;

          
            spawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;


            PlayerMovement_MT pMovement = spawnedPlayer.AddComponent<PlayerMovement_MT>();
            pMovement.idle = idle;
            pMovement.walk = walk;
            pMovement.pointAndClickEffect = pointAndClickEffect;
            pMovement.sceneCamera = mainCamera;
            pMovement.placementLayermask = placementLayermask;
            pMovement.skeletonAnimation = spawnedPlayer.transform.GetChild(0).GetComponent<SkeletonAnimation>();
            spawnedPlayer.SetActive(true);
            spawnedPlayer.transform.position = playerSpawnPosition.transform.position;

            CinemachineVirtualCamera virtualCamera = mainCamera.GetComponent<CinemachineVirtualCamera>();
            virtualCamera.Follow = spawnedPlayer.transform;
            virtualCamera.LookAt = spawnedPlayer.transform;
        }



        /// <summary>
        /// Spawns the ammo with each block representing a word that the player has picked up from the other minigames. 
        /// </summary>
        void SpawnAmmoForDisplay()
        {
            if (words != null)
            {
                //dividing the amount of words with 4 because i have 4 spawnpoints. 
                // Then making sure its rounded up all the time. 
                int wordsMaxHeightIndex = (int)Math.Ceiling((double)words.Count / (double)4);

                ammoDisplay = new GameObject[4, wordsMaxHeightIndex];
            }


            int spawnIndex = 0;

            int spawnHeightIndex=0;

            int amountOfSpawnPositions = ammoPlatform.transform.childCount;

            BoxCollider ammoCollider = ammoToDisplayPrefab.GetComponent<BoxCollider>();


            if (words!=null)
            {
                Debug.Log(words.Count);
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
                    ammo.transform.parent = ammoPlatform.transform;

                    ammo.name = spawnIndex + "," + spawnHeightIndex;
                    ammoDisplay[spawnIndex,spawnHeightIndex]= ammo;


                    spawnIndex++;
                }
            }
          
        }


        /// <summary>
        /// sends out a ray to detect what was cliced on.
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
        /// removes ammoCount and updates the pile of ammoCount
        /// </summary>
        public void RemoveAmmo()
        {
            
            PlayerEvents.RaiseWordRemovedValidated((words[ammoCount-1]));
            words.RemoveAt(ammoCount-1);
         
            ammoCount--;

          
            //if(ammoCount < ammoDisplay.Length)
            //    ammoDisplay[ammoCount].SetActive(false);
            //if (ammoCount <= 0)
            //    noAmmoText.SetActive(true);
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
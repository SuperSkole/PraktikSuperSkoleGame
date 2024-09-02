using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CORE.Scripts;
using CORE.Scripts.GameRules;
using Scenes._10_PlayerScene.Scripts;
using Scenes.Minigames.MonsterTower.Scrips.DataPersistence;
using Scenes.Minigames.MonsterTower.Scrips.MTGameModes;

using TMPro;
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
        [SerializeField] GameObject noAmmoText;
        [SerializeField] List<GameObject> ammoDisplay;
        [SerializeField] GameObject ammoToDisplayPrefab;
        [SerializeField] GameObject ammoPlatform;
        [SerializeField] CatapultAming catapultAming;
        [SerializeField] GameObject playerSpawnPosition;
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

            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
            spawnedPlayer.SetActive(true);
            spawnedPlayer.transform.position = playerSpawnPosition.transform.position;

           
            if (ammoCount <= 0)
            {
                noAmmoText.SetActive(true);
                for (int i = 0; i < ammoDisplay.Count; i++)
                {
                    ammoDisplay[i].SetActive(false);
                }
                return;
            }

            if (ammoCount < ammoDisplay.Count)
            {
                for (int i = ammoDisplay.Count - 1; i >= ammoCount; i--)
                {
                    ammoDisplay[i].SetActive(false);
                }
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                CheckWhatWasClickedOn();
        }


        /// <summary>
        /// Spawns the ammo with each block representing a word that the player has picked up from the other minigames. 
        /// </summary>
        void SpawnAmmoForDisplay()
        {
           

            int spawnIndex = 0;

            int spawnHeightIndex=0;

            int amountOfSpawnPositions = ammoPlatform.transform.childCount;



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

                    Vector3 spawnPos = ammoPlatform.transform.GetChild(spawnIndex).transform.position + new Vector3(0, ammoDimensions.y * spawnHeightIndex);

                  

                    GameObject ammo = Instantiate(ammoToDisplayPrefab, spawnPos, Quaternion.identity);
                    ammo.transform.parent = ammoPlatform.transform;
                    ammoDisplay.Add(ammo);


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

          
            if(ammoCount < ammoDisplay.Count)
                ammoDisplay[ammoCount].SetActive(false);
            if (ammoCount <= 0)
                noAmmoText.SetActive(true);
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
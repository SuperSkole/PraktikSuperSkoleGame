using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CORE.Scripts;
using CORE.Scripts.GameRules;
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

        int ammo = 10;

        [SerializeField] Camera mainCamera;
        [SerializeField] GameObject noAmmoText;
        [SerializeField] GameObject[] ammoDisplay;
        [SerializeField] CatapultAming catapultAming;
        RaycastHit hit;
        Ray ray;
        [SerializeField] AmmoPupUp pupUp;

        [SerializeField] TowerManager towerManager;

        public Difficulty difficulty;

        //temp
        string[] sentanses;
        /// <summary>
        /// used to setup sentanses TEMP make this better!!
        /// </summary>
        void SetupSentanses()
        {
            sentanses = new string[3];
            sentanses[0] = "is på ko";
            sentanses[1] = "ko på is";
            sentanses[2] = "gås under ko";
        }

        /// <summary>
        /// call this to setup the minigame
        /// </summary>
        /// <param name="input">the dictionary that contains all the questions and images</param>
        public void SetDic(string[] input)
        {
            sentanses = input;

        
            towerManager.SetupGame(new SentenceToPictures(), new SpellWord());
        }



        void Start()
        {

            // setting up the main camera so it reflects the chosen difficulty. 
            mainCamera.GetComponent<ToggleZoom>().difficulty = difficulty;

            SetupSentanses();
            towerManager.SetupGame(new SentenceToPictures(), new SpellWord());
            if (ammo <= 0)
            {
                noAmmoText.SetActive(true);
                for (int i = 0; i < ammoDisplay.Length; i++)
                {
                    ammoDisplay[i].SetActive(false);
                }
                return;
            }

            if (ammo < ammoDisplay.Length)
            {
                for (int i = ammoDisplay.Length - 1; i >= ammo; i--)
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
        /// sends out a ray to detect what was cliced on.
        /// also handels the check if you cliced on the right image/stone
        /// </summary>
        void CheckWhatWasClickedOn()
        {
            if(ammo <= 0) return;

            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);
            if (hit.transform == null) return;

            Brick comp = hit.transform.gameObject.GetComponent<Brick>();
            if (comp == null || comp.isShootable == false) return;
            StartCoroutine(catapultAming.Shoot(hit.point, comp, this));
        }

        /// <summary>
        /// removes ammo and updates the pile of ammo
        /// </summary>
        public void RemoveAmmo()
        {
            ammo--;
            if(ammo < ammoDisplay.Length)
                ammoDisplay[ammo].SetActive(false);
            if (ammo <= 0)
                noAmmoText.SetActive(true);
        }

        /// <summary>
        /// shows the tooltip for the amount of ammo the player has
        /// </summary>
        private void OnMouseEnter()
        {
            pupUp.SetAndShowToolTip(ammo.ToString());
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
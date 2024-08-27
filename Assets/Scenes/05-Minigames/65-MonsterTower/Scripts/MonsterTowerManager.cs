using System;
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

        int ammo;

        [SerializeField] Camera mainCamera;
        [SerializeField] GameObject noAmmoText;
        [SerializeField] List<GameObject> ammoDisplay;
        [SerializeField] GameObject ammoToDisplayPrefab;
        [SerializeField] GameObject ammoPlatform;
        [SerializeField] CatapultAming catapultAming;
        RaycastHit hit;
        Ray ray;
        [SerializeField] AmmoPupUp pupUp;

        [SerializeField] TowerManager towerManager;

        public Difficulty difficulty;

        //temp
        List<string> words;
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
        /// call this to setup the minigame
        /// </summary>
        /// <param name="input">the dictionary that contains all the questions and images</param>
     



        void Start()
        {

            // setting up the main camera so it reflects the chosen difficulty. 
            mainCamera.GetComponent<ToggleZoom>().difficulty = difficulty;

            SetupSentanses();

            for (int x = 0; x < words.Count; x++)
            {
                for (int i = 0; i < ammoDisplay.Count; i++)
                {
                    ammoToDisplayPrefab.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = words[x];

                }

                GameObject ammo = Instantiate(ammoToDisplayPrefab, ammoPlatform.transform.position + new Vector3(1 * x - 0.78f, 0), Quaternion.identity);
                ammo.transform.parent = ammoPlatform.transform;
                ammoDisplay.Add(ammo);
            }

            ammo = ammoDisplay.Count;

            if (ammo <= 0)
            {
                noAmmoText.SetActive(true);
                for (int i = 0; i < ammoDisplay.Count; i++)
                {
                    ammoDisplay[i].SetActive(false);
                }
                return;
            }

            if (ammo < ammoDisplay.Count)
            {
                for (int i = ammoDisplay.Count - 1; i >= ammo; i--)
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
            if(ammo < ammoDisplay.Count)
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
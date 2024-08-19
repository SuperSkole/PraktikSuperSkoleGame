using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CORE.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    //temp
    string[] sentanses;
    /// <summary>
    /// used to setup sentences TEMP make this better!!
    /// </summary>
    void SetupSentanses()
    {
        sentanses = new string[3];
        sentanses[0] = "is p� ko";
        sentanses[1] = "ko p� is";
        sentanses[2] = "g�s under ko";
    }

    /// <summary>
    /// call this to setup the minigame
    /// </summary>
    /// <param name="input">the dictionary that contains all the questions and images</param>
    public void SetDic(string[] input)
    {
        sentanses = input;

        
        towerManager.SetTowerData(sentanses);
    }



    void Start()
    {
        SetupSentanses();
        towerManager.SetTowerData(sentanses);
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
        catapultAming.Shoot(hit.point, comp);
        RemoveAmmo();
            

    }

    /// <summary>
    /// removes ammo and updates the pile of ammo
    /// </summary>
    void RemoveAmmo()
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

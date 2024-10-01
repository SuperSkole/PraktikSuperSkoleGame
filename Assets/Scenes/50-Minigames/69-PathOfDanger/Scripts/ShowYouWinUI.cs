using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowYouWinUI : MonoBehaviour
{
    public GameObject WinUI;
    public bool hasCollided = false;
    public PathOfDangerManager manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// If the player collides with the last Platform then the winUi will be activated and movement will be removed. 
    /// Also sets up the player to defaultComponents. 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided == false)
        {
            manager.SetupPlayerToDefaultComponents();
            PlayerManager.Instance.SpawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = false;
            PlayerManager.Instance.SpawnedPlayer.GetComponent<Rigidbody>().velocity=Vector3.zero;
            WinUI.SetActive(true);
            hasCollided = true;
        }
    }
}

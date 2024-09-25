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

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided == false)
        {
            manager.SetupPlayerToDefaultComponents();
            PlayerManager.Instance.SpawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = false;
            WinUI.SetActive(true);
            hasCollided = true;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using CORE;
using LoadSave;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public Transform playerSpawnPoint;
    
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Camera cameraBrain;
    [SerializeField] LayerMask layerMask;
    
    private PlayerData playerData;
    private GameObject spawnedPlayer;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void InitializePlayerData(PlayerData data)
    {
        playerData = data;

        // Init other player settings or components
    }

    public void InstantiatePlayer()
    {
        var tmp = 0;
        try
        {
            tmp = GameManager.Instance.PlayerData.MonsterTypeID;
        }
        catch (System.Exception)
        {
            Debug.Log("SceneStartBehavior/Start/No Game Instance so no monster ID can be found, using ID: 0 ");
            tmp = 0;
        }
        switch (tmp)
        {
            case 0:
                spawnedPlayer = Instantiate(playerPrefab, playerSpawnPoint);
                playerData = spawnedPlayer.GetComponent<PlayerData>();
                virtualCamera.Follow = spawnedPlayer.transform;
                virtualCamera.LookAt = spawnedPlayer.transform;
                spawnedPlayer.GetComponent<SpinePlayerMovement>().sceneCamera = cameraBrain;
                spawnedPlayer.GetComponent<SpinePlayerMovement>().placementLayermask = layerMask;
        
                try
                {
                    PopulatePlayerInfo();
                }
                catch (System.Exception) { Debug.Log("SceneStartBehavior/Start/Error when trying to populate player info "); }
                break;
        }
    }
    
    private void PopulatePlayerInfo()
    {
        var gm = GameManager.Instance.PlayerData;
        playerData.Username = gm.Username;
        playerData.MonsterName = gm.MonsterName;
        playerData.MonsterTypeID = gm.MonsterTypeID;
        playerData.MonsterColor = gm.MonsterColor;
        playerData.CurrentGoldAmount = gm.CurrentGoldAmount;
        playerData.CurrentXPAmount = gm.CurrentXPAmount;
        playerData.CurrentLevel = gm.CurrentLevel;
        
        spawnedPlayer.GetComponent<PlayerColorManager>().ColorChange(playerData.MonsterColor);
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.StartsWith("00") && !scene.name.StartsWith("01"))
        {
            spawnedPlayer = GameObject.Find("PlayerPrefab(Clone)");
            //spawnedPlayer.GetComponent<PlayerColorManager>().ColorChange(playerData.MonsterColor);
        }
    }
}
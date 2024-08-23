using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWorldStartup : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private GameObject spawnedPlayer;
    private void Start()
    {
        Debug.Log("MainWorldStartup/Start/Setting up player camera");
        spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;

        //virtualCamera.Follow = spawnedPlayer.transform;
        //virtualCamera.LookAt = spawnedPlayer.transform;



    }

}

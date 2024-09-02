using Cinemachine;
using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private GameObject spawnedPlayer;

    private void Start()
    {
        Debug.Log("MainWorldStartup/Start/Setting up player camera");
        spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
    }

}

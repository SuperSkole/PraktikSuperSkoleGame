using Cinemachine;
using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private GameObject spawnedPlayer;


    /// <summary>
    /// sets the player in the world at the object this script is attatched to
    /// </summary>
    private void Start()
    {
        //Debug.Log("MainWorldStartup/Start/Setting up player camera");
        spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
        PlayerManager.Instance.PositionPlayerAt(gameObject);
    }

}

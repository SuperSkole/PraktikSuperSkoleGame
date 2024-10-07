using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_SPPlayerManager : MonoBehaviour
{
    public GameObject PlayerSpawnPoint;
    private void Start()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.PositionPlayerAt(PlayerSpawnPoint);
                
            PlayerManager.Instance.SpawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = false;
            PlayerManager.Instance.SpawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;
        }
        else
        {
            Debug.Log("MP_SPPlayerManager.Start(): Player Manager is null");
        }
    }
}

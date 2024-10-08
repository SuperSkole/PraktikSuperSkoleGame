using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerMP : MonoBehaviour
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
                Debug.Log("WordFactory GM.Start(): Player Manager is null");
            }
    }
}

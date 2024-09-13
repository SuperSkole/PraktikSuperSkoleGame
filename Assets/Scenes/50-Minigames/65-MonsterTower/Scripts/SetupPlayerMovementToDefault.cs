using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupPlayerMovementToDefault : MonoBehaviour
{
    public void SetUpDefaultMovement()
    {
        Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerMovement_MT>());
        Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<AutoMovePlayer_MT>());
    }
}

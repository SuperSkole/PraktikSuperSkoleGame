using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpPlayerMovementToDefault : MonoBehaviour
{
    public void SetUp()
    {
        Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerMovement_MT>());
    }
}

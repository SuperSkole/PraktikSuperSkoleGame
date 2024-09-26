using System.Collections;
using System.Collections.Generic;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;

public class PlayerPrison : MonoBehaviour
{
    [SerializeField]GameObject playerPrison;
    private bool positionedPlayer = false;
    // Start is called before the first frame update
    void Update()
    {
        if(!positionedPlayer && PlayerManager.Instance != null)
        {
            PlayerManager.Instance.PositionPlayerAt(playerPrison);
            positionedPlayer = true;
        }
    }
}

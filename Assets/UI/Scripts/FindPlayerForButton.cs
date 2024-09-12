using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindPlayerForButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SpawnCarCloseToPlayer()
    {
        var carGO = GameObject.FindGameObjectWithTag("Car");

        carGO.transform.position = PlayerManager.Instance.SpawnedPlayer.transform.position + new Vector3(5,0,0);

    }
}

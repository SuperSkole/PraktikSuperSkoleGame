using Cinemachine;
using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathOfDangerManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject playerSpawnPoint;
    private GameObject spawnedPlayer;
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.PositionPlayerAt(playerSpawnPoint);
            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
            spawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = true;
            spawnedPlayer.GetComponent<Rigidbody>().useGravity = true;
            spawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;
            spawnedPlayer.GetComponent<PlayerFloating>().enabled = true;
            spawnedPlayer.GetComponent<PlayerAnimatior>().StartUp();
           
            PlayerMovement_POD playerMovement = PlayerManager.Instance.SpawnedPlayer.AddComponent<PlayerMovement_POD>();
             
            playerMovement.sceneCamera = Camera.main;

            virtualCamera.Follow = spawnedPlayer.transform;
            virtualCamera.LookAt = spawnedPlayer.transform;
           

            //spawnedPlayer.GetComponent<PlayerAnimatior>().SetCharacterState("Idle");
            //spawnedPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //spawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;


            //  SetupPlayerMovementForMonsterTower();
        }
        else
        {
            Debug.Log("WordFactory GM.Start(): Player Manager is null");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

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

    [SerializeField] LayerMask playerPlacementLayerMask;

    public int playerLifePoints=3;
    void Start()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.PositionPlayerAt(playerSpawnPoint);
            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;

            SpinePlayerMovement playerSpinePlayerMovement = spawnedPlayer.GetComponent<SpinePlayerMovement>();

            playerSpinePlayerMovement.enabled = true;
            playerSpinePlayerMovement.sceneCamera = Camera.main;

            Rigidbody playerRigidBody = spawnedPlayer.GetComponent<Rigidbody>();
            playerRigidBody.useGravity = true;
            spawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;
            spawnedPlayer.GetComponent<PlayerFloating>().enabled = true;

         

           
            //PlayerMovement_POD playerMovement = PlayerManager.Instance.SpawnedPlayer.AddComponent<PlayerMovement_POD>();
            PlayerAnimatior playerAnimator = spawnedPlayer.GetComponent<PlayerAnimatior>();

            //playerMovement.sceneCamera = Camera.main;

            virtualCamera.Follow = spawnedPlayer.transform;
            virtualCamera.LookAt = spawnedPlayer.transform;

            //playerMovement.placementLayermask = playerPlacementLayerMask;
            

            playerAnimator.SetCharacterState("Idle");

            //playerMovement.animatior = playerAnimator;

            spawnedPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;

            Jump jumpComp=spawnedPlayer.AddComponent<Jump>();
            jumpComp.rigidbody = playerRigidBody;

            OutOfBounce outOfBouncePComp= spawnedPlayer.AddComponent<OutOfBounce>();
            outOfBouncePComp.startPosition = playerSpawnPoint.transform.position;
            outOfBouncePComp.manager = this;

            ////  SetupPlayerMovementForMonsterTower();
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

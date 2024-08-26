using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Camera cameraBrain;
    [SerializeField] private LayerMask layerMask;
    private GameObject spawnedPlayer;


    // Start is called before the first frame update
    public void Setup()
    {
        spawnedPlayer = GameObject.FindGameObjectWithTag("Player");
        virtualCamera.Follow = spawnedPlayer.transform;
        virtualCamera.LookAt = spawnedPlayer.transform;
        spawnedPlayer.GetComponent<SpinePlayerMovement>().sceneCamera = cameraBrain;
        spawnedPlayer.GetComponent<SpinePlayerMovement>().placementLayermask = layerMask;
    }
}

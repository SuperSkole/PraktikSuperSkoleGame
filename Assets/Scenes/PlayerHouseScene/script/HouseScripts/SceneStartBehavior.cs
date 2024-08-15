using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStartBehavior : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private GameObject buildingSystem;
    [SerializeField] private GameObject uiBuilding;
    [SerializeField] private CameraMovement cameraMovement;
    

    private GameObject spawnedPlayer;
    //[SerializeField] private PreviewSystem previewSystem;

    // Start is called before the first frame update
    void Start()
    {
        spawnedPlayer = Instantiate(playerPrefab,playerSpawnPoint);
        buildingSystem.SetActive(false);
        uiBuilding.SetActive(buildingSystem.activeSelf);
        cameraMovement.enabled = buildingSystem.activeSelf;
    }
    public void EnableBuildingSystem()
    {
        if (!buildingSystem.activeSelf)
        {
            buildingSystem.SetActive(true);
            cameraMovement.enabled = buildingSystem.activeSelf;
            spawnedPlayer.SetActive(false);
        }
        else 
        { 
            buildingSystem.SetActive(false); 
            cameraMovement.enabled = buildingSystem.activeSelf;
            spawnedPlayer.SetActive(true);
        }

        uiBuilding.SetActive(buildingSystem.activeSelf);

    }
}

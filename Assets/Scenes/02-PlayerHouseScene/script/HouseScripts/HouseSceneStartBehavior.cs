using Cinemachine;
using UnityEngine;

public class HouseSceneStartBehavior : MonoBehaviour
{
    [SerializeField] private GameObject buildingSystem;
    [SerializeField] private GameObject uiBuilding;
    [SerializeField] private GameObject cameraMovement;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private GameObject spawnedPlayer;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Trying to set player obj in house start");
        spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;

        virtualCamera.Follow = spawnedPlayer.transform;
        virtualCamera.LookAt = spawnedPlayer.transform;


        buildingSystem.SetActive(false);
        uiBuilding.SetActive(buildingSystem.activeSelf);
        cameraMovement.GetComponent<CameraMovement>().enabled = buildingSystem.activeSelf;
    }

    public void EnableBuildingSystem()
    {
        if (!buildingSystem.activeSelf)
        {
            buildingSystem.SetActive(true);
            cameraMovement.GetComponent<CameraMovement>().enabled = buildingSystem.activeSelf;
            spawnedPlayer.SetActive(false);
            virtualCamera.Follow = cameraMovement.transform;
            virtualCamera.LookAt = cameraMovement.transform;
        }
        else
        {
            buildingSystem.SetActive(false);
            cameraMovement.GetComponent<CameraMovement>().enabled = buildingSystem.activeSelf;
            spawnedPlayer.SetActive(true);
            virtualCamera.Follow = spawnedPlayer.transform;
            virtualCamera.LookAt = spawnedPlayer.transform;
        }

        uiBuilding.SetActive(buildingSystem.activeSelf);
    }
}

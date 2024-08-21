using Cinemachine;
using CORE;
using LoadSave;
using UnityEngine;

public class SceneStartBehavior : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    PlayerData playerData;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private GameObject buildingSystem;
    [SerializeField] private GameObject uiBuilding;
    [SerializeField] private GameObject cameraMovement;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private GameObject spawnedPlayer;
    //[SerializeField] private PreviewSystem previewSystem;

    // Start is called before the first frame update
    void Start()
    {
        var tmp = 0;
        try
        {
            tmp = GameManager.Instance.PlayerData.MonsterTypeID;
        }
        catch (System.Exception)
        {
            Debug.Log($"SceneStartBehavior/Start/No Game Instance so no monster ID can be found, using ID: {tmp} ");
            tmp = 0;
        }
        switch (tmp)
        {
            case 0:
                spawnedPlayer = Instantiate(playerPrefab, playerSpawnPoint);
                playerData = spawnedPlayer.GetComponent<PlayerData>();
                try
                {
                    PopulatePlayerInfo();
                }
                catch (System.Exception) { Debug.Log("SceneStartBehavior/Start/Error when trying to populate player info "); }
                break;
        }
        buildingSystem.SetActive(false);
        uiBuilding.SetActive(buildingSystem.activeSelf);
        cameraMovement.GetComponent<CameraMovement>().enabled = buildingSystem.activeSelf;
    }
    private void PopulatePlayerInfo()
    {
        var gm = GameManager.Instance.PlayerData;
        playerData.Username = gm.Username;
        playerData.MonsterName = gm.MonsterName;
        playerData.MonsterTypeID = gm.MonsterTypeID;
        playerData.MonsterColor = gm.MonsterColor;
        playerData.CurrentGoldAmount = gm.CurrentGoldAmount;
        playerData.CurrentXPAmount = gm.CurrentXPAmount;
        playerData.CurrentLevel = gm.CurrentLevel;
        
        spawnedPlayer.GetComponent<PlayerColorManager>().ColorChange(playerData.MonsterColor);
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

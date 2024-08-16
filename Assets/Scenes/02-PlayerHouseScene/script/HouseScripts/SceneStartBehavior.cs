using CORE;
using Player;
using UnityEngine;

public class SceneStartBehavior : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    PlayerData playerData;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private GameObject buildingSystem;
    [SerializeField] private GameObject uiBuilding;
    [SerializeField] private CameraMovement cameraMovement;


    private GameObject spawnedPlayer;
    //[SerializeField] private PreviewSystem previewSystem;

    // Start is called before the first frame update
    void Start()
    {
        var tmp = GameManager.Instance.PlayerData.MonsterTypeID;
        switch (tmp)
        {
            case 0:
                spawnedPlayer = Instantiate(playerPrefab, playerSpawnPoint);
                playerData = spawnedPlayer.GetComponent<PlayerData>();
                PopulatePlayerInfo();
                break;
        }
        buildingSystem.SetActive(false);
        uiBuilding.SetActive(buildingSystem.activeSelf);
        cameraMovement.enabled = buildingSystem.activeSelf;
    }
    private void PopulatePlayerInfo()
    {
        playerData.HashedUsername = GameManager.Instance.PlayerData.HashedUsername;
        playerData.PlayerName= GameManager.Instance.PlayerData.PlayerName;
        playerData.MonsterTypeID = GameManager.Instance.PlayerData.MonsterTypeID;
        playerData.CurrentGoldAmount = GameManager.Instance.PlayerData.CurrentGoldAmount;
        playerData.CurrentXPAmount = GameManager.Instance.PlayerData.CurrentXPAmount;
        playerData.CurrentLevel = GameManager.Instance.PlayerData.CurrentLevel;

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

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
    [SerializeField] private CameraMovement cameraMovement;


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
            Debug.Log("SceneStartBehavior/Start/No Game Instance so no monster ID can be found, using ID: 0 ");
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
        cameraMovement.enabled = buildingSystem.activeSelf;
    }
    private void PopulatePlayerInfo()
    {
        playerData.Username = GameManager.Instance.PlayerData.Username;
        playerData.PlayerName = GameManager.Instance.PlayerData.PlayerName;
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

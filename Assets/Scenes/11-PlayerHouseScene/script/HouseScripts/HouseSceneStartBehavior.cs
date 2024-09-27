using Cinemachine;
using Scenes._10_PlayerScene.Scripts;
using Scenes._11_PlayerHouseScene.script.CameraScripts;
using Scenes._11_PlayerHouseScene.script.SaveData;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.HouseScripts
{
    public class HouseSceneStartBehavior : MonoBehaviour
    {
        [SerializeField] private GameObject buildingSystemParent;
        [SerializeField] private GameObject uiBuilding;
        [SerializeField] private GameObject cameraMovement;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        [SerializeField] private HouseLoadSaveController saveManager;
        [SerializeField] private PlacementSystem placementSystem;

        private GameObject spawnedPlayer;

        // Start is called before the first frame update
        private void Start()
        {
            //Debug.Log("Trying to set player obj in house start");
            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
            
            buildingSystemParent.SetActive(true);
            LoadingHouseItems();
            buildingSystemParent.SetActive(false);

            uiBuilding.SetActive(buildingSystemParent.activeSelf);
            cameraMovement.GetComponent<CameraMovement>().enabled = buildingSystemParent.activeSelf;
        }
        public void LoadingHouseItems()
        {
            saveManager.LoadGridData();
            foreach (var item in saveManager.itemContainer.floorData.placedObjectsList)
            {
                placementSystem.PlaceItemsStartLoading(item.Key, item.ID, EnumFloorDataType.Rug);
            }

            foreach (var item in saveManager.itemContainer.furnitureData.placedObjectsList)
            {
                placementSystem.PlaceItemsStartLoading(item.Key, item.ID, EnumFloorDataType.Furniture);
            }
        }
        public void EnableBuildingSystem()
        {
            if (!buildingSystemParent.activeSelf)
            {
                buildingSystemParent.SetActive(true);
                cameraMovement.GetComponent<CameraMovement>().enabled = buildingSystemParent.activeSelf;
                spawnedPlayer.SetActive(false);
                virtualCamera.Follow = cameraMovement.transform;
                virtualCamera.LookAt = cameraMovement.transform;
            }
            else
            {
                saveManager.floorData.FloorData = placementSystem.FloorData;
                saveManager.furnitureData.FurnitureData = placementSystem.FurnitureData;
                saveManager.SaveGridData();

                buildingSystemParent.SetActive(false);
                cameraMovement.GetComponent<CameraMovement>().enabled = buildingSystemParent.activeSelf;
                spawnedPlayer.SetActive(true);
                virtualCamera.Follow = spawnedPlayer.transform;
                virtualCamera.LookAt = spawnedPlayer.transform;
            }

            uiBuilding.SetActive(buildingSystemParent.activeSelf);
        }
    }
}

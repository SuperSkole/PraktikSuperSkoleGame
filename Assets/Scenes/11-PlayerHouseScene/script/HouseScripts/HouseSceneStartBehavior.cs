using Cinemachine;
using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using Scenes._11_PlayerHouseScene.script.CameraScripts;
using Scenes._11_PlayerHouseScene.script.SaveData;
using System.Threading.Tasks;
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
        
        [HideInInspector]
        public SaveContainer itemContainer = new SaveContainer();

        private GameObject spawnedPlayer;

        // Start is called before the first frame update
        private void Start()
        {
            //Debug.Log("Trying to set player obj in house start");
            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
            
            buildingSystemParent.SetActive(true);
            //LoadingHouseItems();
            buildingSystemParent.SetActive(false);

            uiBuilding.SetActive(buildingSystemParent.activeSelf);
            cameraMovement.GetComponent<CameraMovement>().enabled = buildingSystemParent.activeSelf;
        }
        public void LoadingHouseItems()
        {
            var data = saveManager.LoadGridData<HouseDataDTO>();
            ApplyLoadedData(data);

            foreach (var item in itemContainer.floorData.placedObjectsList)
            {
                placementSystem.PlaceItemsStartLoading(item.Key, item.ID, EnumFloorDataType.Rug);
            }

            foreach (var item in itemContainer.furnitureData.placedObjectsList)
            {
                placementSystem.PlaceItemsStartLoading(item.Key, item.ID, EnumFloorDataType.Furniture);
            }
        }
        public void ApplyLoadedData(Task<HouseDataDTO> houseDataDTO)
        {
            // Apply the loaded grid data to the house systems
            itemContainer.floorData = houseDataDTO.Result.FloorData;
            itemContainer.furnitureData = houseDataDTO.Result.FurnitureData;

            //   floorData.FloorData = houseDataDTO.FloorData
            // furnitureData.FurnitureData.placedObjects = houseDataDTO.FurnitureData.ConvertToDictionary();

            Debug.Log("House data applied successfully.");
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

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
        [SerializeField] private UIInvetoryManager invetoryManager;
        [SerializeField] private PlacementSystem placementSystem;

        [HideInInspector]
        public SaveContainer itemContainer = new SaveContainer();

        private GameObject spawnedPlayer;

        // Start is called before the first frame update
        private async void Start()
        {
            //Debug.Log("Trying to set player obj in house start");
            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;

            buildingSystemParent.SetActive(true);
            invetoryManager.LoadFurnitureAmount();
            try
            {
                await LoadingHouseItems();  // Wait for the house data to load
            }
            catch { print("No House Save has been found"); }

            buildingSystemParent.SetActive(false);

            uiBuilding.SetActive(buildingSystemParent.activeSelf);
            cameraMovement.GetComponent<CameraMovement>().enabled = buildingSystemParent.activeSelf;
        }
        public async Task LoadingHouseItems()
        {
            // Load the house data asynchronously
            var data = await saveManager.LoadGridData<HouseDataDTO>();

            ApplyLoadedData(data);  // Apply the loaded data after it's loaded

            foreach (var item in itemContainer.SavedGridData.placedObjectsList)
            {
                placementSystem.PlaceItemsStartLoading(item.Key.key, item.ID, item.FloorType, item.RotationValue);
            }
        }
        public void ApplyLoadedData(HouseDataDTO houseDataDTO)
        {
            // Apply the loaded grid data to the house systems
            itemContainer.SavedGridData = houseDataDTO.SavedGridData;

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

                invetoryManager.SaveFurnitureAmount();

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

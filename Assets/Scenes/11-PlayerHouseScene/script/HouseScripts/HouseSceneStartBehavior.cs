using Cinemachine;
using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using Scenes._11_PlayerHouseScene.script.CameraScripts;
using Scenes._11_PlayerHouseScene.script.SaveData;
using System.Collections.Generic;
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
        [SerializeField] private PreviewSystem previewSystem;
        [SerializeField] RectTransform inventoryContentUIParent;
        [SerializeField] RectTransform inScreenView;
        [SerializeField] RectTransform outSideScreenView;


        [HideInInspector]
        public SaveContainer itemContainer = new SaveContainer();

        private GameObject spawnedPlayer;
        [SerializeField] private GameObject houseFloorSide;//Where the walls are being placed by the PC
        private bool saveDataHasBeenMade = false;

        // Start is called before the first frame update
        private async void Start()
        {
            //Debug.Log("Trying to set player obj in house start");
            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;

            buildingSystemParent.SetActive(true);
            houseFloorSide.SetActive(true);

            inventoryContentUIParent.position = outSideScreenView.position;

            invetoryManager.LoadFurnitureAmount();

            await LoadingHouseItems();  // Wait for the house data to load


            houseFloorSide.SetActive(false);
            buildingSystemParent.SetActive(false);

            uiBuilding.SetActive(buildingSystemParent.activeSelf);
            cameraMovement.GetComponent<CameraMovement>().enabled = buildingSystemParent.activeSelf;
        }
        private void CreateHouseSaveData()
        {
            // Creates the walls around the house.
            List<SerializableKeyValuePair> defaultObjects = new List<SerializableKeyValuePair>
            {
                // Walls
                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(-5, -6, 0), EnumFloorDataType.NoneRemoveable),
                    50, 180, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(-3, -6, 0), EnumFloorDataType.NoneRemoveable),
                    51, 180, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(-1, -6, 0), EnumFloorDataType.NoneRemoveable),
                    52, 180, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(1, -6, 0), EnumFloorDataType.NoneRemoveable),
                    51, 180, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(-6, -5, 0), EnumFloorDataType.NoneRemoveable),
                    50, 270, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(-6, -3, 0), EnumFloorDataType.NoneRemoveable),
                    50, 270, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(-6, -1, 0), EnumFloorDataType.NoneRemoveable),
                    51, 270, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(-6, 1, 0), EnumFloorDataType.NoneRemoveable),
                    50, 270, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(-6, 3, 0), EnumFloorDataType.NoneRemoveable),
                    50, 270, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(-5, 5, 0), EnumFloorDataType.NoneRemoveable),
                    51, 0, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(-3, 5, 0), EnumFloorDataType.NoneRemoveable),
                    50, 0, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(-1, 5, 0), EnumFloorDataType.NoneRemoveable),
                    50, 0, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(1, 5, 0), EnumFloorDataType.NoneRemoveable),
                    50, 0, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(3, 5, 0), EnumFloorDataType.NoneRemoveable),
                    50, 0, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(5, 5, 0), EnumFloorDataType.NoneRemoveable),
                    50, 0, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(7, 5, 0), EnumFloorDataType.NoneRemoveable),
                    50, 0, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(9, 5, 0), EnumFloorDataType.NoneRemoveable),
                    50, 0, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(11, 5, 0), EnumFloorDataType.NoneRemoveable),
                    50, 0, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(13, 5, 0), EnumFloorDataType.NoneRemoveable),
                    51, 0, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(15, 3, 0), EnumFloorDataType.NoneRemoveable),
                    50, 90, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(15, 1, 0), EnumFloorDataType.NoneRemoveable),
                    50, 90, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(15, -1, 0), EnumFloorDataType.NoneRemoveable),
                    50, 90, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(15, -3, 0), EnumFloorDataType.NoneRemoveable),
                    50, 90, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(15, -5, 0), EnumFloorDataType.NoneRemoveable),
                    50, 90, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(13, -6, 0), EnumFloorDataType.NoneRemoveable),
                    50, 180, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(11, -6, 0), EnumFloorDataType.NoneRemoveable),
                    50, 180, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(9, -6, 0), EnumFloorDataType.NoneRemoveable),
                    50, 180, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(7, -6, 0), EnumFloorDataType.NoneRemoveable),
                    50, 180, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(5, -6, 0), EnumFloorDataType.NoneRemoveable),
                    50, 180, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(3, -6, 0), EnumFloorDataType.NoneRemoveable),
                    50, 180, EnumFloorDataType.NoneRemoveable),

                // Corner pices
                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(-6, -6, 0), EnumFloorDataType.NoneRemoveable),
                    53, 0, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(-6, 5, 0), EnumFloorDataType.NoneRemoveable),
                    53, 90, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(15, 5, 0), EnumFloorDataType.NoneRemoveable),
                    53, 180, EnumFloorDataType.NoneRemoveable),

                new SerializableKeyValuePair(
                    new PlaceableTemporayItemsInfo(new Vector3Int(15, -6, 0), EnumFloorDataType.NoneRemoveable),
                    53, 270, EnumFloorDataType.NoneRemoveable)

            };

            foreach (var item in defaultObjects)
            {
                placementSystem.PlaceItemsStartLoading(item.Key.key, item.ID, item.FloorType, item.RotationValue);
            }
        }

        public async Task LoadingHouseItems()
        {
            // Load the house data asynchronously
            var data = await saveManager.LoadGridData<HouseDataDTO>();

            if (data == null)
            {
                Debug.Log("No Save was Found");
                saveDataHasBeenMade = false;
                CreateHouseSaveData();
                return;
            }

            ApplyLoadedData(data);  // Apply the loaded data after it's loaded

            foreach (var item in itemContainer.SavedGridData.placedObjectsList)
            {
                placementSystem.PlaceItemsStartLoading(item.Key.key, item.ID, item.FloorType, item.RotationValue);
            }
            saveDataHasBeenMade = true;

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

                inventoryContentUIParent.position = inScreenView.position;

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

                inventoryContentUIParent.position = outSideScreenView.position;
                invetoryManager.SaveFurnitureAmount();

                previewSystem.StopShowingPreview();

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

using Scenes._11_PlayerHouseScene.script.SaveData;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.HouseScripts
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField] private InputManager inputManager;
        [SerializeField] private Grid grid;
        public HouseLoadSaveController saveManager;

        [SerializeField] private ObjectsDataBaseSO database;
        [SerializeField] private GameObject gridVisualization;

        public GameObject GridVisualization { get { return gridVisualization; } private set { } }

        private GridData floorData, furnitureData;

        public GridData FloorData { get => floorData; set => floorData = value; }
        public GridData FurnitureData { get => furnitureData; set => furnitureData = value; }

        [SerializeField] private PreviewSystem preview;
        private Vector3Int lastDetectedPosition = Vector3Int.zero;

        [SerializeField] private ObjectPlacer objectPlacer;
        private IBuildingState buildingState;

        // Initializes the placement system.
        private void Start()
        {
            // Stop any ongoing placement process.
            StopPlacement();
            floorData = new GridData();
            furnitureData = new GridData();

            // TODO refactor house save
            return;
            //if (GameManager.Instance.LoadManager.DoesSaveFileExist(
            //        GameManager.Instance.CurrentUser,
            //        GameManager.Instance.PlayerData.MonsterName,
            //        "house"))
            //{
            //    saveManager.LoadGridData();
            //    foreach (var item in saveManager.container.floorData.placedObjectsList)
            //    {
            //        PlaceItemsStartLoading(item.Key, item.ID);
            //    }

            //    foreach (var item in saveManager.container.furnitureData.placedObjectsList)
            //    {
            //        PlaceItemsStartLoading(item.Key, item.ID);
            //    }
            //}
        }

        private Vector3Int previousKey = new();
        public void PlaceItemsStartLoading(Vector3Int key, int ID, EnumFloorDataType floorType)
        {
            //if Obj is placed on 0,0,0 this doesnt work like it should
            //At later date update this so we can have checks for rugs i.e if something is bigger than 1x1

            if (floorType == EnumFloorDataType.Furniture && previousKey == Vector3Int.zero)
            {
                //Dont think this methode will work if size is 2x2 or rotation gets build in
                if (key.y == 0 && key.x == 1)
                {
                    return;
                }
                previousKey = key;
            }
            if (floorType == EnumFloorDataType.Furniture && !key.Equals(previousKey))
            {
                previousKey = new();
                return;
            }
            // Set the current building state to placement, passing in necessary dependencies.
            buildingState = new PlacementState(ID,
                grid,
                preview,
                database,
                floorData,
                furnitureData,
                objectPlacer,
                floorType);
            buildingState.OnLoadStartUp(key, ID);
        }
        // Starts the placement process for an object with the specified ID.
        // public void StartPlacement(int ID, EnumFloorDataType floorType)
        public void StartPlacement(PlaceableButtons info)
        {
            var ID = info.ID;
            var floorType = info.FloorType;
            // Stop any ongoing placement process.
            StopPlacement();

            // Activate the grid visualization.
            gridVisualization.SetActive(true);

            // Set the current building state to placement, passing in necessary dependencies.
            buildingState = new PlacementState(ID,
                grid,
                preview,
                database,
                floorData,
                furnitureData,
                objectPlacer,
                floorType);

            // Subscribe to input events for clicking and exiting the placement mode.
            inputManager.OnClicked += PlaceStructure;
            inputManager.OnExit += StopPlacement;
            inputManager.RotateMinus += RotateMinusGameObject;
            inputManager.RotatePlus += RotatePlusGameObject;
        }

        // Starts the process for removing an object.
        public void StartRemoving()
        {
            // Stop any ongoing placement process.
            StopPlacement();

            // Activate the grid visualization.
            gridVisualization.SetActive(true);

            // Set the current building state to removing, passing in necessary dependencies.
            buildingState = new RemovingState(grid,
                preview,
                floorData,
                furnitureData,
                objectPlacer);

            // Subscribe to input events for clicking and exiting the removal mode.
            inputManager.OnClicked += PlaceStructure;
            inputManager.OnExit += StopPlacement;
        }

        // Places the structure at the current grid position based on mouse position.
        private void PlaceStructure()
        {
            // Check if the mouse pointer is over a UI element to prevent unintended placement.
            if (inputManager.IsPointerOverUI())
            {
                return;
            }

            // Get the current mouse position on the map.
            Vector3 mousePos = inputManager.GetSelectedMapPosition();

            // Convert the world position of the mouse to a grid position.
            Vector3Int gridPos = grid.WorldToCell(mousePos);

            // Perform the action (place or remove) at the detected grid position.
            buildingState.OnAction(gridPos);
        }

        // Stops the current placement or removal process.
        private void StopPlacement()
        {
            // If there's no active building state, there's nothing to stop.
            if (buildingState == null)
            {
                return;
            }

            // Deactivate the grid visualization.
            gridVisualization.SetActive(false);

            // End the current state (clean up preview, etc.).
            buildingState.EndState();

            // Unsubscribe from input events to prevent further actions.
            inputManager.OnClicked -= PlaceStructure;
            inputManager.OnExit -= StopPlacement;
            inputManager.RotateMinus -= RotateMinusGameObject;
            inputManager.RotatePlus -= RotatePlusGameObject;

            // Reset the last detected grid position.
            lastDetectedPosition = Vector3Int.zero;

            // Clear the current building state.
            buildingState = null;
        }
        private void RotateMinusGameObject()
        {
            buildingState.RotateItem(-90);
        }
        private void RotatePlusGameObject()
        {
            buildingState.RotateItem(90);
        }

        // Updates the placement system each frame.
        private void Update()
        {
            // If there's no active building state, there's nothing to update.
            if (buildingState == null)
            {
                return;
            }

            // Get the current mouse position on the map.
            Vector3 mousePos = inputManager.GetSelectedMapPosition();

            // Convert the world position of the mouse to a grid position.
            Vector3Int gridPos = grid.WorldToCell(mousePos);
            // If the mouse has moved to a new grid position, update the building state.
            if (lastDetectedPosition != gridPos)
            {
                // Update the building state with the new grid position.
                buildingState.UpdateState(gridPos);

                // Update the last detected grid position.
                lastDetectedPosition = gridPos;
            }

        }
    }
}

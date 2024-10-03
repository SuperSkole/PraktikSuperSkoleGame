using Scenes._11_PlayerHouseScene.script.HouseScripts;
using Scenes._11_PlayerHouseScene.script.SaveData;
using System.Collections.Generic;
using System.ComponentModel;
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

        private GridData floorData, furnitureData, nonePlaceablesData;

        public GridData FloorData { get => floorData; set => floorData = value; }
        public GridData FurnitureData { get => furnitureData; set => furnitureData = value; }
        public GridData NonePlaceablesData { get => nonePlaceablesData; set => nonePlaceablesData = value; }


        [SerializeField] private PreviewSystem preview;
        private Vector3Int lastDetectedPosition = Vector3Int.zero;

        [SerializeField] private ObjectPlacer objectPlacer;
        private IBuildingState buildingState;
        [SerializeField] private UIInvetoryManager invetoryManager;

        [SerializeField]
        public Dictionary<PlaceableTemporayItemsInfo, SaveableGridData> placedObjectsSaved = new();
        public List<PlaceableTemporayItemsInfo> itemsFoundPositions = new();
      
        // Initializes the placement system.
        private void Start()
        {
            // Stop any ongoing placement process.
            StopPlacement();
            floorData = new GridData();
            furnitureData = new GridData();

            return;

        }

        public void PlaceItemsStartLoading(Vector3Int key, int ID, EnumFloorDataType floorType, int rotationValue)
        {
            // Set the current building state to placement, passing in necessary dependencies.
            buildingState = new PlacementState(ID,
                grid,
                preview,
                this,
                database,
                floorData,
                furnitureData,
                nonePlaceablesData,
                objectPlacer,
                invetoryManager,
                floorType);
            buildingState.OnLoadStartUp(key, ID, rotationValue);
        }
        /// <summary>
        /// Starts the placement process for an object with the specified ID.
        /// </summary>
        /// <param name="info"></param>
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
                this,
                database,
                floorData,
                furnitureData,
                nonePlaceablesData,
                objectPlacer,
                invetoryManager,
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
                this,
                invetoryManager,
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
        public void StopPlacement()
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

        /// <summary>
        /// Removes the object at the specified grid position from the grid.
        /// </summary>
        /// <param name="gridPos">The grid position of the object to be removed.</param>
        public void RemoveObjectAt()
        {
            foreach (PlaceableTemporayItemsInfo pos in itemsFoundPositions)
            {
                if (placedObjectsSaved.ContainsKey(pos))
                {
                   // print("Contains: " + pos);
                    placedObjectsSaved.Remove(pos);
                }
                else
                {
                    //print("Does not contain: " + pos);
                }
            }
            itemsFoundPositions.Clear();
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
public class PlaceableTemporayItemsInfo
{
    public Vector3Int key;
    public EnumFloorDataType floorDataType;

    public PlaceableTemporayItemsInfo(Vector3Int key, EnumFloorDataType floorDataType)
    {
        this.key = key;
        this.floorDataType = floorDataType;
    }
   
    // Override Equals
    public override bool Equals(object obj)
    {
        if (obj is PlaceableTemporayItemsInfo other)
        {
            return this.key.Equals(other.key) && this.floorDataType == other.floorDataType;
        }
        return false;
    }

    // Override GetHashCode
    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 31 + key.GetHashCode();
        hash = hash * 31 + floorDataType.GetHashCode();
        return hash;
    }

}

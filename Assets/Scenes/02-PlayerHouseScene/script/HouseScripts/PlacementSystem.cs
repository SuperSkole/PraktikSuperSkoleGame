using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private HouseSaving saveManager;

    [SerializeField] private ObjectsDataBaseSO database;
    [SerializeField] private GameObject gridVisualization;

    public GameObject GridVisualization { get { return gridVisualization; } private set { } }
    private GridData floorData, furnitureData;

    public GridData FloorData { get { return floorData; } set { floorData = value; } }
    public GridData FurnitureData { get { return furnitureData; } set { furnitureData = value; } }

    [SerializeField] private PreviewSystem preview;
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField] private ObjectPlacer objectPlacer;
    IBuildingState buildingState;

    // Initializes the placement system.
    private void Start()
    {
        // Stop any ongoing placement process.
        StopPlacement();
        floorData = new GridData();
        furnitureData = new GridData();
        if (saveManager.IsThereSaveFile())
        {
            saveManager.LoadGridData();
            foreach (var item in saveManager.container.floorData.placedObjectsList)
            {
                TestingSaving(item.Key, item.ID);
            }
            foreach (var item in saveManager.container.furnitureData.placedObjectsList)
            {
                TestingSaving(item.Key, item.ID);
            }
            //floorData.placedObjects = saveManager.ReturnLoadGridFile("floor");
            //furnitureData.placedObjects = saveManager.ReturnLoadGridFile("furniture");
            //PlaceSavedItems(floorData);
            //PlaceSavedItems(furnitureData);
        }

    }
    private int amountOfPlacedObj = 0;
    Vector3Int previousKey = new();

    private void TestingSaving(Vector3Int key, int ID)
    {
        //if Obj is placed on 0,0,0 this doesnt work like it should

        if (ID == 0 && previousKey == Vector3Int.zero)
        {
            previousKey = key;
        }
        if (ID == 0 && !key.Equals(previousKey))
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
                                           objectPlacer);
        buildingState.OnLoadStartUp(key, ID);



    }
    private void PlaceSavedItems(GridData data)
    {
        amountOfPlacedObj = 0;
        Vector3Int previousKey = new();
        //if Obj is placed on 0,0,0 this doesnt work like it should
        foreach (var item in data.placedObjects)
        {
            if (item.Value.ID == 0 && previousKey == Vector3Int.zero)
            {
                previousKey = item.Key;
            }
            if (item.Value.ID == 0 && !item.Key.Equals(previousKey))
            {
                previousKey = new();
                continue;
            }
            buildingState = new PlacementState(item.Value.ID,
                                   grid,
                                   preview,
                                   database,
                                   data,
                                   data,
                                   objectPlacer);
            //buildingState.OnLoadStartUp(item.Key);
            amountOfPlacedObj++;
        }
        print(amountOfPlacedObj);
    }
    // Starts the placement process for an object with the specified ID.
    public void StartPlacement(int ID)
    {
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
                                           objectPlacer);

        // Subscribe to input events for clicking and exiting the placement mode.
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
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

        // Reset the last detected grid position.
        lastDetectedPosition = Vector3Int.zero;

        // Clear the current building state.
        buildingState = null;
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

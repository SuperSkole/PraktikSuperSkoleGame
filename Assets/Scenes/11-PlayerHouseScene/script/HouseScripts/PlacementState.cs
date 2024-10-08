using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.HouseScripts
{
    public class PlacementState : IBuildingState
    {
        private int selectedObjectIndex = -1;
        private int ID;
        private Grid grid;
        private PreviewSystem previewSystem;
        private PlacementSystem placementSystem;
        private ObjectsDataBaseSO database;
        private GridData floorData;
        private GridData furnitureData;
        private GridData wallfurnitureData;
        private GridData nonePlaceablesData;
        private ObjectPlacer objectPlacer;
        private EnumFloorDataType floorType;
        private UIInvetoryManager invetoryManager;

        private Vector2Int sizeCopy;
        public Vector2Int SizeCopy { get { return sizeCopy; } set { sizeCopy = value; } }

        // Constructor for initializing the PlacementState with required dependencies.
        public PlacementState(int iD,
            Grid grid,
            PreviewSystem previewSystem,
            PlacementSystem placementSystem,
            ObjectsDataBaseSO database,
            GridData floorData,
            GridData furnitureData,
            GridData wallfurnitureData,
            GridData nonePlaceablesData,
            ObjectPlacer objectPlacer,
            UIInvetoryManager invetoryManager,
            EnumFloorDataType floorType)
        {
            // Initialize fields with provided parameters.
            ID = iD;
            this.grid = grid;
            this.previewSystem = previewSystem;
            this.placementSystem = placementSystem;
            this.database = database;
            this.floorData = floorData;
            this.furnitureData = furnitureData;
            this.wallfurnitureData = wallfurnitureData;
            this.nonePlaceablesData = nonePlaceablesData;
            this.objectPlacer = objectPlacer;
            this.invetoryManager = invetoryManager;
            this.floorType = floorType;

            // Find the index of the selected object in the database using its ID.
            selectedObjectIndex = database.objectData.FindIndex(data => data.ID == ID);
            SizeCopy = database.objectData[selectedObjectIndex].Size;
            // If the object is found, start showing the placement preview.
            if (selectedObjectIndex > -1)
            {
                previewSystem.StartShowingPlacementPreview(
                    database.objectData[selectedObjectIndex].Prefab,
                    SizeCopy);
            }
            else
            {
                // If the object is not found, throw an exception.
                throw new System.Exception($"No object with ID {iD}");
            }
        }

        // Ends the current placement state, stopping the preview.
        public void EndState()
        {
            previewSystem.StopShowingPreview();
        }

        // Called when an action (like clicking) is performed at a specific grid position.
        public void OnAction(Vector3Int gridPos)
        {
            // Check if the object can be placed at the specified grid position.
            bool placementValidity = CheckPlacementValidity(gridPos);
            if (!placementValidity)
                return;  // If placement is not valid, exit the method.

            // Place the object in the world using the object placer.
            int index = objectPlacer.PlaceObject(
                database.objectData[selectedObjectIndex].Prefab,
                grid.CellToWorld(gridPos),
                previewSystem.ReturnItemRotation());

            //// Determine whether the object is floor data or furniture data. has to fit with the Database
            //GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ?
            //    furnitureData : floorData;

            // Determine whether the object is floor data or furniture data. has to fit with the Database
            GridData selectedData = new();

            switch (floorType)
            {
                case EnumFloorDataType.Rug:
                    selectedData = floorData;
                    break;
                case EnumFloorDataType.Furniture:
                    selectedData = furnitureData;
                    break;
                case EnumFloorDataType.NoneRemoveable:
                    selectedData = nonePlaceablesData;
                    break;
                case EnumFloorDataType.WallPlaceable:
                    selectedData = wallfurnitureData;
                    break;
            }
            // Record the placed object's position, size, ID, and index in the grid data.
            selectedData.AddObjectAt(gridPos,
                SizeCopy,
                database.objectData[selectedObjectIndex].ID,
                index,
                floorType
                );

            int rotationYInDegrees = Mathf.RoundToInt(previewSystem.ReturnYEulerAngelsOnPreviewItem());

            placementSystem.placedObjectsSaved.Add(new PlaceableTemporayItemsInfo(gridPos, floorType),
                (new SaveableGridData(new Vector2Int(gridPos.x, gridPos.y),
                database.objectData[selectedObjectIndex].ID, rotationYInDegrees, floorType)));

            invetoryManager.ModifyFurnitureAmount(selectedObjectIndex);

            // Update the preview position and make it invalid (since the object is placed).
            previewSystem.UpdatePosition(grid.CellToWorld(gridPos), false);

            if (invetoryManager.ReturnAmountOfRemaingItems(selectedObjectIndex) <= 0)
            {
                placementSystem.StopPlacement();
            }
        }
        public void RotateItem(int degree)
        {

            previewSystem.RotateItem(degree, this);
        }

        public void OnLoadStartUp(Vector3Int gridPos, int ID, int RotationValue)
        {
            // Convert the rotation value (e.g., 90) to a clean quaternion
            Quaternion targetRotation = Quaternion.Euler(0, RotationValue, 0);
            // Place the object in the world using the object placer.
            int index = objectPlacer.PlaceObject(
                database.objectData[selectedObjectIndex].Prefab,
                grid.CellToWorld(gridPos),
                targetRotation);

            // Determine whether the object is floor data or furniture data. has to fit with the Database
            GridData selectedData = new();

            switch (floorType)
            {
                case EnumFloorDataType.Rug:
                    selectedData = floorData;
                    break;
                case EnumFloorDataType.Furniture:
                    selectedData = furnitureData;
                    break;
                case EnumFloorDataType.NoneRemoveable:
                    selectedData = nonePlaceablesData;
                    break;
                case EnumFloorDataType.WallPlaceable:
                    selectedData = wallfurnitureData;
                    break;
            }

            GetSizeOnRotation(RotationValue, selectedObjectIndex);

            // Record the placed object's position, size, ID, and index in the grid data.
            selectedData.AddObjectAt(gridPos,
                SizeCopy,
                database.objectData[selectedObjectIndex].ID,
                index,
                floorType);

            placementSystem.placedObjectsSaved.Add(new PlaceableTemporayItemsInfo(gridPos, floorType),
                 (new SaveableGridData(new Vector2Int(gridPos.x, gridPos.y),
                 database.objectData[selectedObjectIndex].ID, RotationValue, floorType)));

            // Update the preview position and make it invalid (since the object is placed).
            previewSystem.UpdatePosition(grid.CellToWorld(gridPos), false);
            previewSystem.StopShowingPreview();
        }
        public void OnStartUpSorftObjList()
        {
            objectPlacer.PlacedGameObjects.Sort();
        }
        private void GetSizeOnRotation(int rotationValue, int indexer)
        {
            var xtmp = SizeCopy.x;
            var ytmp = SizeCopy.y;
            var tmp = xtmp;
            switch (rotationValue)
            {
                case 0:
                    SizeCopy = database.objectData[indexer].Size;
                    break;
                case 90:
                    xtmp = ytmp;
                    ytmp = tmp;
                    SizeCopy = new Vector2Int(xtmp, ytmp);
                    break;
                case 180:
                    SizeCopy = database.objectData[indexer].Size;
                    break;
                case 270:
                    xtmp = ytmp;
                    ytmp = tmp;
                    SizeCopy = new Vector2Int(xtmp, ytmp);
                    break;
            }
        }

        // Updates the placement state as the player moves the cursor on the grid.
        public void UpdateState(Vector3Int gridPos)
        {
            // Check if the object can be placed at the new grid position.
            bool placementValidity = CheckPlacementValidity(gridPos);

            // Update the preview system's position and validity based on the new grid position.
            previewSystem.UpdatePosition(grid.CellToWorld(gridPos), placementValidity);
        }

        // Checks if the object can be placed at the specified grid position.
        private bool CheckPlacementValidity(Vector3Int gridPos)
        {
            // Determine whether the object is floor data or furniture data.
            GridData selectedData = new();

            switch (floorType)
            {
                case EnumFloorDataType.Rug:
                    selectedData = floorData;
                    break;
                case EnumFloorDataType.Furniture:
                    selectedData = furnitureData;
                    break;
                case EnumFloorDataType.NoneRemoveable:
                    selectedData = nonePlaceablesData;
                    break;
                case EnumFloorDataType.WallPlaceable:
                    selectedData = wallfurnitureData;
                    break;

            }
            //database.objectData[selectedObjectIndex].Size
            // Check if the object can be placed at the given grid position based on its size.
            if (selectedData == wallfurnitureData)
            {
                return selectedData.CanPlaceObjectAt(gridPos, SizeCopy, nonePlaceablesData, EnumFloorDataType.WallPlaceable);
            }
            else
            {
                return selectedData.CanPlaceObjectAt(gridPos, SizeCopy, nonePlaceablesData, null);
            }
        }
    }
}

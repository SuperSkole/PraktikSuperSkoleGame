using System;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.HouseScripts
{
    public class RemovingState : IBuildingState
    {
        private int gameObjectIndex = -1;
        private Grid grid;
        private PreviewSystem previewSystem;
        private PlacementSystem placementSystem;
        private UIInvetoryManager invetoryManager;
        private GridData floorData;
        private GridData furnitureData;
        private GridData wallfurnitureData;
        private GridData nonePlaceablesData;
        private ObjectPlacer objectPlacer;

        public RemovingState(Grid grid,
            PreviewSystem previewSystem,
            PlacementSystem placementSystem,
            UIInvetoryManager invetoryManager,
            GridData floorData,
            GridData furnitureData,
            GridData wallfurnitureData,
            GridData nonePlaceablesData,
            ObjectPlacer objectPlacer)
        {
            this.grid = grid;
            this.previewSystem = previewSystem;
            this.placementSystem = placementSystem;
            this.invetoryManager = invetoryManager;
            this.floorData = floorData;
            this.furnitureData = furnitureData;
            this.wallfurnitureData = wallfurnitureData;
            this.nonePlaceablesData = nonePlaceablesData;
            this.objectPlacer = objectPlacer;

            previewSystem.StartShowingRemovePreview();
        }

        public void EndState()
        {
            previewSystem.StopShowingPreview();
        }

        public void OnAction(Vector3Int gridPos)
        {
            GridData selectedData = null;
            if (furnitureData.CanPlaceObjectAt(gridPos, Vector2Int.one, furnitureData, null) == false)
            {
                selectedData = furnitureData;
            }
            else if (floorData.CanPlaceObjectAt(gridPos, Vector2Int.one, floorData, null) == false)
            {
                selectedData = floorData;
            }
            else if (wallfurnitureData.CanPlaceObjectAt(gridPos, Vector2Int.one, wallfurnitureData, EnumFloorDataType.WallPlaceable) == false)
            {
                selectedData = wallfurnitureData;
            }
            if (selectedData == null)
            {
                //PlaySound if nothing to remove
                return;
            }
            else
            {
                gameObjectIndex = selectedData.GetRepresentationIndex(gridPos);
                if (gameObjectIndex == -1)
                    return;

                invetoryManager.AddFuritureBackToPile(selectedData.placedObjects[gridPos].ID);
                selectedData.RemoveObjectAt(gridPos, placementSystem, selectedData.placedObjects[gridPos].FloorType);
                objectPlacer.RemoveObjectAt(gameObjectIndex);

                placementSystem.RemoveObjectAt();
            }
            Vector3 cellPos = grid.CellToWorld(gridPos);
            previewSystem.UpdatePosition(cellPos, CheckIfSelectionIsValid(gridPos));
        }

        private bool CheckIfSelectionIsValid(Vector3Int gridPos)
        {
            return !(furnitureData.CanPlaceObjectAt(gridPos, Vector2Int.one, null, null) &&
                     floorData.CanPlaceObjectAt(gridPos, Vector2Int.one, null, null) && 
                     wallfurnitureData.CanPlaceObjectAt(gridPos, Vector2Int.one, null, null));
        }

        public void UpdateState(Vector3Int gridPos)
        {
            bool validity = CheckIfSelectionIsValid((Vector3Int)gridPos);
            previewSystem.UpdatePosition(grid.CellToWorld(gridPos), validity);
        }

        public void OnLoadStartUp(Vector3Int gridPos, int ID, int RotationValue)
        {
            throw new NotImplementedException();
        }
        //No need to be able to rotate obj during deletion 
        public void RotateItem(int degree)
        {
            throw new NotImplementedException();
        }
    }
}

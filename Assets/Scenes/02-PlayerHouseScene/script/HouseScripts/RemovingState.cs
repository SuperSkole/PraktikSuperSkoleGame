using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    public RemovingState(Grid grid,
                         PreviewSystem previewSystem,
                         GridData floorData,
                         GridData furnitureData,
                         ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
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
        if (furnitureData.CanPlaceObjectAt(gridPos,Vector2Int.one) == false)
        {
            selectedData = furnitureData;
        }
        else if (floorData.CanPlaceObjectAt(gridPos, Vector2Int.one) == false)
        {
            selectedData = floorData;
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
            selectedData.RemoveObjectAt(gridPos);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }
        Vector3 cellPos = grid.CellToWorld(gridPos);
        previewSystem.UpdatePosition(cellPos, CheckIfSelectionIsValid(gridPos));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPos)
    {
        return !(furnitureData.CanPlaceObjectAt(gridPos, Vector2Int.one) &&
            floorData.CanPlaceObjectAt(gridPos, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool validity = CheckIfSelectionIsValid((Vector3Int)gridPos);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), validity);
    }

    public void OnLoadStartUp(Vector3Int gridPos, int ID)
    {
        throw new NotImplementedException();
    }
}

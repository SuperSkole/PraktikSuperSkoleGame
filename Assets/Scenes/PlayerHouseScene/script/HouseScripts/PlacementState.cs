using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDataBaseSO database;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDataBaseSO database,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objectData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                       database.objectData[selectedObjectIndex].Prefab,
                       database.objectData[selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No object with ID {iD}");
        }

    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPos)
    {
        bool placementValidty = CheckPlacementValidity(gridPos, selectedObjectIndex);
        if (!placementValidty)
            return;

        int index = objectPlacer.PlaceObject(
            database.objectData[selectedObjectIndex].Prefab,
            grid.CellToWorld(gridPos));

        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ?
            floorData : furnitureData;
        selectedData.AddObjectAt(gridPos,
                                 database.objectData[selectedObjectIndex].Size,
                                 database.objectData[selectedObjectIndex].ID,
                                 index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), false);
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool placementValidty = CheckPlacementValidity(gridPos, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), placementValidty);
    }

    private bool CheckPlacementValidity(Vector3Int gridPos, int selectedObjectindex)
    {
        GridData selectedData = database.objectData[selectedObjectindex].ID == 0 ?
            floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(gridPos, database.objectData[selectedObjectindex].Size);
    }

}

using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.HouseScripts
{
    /// <summary>
    /// Represents a data structure for managing the placement of objects on a grid.
    /// </summary>
    [Serializable]
    public class GridData
    {
        /// <summary>
        /// Dictionary storing the placed objects on the grid, with their grid positions as keys.
        /// </summary>
        [SerializeField]
        public Dictionary<Vector3Int, PlacementData> placedObjects = new();

        /// <summary>
        /// Adds an object to the grid at the specified position and size.
        /// </summary>
        /// <param name="gridPostion">The grid position where the object will be placed.</param>
        /// <param name="ObjectSize">The size of the object being placed on the grid.</param>
        /// <param name="ID">The identifier for the object being placed.</param>
        /// <param name="placedObjectIndex">The index representing the placed object.</param>
        /// <exception cref="Exception">Thrown if the grid position is already occupied.</exception>
        public void AddObjectAt(Vector3Int gridPostion,
            Vector2Int ObjectSize,
            int ID,
            int placedObjectIndex,
            EnumFloorDataType floorType)
        {
            //CalculatePositions does not like being fed for example objectSize 1,2 instead of 2,1 Cant fix the problem but put in a patch that should fix it
            List<Vector3Int> positionToOccupy = CalculatePositions(gridPostion, ObjectSize);
            //PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex, floorType, rotationValue);
            PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex, floorType);
            foreach (var pos in positionToOccupy)
            {
                if (placedObjects.ContainsKey(pos))
                {
                    throw new Exception($"Dictionary already contains this cell position {pos}");
                }
                placedObjects[pos] = data;
            }
        }

        /// <summary>
        /// Calculates all grid positions that the object will occupy based on its size.
        /// </summary>
        /// <param name="gridPostion">The starting grid position.</param>
        /// <param name="objectSize">The size of the object being placed.</param>
        /// <returns>A list of grid positions occupied by the object.</returns>
        private List<Vector3Int> CalculatePositions(Vector3Int gridPostion, Vector2Int objectSize)
        {
            List<Vector3Int> returnVal = new();
            for (int x = 0; x < objectSize.x; x++)
            {
                for (int y = 0; y < objectSize.y; y++)
                {
                    //Patch work so its doesnt break if its fed 1,2 instead of 2,1
                    var tmp = gridPostion + new Vector3Int(x, 0, y);
                    if (tmp.z > 0)
                    {
                        tmp.y += tmp.z;
                        tmp.z = 0;
                    }
                    returnVal.Add(tmp);
                }
            }
            return returnVal;
        }

        /// <summary>
        /// Checks if an object can be placed at the specified grid position with the given size.
        /// </summary>
        /// <param name="gridPostion">The grid position where the object would be placed.</param>
        /// <param name="objectSize">The size of the object being placed.</param>
        /// <returns>True if the object can be placed, otherwise false.</returns>
        //public bool CanPlaceObjectAt(Vector3Int gridPostion, Vector2Int objectSize)
        public bool CanPlaceObjectAt(Vector3Int gridPostion, Vector2Int objectSize, GridData? wallGridData, EnumFloorDataType? currentFloorType)
        {
            List<Vector3Int> positionToOccupy = CalculatePositions(gridPostion, objectSize);
            foreach (var pos in positionToOccupy)
            {
                if (wallGridData != null && wallGridData.placedObjects.ContainsKey(pos))
                {
                    return false;
                }
                //Debug.Log($"Checking These pos:{pos}");
                if (placedObjects.ContainsKey(pos))
                {
                    return false;
                }
                if (currentFloorType != null && currentFloorType == EnumFloorDataType.WallPlaceable)
                {
                    return IsWallNearBy(pos, wallGridData);
                }
            }
            return true;
        }

        private bool IsWallNearBy(Vector3Int gridposition, GridData wallGridData)
        {
            if (wallGridData.placedObjects.ContainsKey(new Vector3Int(gridposition.x, gridposition.y+1, gridposition.z)))
            {
                return true;
            }
            if (wallGridData.placedObjects.ContainsKey(new Vector3Int(gridposition.x+1, gridposition.y, gridposition.z)))
            {
                return true;
            }   
            if (wallGridData.placedObjects.ContainsKey(new Vector3Int(gridposition.x, gridposition.y-1, gridposition.z)))
            {
                return true;
            }   
            if (wallGridData.placedObjects.ContainsKey(new Vector3Int(gridposition.x-1, gridposition.y, gridposition.z)))
            {
                return true;
            }   

            return false;
        }

        /// <summary>
        /// Retrieves the index representing the placed object at the specified grid position.
        /// </summary>
        /// <param name="gridPos">The grid position of the object.</param>
        /// <returns>The index of the placed object, or -1 if no object is found.</returns>
        internal int GetRepresentationIndex(Vector3Int gridPos)
        {
            if (placedObjects.ContainsKey(gridPos) == false)
            {
                return -1;
            }
            return placedObjects[gridPos].PlacedObjectIndex;
        }

        /// <summary>
        /// Removes the object at the specified grid position from the grid.
        /// </summary>
        /// <param name="gridPos">The grid position of the object to be removed.</param>
        internal void RemoveObjectAt(Vector3Int gridPos, PlacementSystem placementSystem, EnumFloorDataType floorType)
        {
            foreach (var pos in placedObjects[gridPos].occupiedPositions)
            {
                placementSystem.itemsFoundPositions.Add(new PlaceableTemporayItemsInfo(pos, floorType));
                placedObjects.Remove(pos);
            }
        }
    }

    /// <summary>
    /// Represents data related to the placement of an object on a grid.
    /// </summary>
    [Serializable]
    public class PlacementData
    {
        /// <summary>
        /// A list of grid positions occupied by the object.
        /// </summary>
        public List<Vector3Int> occupiedPositions;

        /// <summary>
        /// Gets the identifier of the object.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Gets the index representing the placed object.
        /// </summary>
        public int PlacedObjectIndex { get; private set; }

        public EnumFloorDataType FloorType { get; private set; }

        public int rotationValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlacementData"/> class.
        /// </summary>
        /// <param name="occupiedPositions">The grid positions occupied by the object.</param>
        /// <param name="iD">The identifier of the object.</param>
        /// <param name="placedObjectIndex">The index representing the placed object.</param>
        public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex, EnumFloorDataType floorType)
        {
            this.occupiedPositions = occupiedPositions;
            ID = iD;
            PlacedObjectIndex = placedObjectIndex;
            FloorType = floorType;
            //this.rotationValue = rotationValue;
        }
    }
}
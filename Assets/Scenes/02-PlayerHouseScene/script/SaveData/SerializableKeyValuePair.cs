using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SerializableKeyValuePair
{
    public Vector3Int Key;

    public int ID;
    public int PlacedObjectIndex;
    public List<Vector3Int> occupiedPositions;

    public SerializableKeyValuePair(Vector3Int key, int iD, int placedObjectIndex, List<Vector3Int> occupiedPositions)
    {
        Key = key;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
        this.occupiedPositions = occupiedPositions;
    }

    public PlacementData CovertToPlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        return new PlacementData(occupiedPositions, iD, placedObjectIndex);
    }
}

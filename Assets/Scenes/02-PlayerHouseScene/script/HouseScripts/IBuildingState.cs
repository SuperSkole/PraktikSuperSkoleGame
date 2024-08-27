using UnityEngine;

public interface IBuildingState
{
    void EndState();
    void OnLoadStartUp(Vector3Int gridPos,int ID);
    void OnAction(Vector3Int gridPos);
    void UpdateState(Vector3Int gridPos);
}
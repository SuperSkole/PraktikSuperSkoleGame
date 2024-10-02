using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.HouseScripts
{
    public interface IBuildingState
    {
        void EndState();
        void OnLoadStartUp(Vector3Int gridPos, int ID, int RotationValue);
        void OnAction(Vector3Int gridPos);
        void UpdateState(Vector3Int gridPos);
        void RotateItem(int degree);
    }
}
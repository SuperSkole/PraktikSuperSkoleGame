using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.HouseScripts
{
    [CreateAssetMenu]
    public class ObjectsDataBaseSO : ScriptableObject
    {
        public List<ObjectData> objectData;
    }

    [Serializable]
    public class ObjectData
    {
        [field: SerializeField]
        //Name of the object
        public string name {  get; private set; }

        [field: SerializeField]
        //uniq ID for each object
        public int ID { get; private set; }

        [field: SerializeField]
        //Size of each object 1x1 default
        public Vector2Int Size { get; private set; } = Vector2Int.one;
    
        [field: SerializeField]
        //The reference to the object prefabs
        public GameObject Prefab { get; private set; }

        [field: SerializeField]
        public EnumFloorDataType ObjectType { get; private set; }
    }

    public enum EnumFloorDataType
    {
        Rug,            // Underneath furniture
        Wall,           // decor like wallpaper, images, cabinets or wall lights
        Furniture,      // Tables, chairs, beds, cabinets, etc.
        NoneRemoveable,    //For Objects that are not supposed to be removed, Used for the walls of the house
        WallPlaceable   //For items that can only be placed allong a wall
    }
}


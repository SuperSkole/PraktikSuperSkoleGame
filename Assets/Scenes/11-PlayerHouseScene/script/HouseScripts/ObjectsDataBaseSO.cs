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
        Furniture      // Tables, chairs, beds, cabinets, etc.

       // Ceiling,        // Ceiling or ceiling decorations
       // Appliance,      // Functional items like stoves, fridges, washing machines
       // Door,           // Any type of doors (interior or exterior)
       // Window,         // Windows and window frames
       // Lighting,       // Light fixtures, lamps, chandeliers
       // Flooring,       // General floors (tiles, wood, carpet excluding rugs)
       // Decor,          // Paintings, plants, sculptures, small decorative items
       // Electronics,    // TVs, computers, sound systems
       // Plumbing,       // Sinks, toilets, bathtubs, showers
       // Storage,        // Cabinets, wardrobes, shelves
       // Outdoor,        // Outdoor elements like fences, paths, gardens
       // Stairs,         // Stairs, ladders, or any kind of elevation structure
       // Utilities,      // Power outlets, radiators, air conditioners
       // Structural      // Columns, beams, and any non-decorative supporting structure

    }
}


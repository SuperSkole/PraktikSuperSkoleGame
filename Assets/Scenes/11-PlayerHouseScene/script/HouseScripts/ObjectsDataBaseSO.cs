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
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes._50_Minigames._67_WordProductionLine.Scripts
{

    public class ProductionLineObjectPool : MonoBehaviour
    {


        public List<GameObject> pooledObjects = new List<GameObject>();

        public int amountToPool = 5;

        public int amountSpawned;

        [SerializeField]
        private ProductionLineManager productionManager;

        [SerializeField]
        private GameObject BoxPrefab;

        // tells the list what kinda objects we want to pool. 
        void Start()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                GameObject obj = Instantiate(BoxPrefab, transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }


        // Gets the object that should pool.
        public GameObject GetPooledObject()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    amountSpawned++;
                    return pooledObjects[i];
                }
            }
            GameObject obj = Instantiate(BoxPrefab, transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            return obj;
        }


    }

}
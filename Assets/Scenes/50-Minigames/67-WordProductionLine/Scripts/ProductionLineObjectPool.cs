using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes._50_Minigames._67_WordProductionLine.Scripts
{

    public class ProductionLineObjectPool : MonoBehaviour
    {


        private List<GameObject> pooledObjects = new List<GameObject>();
        private int amountToPool = 10;

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
                    return pooledObjects[i];
                }
            }

            GameObject obj = Instantiate(BoxPrefab, transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            return obj;
        }

        /// <summary>
        /// resets the Cube, so the momentum dosnt stay.
        /// </summary>
        /// <param name="cube"></param>
        public void ResetCube(GameObject cube)
        {
            cube.SetActive(false);
            Rigidbody rb = cube.GetComponent<Rigidbody>();

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;


            rb.rotation = Quaternion.Euler(0, 0, 0);

        }


    }

}
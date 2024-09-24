using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes._50_Minigames._67_WordProductionLine.Scripts
{

    public class ProductionLineObjectPool : MonoBehaviour
    {


        public List<GameObject> pooledObjects = new List<GameObject>();

        public int amountToPool = 10;

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

        /// <summary>
        /// resets the Cube, so the momentum dosnt stay.
        /// </summary>
        /// <param name="cube"></param>
        public void ResetCube(GameObject cube)
        {

            LetterBox letterBox = cube.transform.GetChild(0).gameObject.GetComponent<LetterBox>();
            if (letterBox != null)
            {
               productionManager.WasCorrect(letterBox.letterText.text); 
            }
            

            cube.SetActive(false);
            Rigidbody rb = cube.GetComponent<Rigidbody>();

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;


            rb.rotation = Quaternion.Euler(0, 0, 0);

        }


    }

}
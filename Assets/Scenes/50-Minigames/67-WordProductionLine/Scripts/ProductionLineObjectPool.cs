using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ProductionLineObjectPool : MonoBehaviour
{
    

    private List<GameObject> pooledObjects = new List<GameObject>();
    private int amountToPool = 10;

    [SerializeField]
    private GameObject letterBoxPrefab;

    [SerializeField]
    private GameObject imageBoxPrefab;

    public static ProductionLineObjectPool instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // tells the list what kinda objects we want to pool. 
    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(letterBoxPrefab);
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

        return null;
    }

    
}

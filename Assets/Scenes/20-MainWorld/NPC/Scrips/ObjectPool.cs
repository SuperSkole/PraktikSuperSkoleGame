using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private List<GameObject> pool = new();
    [SerializeField] private int objectAmount = 10;

    private void Start()
    {
        if(prefab == null) return;

        for (int i = 0; i < objectAmount; i++)
        {
            pool.Add(Instantiate(prefab,transform));
        }
    }

    /// <summary>
    /// used to get an object from the pool and if the pool is emty is spawns a new object and adds it to the pool
    /// </summary>
    /// <returns>a gameobject instance of the given prefab</returns>
    public GameObject GetObject()
    {
        GameObject obj = null;
        foreach (GameObject go in pool)
        {
            if (!go.activeInHierarchy)
            {
                obj = go;
                break;
            }
        }
        if (obj == null)
        {
            obj = Instantiate(prefab, transform);
            pool.Add(obj);
        }

        return obj;
    }
}

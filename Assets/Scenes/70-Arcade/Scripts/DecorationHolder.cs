using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationHolder : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    void Start()
    {
        if(prefab != null)
        {
            GameObject decoration = Instantiate(prefab, this.transform);
        }
    }

}

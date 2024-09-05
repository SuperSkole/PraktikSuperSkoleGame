using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationHolder : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    /// <summary>
    /// sets the decoration chosen in the scene, on the object this is attatched to
    /// </summary>
    void Start()
    {
        if(prefab != null)
        {
            GameObject decoration = Instantiate(prefab, this.transform);
        }
    }

}

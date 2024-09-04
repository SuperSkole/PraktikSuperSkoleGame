using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    // Applies DontDestroyOnLoad to the gameobject.
    // Requires a way to delete it later or it will duplicate and leak memory if the scene containing the gameobject is loaded
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}

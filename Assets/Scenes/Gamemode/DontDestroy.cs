using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    // Applies DontDestroyOnLoad to the gameobject. requires a way to delete it later or it will duplicate and leak memory
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}

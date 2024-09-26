using Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToArcadeLoseScene : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        // Is used in start so when the object is instantiated a coroutine is started.  
        StartCoroutine(SwitchToLoseScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// the whole script is used so when a certain time has past the scene is switched to AsteroideLoseScreen. 
    /// </summary>
    /// <returns></returns>
    IEnumerator SwitchToLoseScene()
    {
        // suspend execution for 5 seconds
        yield return new WaitForSeconds(5);
        SwitchScenes.SwitchToArcadeAsteroidLoseScene();
    }
}

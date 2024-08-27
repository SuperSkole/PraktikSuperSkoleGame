using Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{   
    public static void SwitchToMainWorld() => SceneManager.LoadScene(SceneNames.Main);
    public static void SwitchToPlayerHouseScene() => SceneManager.LoadScene(SceneNames.House);
    public static void SwitchToWordFactory() => SceneManager.LoadScene(SceneNames.Factory);
    // TODO : Change this when we have a racing scene
    public static void SwitchToRacingScene() => SceneManager.LoadScene(SceneNames.House);    
    // TODO : Change this to correct scene
    public static void SwitchToSymbolEaterScene() => SceneManager.LoadScene(SceneNames.Eater);    
    // TODO : Change this to correct scene
    public static void SwitchToTowerScene() => SceneManager.LoadScene(SceneNames.Tower);
}

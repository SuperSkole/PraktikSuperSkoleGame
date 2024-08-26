using Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{   
    public void SwitchToMainWorld() => SceneManager.LoadScene(SceneNames.Main);
    public void SwitchToPlayerHouseScene() => SceneManager.LoadScene(SceneNames.House);
    public void SwitchToWordFactory() => SceneManager.LoadScene(SceneNames.Factory);
    // TODO : Change this when we have a racing scene
    public void SwitchToRacingScene() => SceneManager.LoadScene(SceneNames.House);
    // TODO : Change this to correct scene
    public void SwitchToSimpleEaterScene() => SceneManager.LoadScene(SceneNames.House);
    
    // TODO : Change this to correct scene
    public void SwitchToTowerScene() => SceneManager.LoadScene(SceneNames.House);

}

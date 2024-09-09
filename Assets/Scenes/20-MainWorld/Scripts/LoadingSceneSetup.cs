using System.Collections;
using System.Collections.Generic;
using Scenes;
using Scenes._50_Minigames.Gamemode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneSetup : MonoBehaviour
{
    [SerializeField]private int sceneID;

    /// <summary>
    /// Adds OnSceneLoaded to be run then a scene is loaded and begins loading the gamemode selector.
    /// </summary>
    public void Load()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SwitchScenes.SwitchToMinigameLoadingScene();
    }

    /// <summary>
    /// Finds the SetGameModeAndDestroy script and sets its sceneid. Removes itself as a method to be run on sceneload afterwards.
    /// </summary>
    /// <param name="scene">not used</param>
    /// <param name="loadSceneMode">not used</param>
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        GameObject gamemode = GameObject.FindGameObjectWithTag("GamemodeSelect");
        gamemode.GetComponent<SetGameModeAndDestroy>().sceneID = sceneID;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

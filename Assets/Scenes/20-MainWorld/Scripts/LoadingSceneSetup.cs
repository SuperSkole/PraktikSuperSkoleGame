using System.Collections;
using System.Collections.Generic;
using Scenes;
using Scenes._50_Minigames.Gamemode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneSetup : MonoBehaviour
{
    [SerializeField]private int sceneID;

    public void Load()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SwitchScenes.SwitchToMinigameLoadingScene();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        GameObject gamemode = GameObject.FindGameObjectWithTag("GamemodeSelect");
        gamemode.GetComponent<SetGameModeAndDestroy>().sceneID = sceneID;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

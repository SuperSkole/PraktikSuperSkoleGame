using System.Collections;
using System.Collections.Generic;
using CORE.Scripts.GameRules;
using Scenes.Minigames.MonsterTower;
using Scenes.Minigames.MonsterTower.Scrips;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scenes.Minigames.SymbolEater.Scripts.Gamemodes;
using Scenes.Minigames.SymbolEater.Scripts;
using CORE.Scripts;
using CORE.Scripts.GameRules;



public class SetGameModeAndDestroy : MonoBehaviour
{

    private IGenericGameMode gamemode;
    private IGameRules gameRule;
    //tells the scene manager to call the OnSceneLoaded function whenever a scene is loaded
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    //Finds an object with the tag "setup" in the scene, then runs the expected MiniGameSetup interface's method, then kills itself to avoid memory leak
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        GameObject parentToTarget = GameObject.FindGameObjectWithTag("Setup");
        IMinigameSetup target = parentToTarget.GetComponent<IMinigameSetup>();
        target.SetupGame(gamemode, gameRule);
        

        SceneManager.sceneLoaded -= OnSceneLoaded;

        Destroy(gameObject);
    }
    //sets a value so the OnSceneChange script can correctly determine which objects to look for and what modes to set
    public void Setgamemode(IGenericGameMode gamemodeID)
    {
        gamemode = gamemodeID;
    }
    public void SetGameRules(IGameRules gameRuleID)
    {
        gameRule = gameRuleID;
    }
}

using System.Collections;
using System.Collections.Generic;
using CORE.Scripts.GameRules;
using Scenes.Minigames.MonsterTower;
using Scenes.Minigames.MonsterTower.Scrips;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scenes.Minigames.SymbolEater.Scripts.Gamemodes;
using Scenes.Minigames.SymbolEater.Scripts;




public class SetGameModeAndDestroy : MonoBehaviour
{

    private int gamemode;
    //tells the scene manager to call the OnSceneLoaded function whenever a scene is loaded
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Sets some values in the new scene to change the gamemode, then kills itself to avoid memory leak
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        switch (gamemode)
        {
            //sets the gamemode to SpellWord
            case 1:
                BoardController target1 = FindObjectOfType<BoardController>();
                target1.GameModeSet(new SpellWordFromImage(), new SpellWord());
                break;
            //sets the gamemode to sound out letter
            case 2:
                BoardController target2 = FindObjectOfType<BoardController>();
                target2.GameModeSet(new FindImageFromSound(), new FindCorrectImage());
                break;
            //sets the gamemode to findnumberseries
            case 3:
                BoardController target3 = FindObjectOfType<BoardController>();
                target3.GameModeSet(new FindNumber(), new FindNumberSeries());
                break;

            case 4:
                MonsterTowerManager target4 = FindObjectOfType<MonsterTowerManager>();

                target4.difficulty = Difficulty.Easy;



                break;
                
            case 5:
                MonsterTowerManager target5 = FindObjectOfType<MonsterTowerManager>();

                target5.difficulty = Difficulty.Medium;

                break;

            case 6:
                MonsterTowerManager target6 = FindObjectOfType<MonsterTowerManager>();

                target6.difficulty = Difficulty.Hard;
                break;

            default:
                break;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;

        Destroy(gameObject);
    }
    //sets a value so the OnSceneChange script can correctly determine which objects to look for and what modes to set
    public void Setgamemode(int gamemodeID)
    {
        gamemode = gamemodeID;
    }
}

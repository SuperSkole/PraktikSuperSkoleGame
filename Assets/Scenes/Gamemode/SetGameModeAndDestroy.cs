using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                target1.GameModeSet(new SpellWord());
                break;
            //sets the gamemode to sound out letter
            case 2:
                BoardController target2 = FindObjectOfType<BoardController>();
                target2.GameModeSet(new FindSymbol());
                break;
            //sets the gamemode to findnumberseries
            case 3:
                BoardController target3 = FindObjectOfType<BoardController>();
                target3.GameModeSet(new FindNumber());
                break;

            case 4:

                break;
                
            case 5:

                break;

            default:
                break;
        }


        Destroy(gameObject);
    }
    //sets a value so the OnSceneChange script can correctly determine which objects to look for and what modes to set
    public void Setgamemode(int gamemodeID)
    {
        gamemode = gamemodeID;
    }
}

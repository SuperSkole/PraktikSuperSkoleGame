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
            //
            case 1:
                BoardController target = FindObjectOfType<BoardController>();
                target.GameModeSet(new SpellWord());
                break;

            case 2:

                break;

            case 3:

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
    
    public void Setgamemode(int gamemodeID)
    {
        gamemode = gamemodeID;
    }
}

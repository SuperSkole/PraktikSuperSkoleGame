using Scenes;
using Scenes._50_Minigames._54_SymbolEater.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToMainWorld : MonoBehaviour
{
    public void ChangeSceen()
    {
        FindAnyObjectByType<SymbolEaterPlayer>().GameOver();
        SwitchScenes.SwitchToMainWorld();
    }
}

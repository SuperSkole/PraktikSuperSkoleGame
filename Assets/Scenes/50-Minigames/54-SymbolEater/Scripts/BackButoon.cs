using Scenes.Minigames.SymbolEater.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class BackButoon : MonoBehaviour
{
    public void GotToMain()
    {
        FindAnyObjectByType<SymbolEaterPlayer>().GameOver();
        SwitchScenes.SwitchToMainWorld();
    }
}

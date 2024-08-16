using System.Collections.Generic;
using CORE;
using Player;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CharacterVisuelManagement : MonoBehaviour
{
    public GameObject characterHead;
    public GameObject characterBody;
    public GameObject characterLeg;

    //public List<Color> characterHeadColor = new List<Color>() { };

    //public List<Color> characterBodyColor = new List<Color>() { };

    //public List<Color> characterLegColor = new List<Color>() { };
    PlayerData player;

    //public Color CurrentHeadColor;
    //public Color CurrentBodyColor;
    //public Color CurrentLegColor;

    //public string playerName;
    //public characterManagement(string playerName, Color CurrentHeadColor, Color CurrentBodyColor, Color CurrentLegColor)
    //{
    //    this.playerName = playerName;
    //    this.CurrentHeadColor = CurrentHeadColor;
    //    this.CurrentBodyColor = CurrentBodyColor;
    //    this.CurrentLegColor = CurrentLegColor;
    //}

    public void JustCreatedChar(GameObject gm)
    {
        player = gm.GetComponent<OldGameManager>().ReturnPlayer();

        characterHead.GetComponent<SpriteRenderer>().color = player.currentHeadColor;
        characterBody.GetComponent<SpriteRenderer>().color = player.currentBodyColor;
        characterLeg.GetComponent<SpriteRenderer>().color = player.currentLegColor; 
        
        characterHead.GetComponent<SpriteRenderer>().sprite = player.spriteHead;
        characterBody.GetComponent<SpriteRenderer>().sprite = player.spriteBody;
        characterLeg.GetComponent<SpriteRenderer>().sprite = player.spriteLeg;
    }

}

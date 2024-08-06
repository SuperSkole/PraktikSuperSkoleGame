using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class characterManagement : MonoBehaviour
{
    public GameObject characterHead;
    public GameObject characterBody;
    public GameObject characterLeg;

    //public List<Color> characterHeadColor = new List<Color>() { };

    //public List<Color> characterBodyColor = new List<Color>() { };

    //public List<Color> characterLegColor = new List<Color>() { };
    CharacterController player;

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
        player = gm.GetComponent<NewGame>().ReturnPlayer();

        characterHead.GetComponent<SpriteRenderer>().color = player.CurrentHeadColor;
        characterBody.GetComponent<SpriteRenderer>().color = player.CurrentBodyColor;
        characterLeg.GetComponent<SpriteRenderer>().color = player.CurrentLegColor; 
        
        characterHead.GetComponent<SpriteRenderer>().sprite = player.spriteHead;
        characterBody.GetComponent<SpriteRenderer>().sprite = player.spriteBody;
        characterLeg.GetComponent<SpriteRenderer>().sprite = player.spriteLeg;
    }

}

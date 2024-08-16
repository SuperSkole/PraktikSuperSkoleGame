using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetCuzCurrentColor : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerParts = new List<GameObject>();
    [SerializeField] private List<GameObject> cuz = new List<GameObject>();
    PlayerData p;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private GameObject gamemanger;
    [SerializeField] private GameObject GetBodyParts;

    //   [SerializeField] private List<UnityEvent> events = new List<UnityEvent>();
    private List<Color> colorsHead = new List<Color>();
    private List<Color> colorsBody = new List<Color>();
    private List<Color> colorsLeg = new List<Color>();
    private int headI = 0;
    private int BodyI = 0;
    private int LegI = 0;

    // Start is called before the first frame update
    
    private void Start()
    {
        cuz[0].GetComponent<Image>().color = playerParts[0].GetComponent<SpriteRenderer>().color;
        cuz[1].GetComponent<Image>().color = playerParts[1].GetComponent<SpriteRenderer>().color;
        cuz[2].GetComponent<Image>().color = playerParts[2].GetComponent<SpriteRenderer>().color;
        //colorsHead = player.GetComponent<characterManagement>().characterHeadColor;
        //colorsBody = player.GetComponent<characterManagement>().characterBodyColor;
        //colorsLeg = player.GetComponent<characterManagement>().characterLegColor;
    }
    
    public void OpeningScreen()
    {
        //colorsHead = player.GetComponent<characterManagement>().characterHeadColor;
        //colorsBody = player.GetComponent<characterManagement>().characterBodyColor;
        //colorsLeg = player.GetComponent<characterManagement>().characterLegColor;
        PlayerData tmp = gamemanger.GetComponent<GameManager>().ReturnPlayer();
        nameField.text = tmp.playerName;
    }
    //public void BackButtonPressed()
    //{
    //    p = gamemanger.GetComponent<NewGame>().ReturnPlayer();

    //    p.spriteHead = GetBodyParts.GetComponent<ShowCusBodyParts>().playerHead.GetComponent<Image>().sprite;
    //    p.spriteBody = GetBodyParts.GetComponent<ShowCusBodyParts>().playerBody.GetComponent<Image>().sprite;
    //    p.spriteLeg = GetBodyParts.GetComponent<ShowCusBodyParts>().playerLegs.GetComponent<Image>().sprite;

    //    player.GetComponent<characterManagement>().characterHead.GetComponent<SpriteRenderer>().sprite
    //        = GetBodyParts.GetComponent<ShowCusBodyParts>().playerHead.GetComponent<Image>().sprite;
    //    //player.GetComponent<characterManagement>().characterHead.GetComponent<Transform>().localScale = new Vector3 (0.1f, 0.1f, 0.1f);

    //    player.GetComponent<characterManagement>().characterBody.GetComponent<SpriteRenderer>().sprite 
    //        = GetBodyParts.GetComponent<ShowCusBodyParts>().playerBody.GetComponent<Image>().sprite;
    //    //player.GetComponent<characterManagement>().characterBody.GetComponent<Transform>().localScale = new Vector3(0.12f, 0.08f, 0.1f);

    //    player.GetComponent<characterManagement>().characterLeg.GetComponent<SpriteRenderer>().sprite 
    //        = GetBodyParts.GetComponent<ShowCusBodyParts>().playerLegs.GetComponent<Image>().sprite;
    //    //player.GetComponent<characterManagement>().characterLeg.GetComponent<Transform>().localScale = new Vector3(0.1f, 0.06f, 0.1f);
    //}
    ////For switching the color of the players body part
    public void HeadColorLeft()
    {
        headI--;
        if (headI < 0)
        {
            headI = colorsHead.Count - 1;
        }
        cuz[0].GetComponent<Image>().color = colorsHead[headI];
        player.GetComponent<CharacterVisuelManagement>().characterHead.GetComponent<SpriteRenderer>().color = colorsHead[headI];
        gamemanger.GetComponent<GameManager>().headColor = colorsHead[headI];
    }
    public void HeadColorRight()
    {
        headI++;
        if (headI >= colorsHead.Count)
        {
            headI = 0;
        }
        cuz[0].GetComponent<Image>().color = colorsHead[headI];
        player.GetComponent<CharacterVisuelManagement>().characterHead.GetComponent<SpriteRenderer>().color = colorsHead[headI];
        gamemanger.GetComponent<GameManager>().headColor = colorsHead[headI];

    }
    public void BodyColorLeft()
    {
        BodyI--;
        if (BodyI < 0)
        {
            BodyI = colorsBody.Count - 1;
        }
        cuz[1].GetComponent<Image>().color = colorsBody[BodyI];
        player.GetComponent<CharacterVisuelManagement>().characterBody.GetComponent<SpriteRenderer>().color = colorsBody[BodyI];
        gamemanger.GetComponent<GameManager>().BodyColor = colorsBody[BodyI];
    }
    public void BodyColorRight()
    {
        BodyI++;
        if (BodyI >= colorsBody.Count)
        {
            BodyI = 0;
        }
        cuz[1].GetComponent<Image>().color = colorsHead[BodyI];
        player.GetComponent<CharacterVisuelManagement>().characterBody.GetComponent<SpriteRenderer>().color = colorsBody[BodyI];
        gamemanger.GetComponent<GameManager>().BodyColor = colorsBody[BodyI];

    }
    public void LegColorLeft()
    {
        LegI--;
        if (LegI < 0)
        {
            LegI = colorsLeg.Count - 1;
        }
        cuz[2].GetComponent<Image>().color = colorsLeg[LegI];
        player.GetComponent<CharacterVisuelManagement>().characterLeg.GetComponent<SpriteRenderer>().color = colorsLeg[LegI];
        gamemanger.GetComponent<GameManager>().LegColor = colorsLeg[LegI];
    }
    public void LegColorRight()
    {
        LegI++;
        if (LegI >= colorsLeg.Count)
        {
            LegI = 0;
        }
        cuz[2].GetComponent<Image>().color = colorsLeg[LegI];
        player.GetComponent<CharacterVisuelManagement>().characterLeg.GetComponent<SpriteRenderer>().color = colorsLeg[LegI];
        gamemanger.GetComponent<GameManager>().LegColor = colorsLeg[LegI];

    }





}

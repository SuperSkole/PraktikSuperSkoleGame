using Cinemachine;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChosewhichPlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerPreview;
    private string avatarName;
    [SerializeField] private GameObject CharCreate;
    [SerializeField] private Transform SpawnCharPoint;
    private GameObject SpawnCharObj;
    [FormerlySerializedAs("gmNewGame")] [SerializeField] private GameManager gmGameManager;
    [SerializeField] private ShopSkinManagement gmSkins;
    public void ChangeImageOfPlayer(GameObject icon)
    {
        playerPreview.GetComponent<Image>().sprite = icon.GetComponent<Image>().sprite;
    }
    //Gets called on the buttons in begning of the character chooser
    public void GivePlayerStringInfo(string monsterName)
    {
        avatarName = monsterName;
    }
    public void ChoosenPlayerClicked()
    {
        //Sets the starting skins
        switch (avatarName)
        {
            case "Girl":
                gmSkins.girlSkins.head[0].purchased = true;
                gmSkins.girlSkins.head[0].equipped= true;
                gmSkins.girlSkins.torso[0].purchased = true;
                gmSkins.girlSkins.torso[0].equipped= true;
                gmSkins.girlSkins.legs[0].purchased = true;
                gmSkins.girlSkins.legs[0].equipped= true;

                break;
            case "SimpleMonster":
                gmSkins.monsterSkins.head[0].purchased = true;
                gmSkins.monsterSkins.head[0].equipped = true;
                gmSkins.monsterSkins.torso[0].purchased = true;
                gmSkins.monsterSkins.torso[0].equipped = true;
                gmSkins.monsterSkins.legs[0].purchased = true;
                gmSkins.monsterSkins.legs[0].equipped = true;

                break;
        }

        gmGameManager.SetMonsterName(avatarName);
        Instantiate(SpawnCharObj, SpawnCharPoint);
        
        //CharCreate.GetComponent<SeeChoosenPlayer>().TakeINFO(avatarName);
    }
    public void GiveCharPrefab(GameObject type)
    {
        SpawnCharObj = type;
    }
}

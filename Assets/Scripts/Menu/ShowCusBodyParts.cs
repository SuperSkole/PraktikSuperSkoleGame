using System;
using System.Collections.Generic;
using CORE;
using UnityEngine;
using UnityEngine.UI;

public class ShowCusBodyParts : MonoBehaviour
{

    [SerializeField] private GameObject gm;
    [SerializeField] private IconSpriteMapper mapper;
    private GameObject playerHead;
    private GameObject playerBody;
    private GameObject playerLegs;

    [SerializeField] private Sprite notEquippedSkin;
    [SerializeField] private Sprite equippedSkin;

    private string whichChar;


    bool HeadClicked = true;
    bool BodyClicked = false;
    bool LegClicked = false;

    private bool WorldLoadedIn = false;
    private void OnEnable()
    {
        GetPlayerPartsInfo();
        if (WorldLoadedIn)
        {
            SeeHeadParts();
        }
        WorldLoadedIn = true;
    }
    public void GetPlayerPartsInfo()
    {
        GameObject head = GameObject.Find("Head");
        GameObject torso = GameObject.Find("Mainbody");
        GameObject legs = GameObject.Find("Legs");

        if (head != null)
        {
            playerHead = head;
        }
        else
        {
            Debug.Log("ShowCusBodyParts/GetPlayerPartsInfo/No Head was found");
        }
        if (torso != null)
        {
            playerBody = torso;
        }
        else
        {
            Debug.Log("ShowCusBodyParts/GetPlayerPartsInfo/No Torso was found");
        }
        if (legs != null)
        {
            playerLegs = legs;
        }
        else
        {
            Debug.Log("ShowCusBodyParts/GetPlayerPartsInfo/No Legs was found");
        }

    }


    public void GiveNameToChar(string name)
    {
        whichChar = name;
    }
    private void GetPlayerNameFromSaveFile()
    {
        whichChar = gm.GetComponent<OldGameManager>().save.MonsterName;
        whichChar += "(Clone)";
    }
    //For displaying all the different parts that the player can equip
    public void SeeHeadParts()
    {
        //Must be placed before any logics are carried out
        if (whichChar == null)
        {
            GetPlayerNameFromSaveFile();
        }
        // var HeadParts = gm.GetComponent<ShopSkinManagement>().HeadParts;
        List<Skins> HeadParts = new List<Skins>();
        try
        {
            switch (whichChar)
            {
                case "Girl(Clone)":
                    HeadParts = gm.GetComponent<ShopSkinManagement>().girlSkins.head;
                    break;
                case "SimpleMonster(Clone)":
                    HeadParts = gm.GetComponent<ShopSkinManagement>().monsterSkins.head;
                    break;
            }
        }
        catch (Exception)
        {
            Debug.Log("Cant find the skins");
        }
        //var AnimHeadParts = gm.GetComponent<ShopSkinManagement>().ActuelHeadParts;
        HeadClicked = true;
        BodyClicked = false;
        LegClicked = false;
        for (int i = 0; i <= 5; i++)
        {
            // Construct the name of the child GameObject
            string childName = "Box (" + (i + 1) + ")";

            // Find the child GameObject by name
            Transform child = this.transform.Find(childName);
            // Find the Img child GameObject of the Box GameObject
            Transform imgChild = child.Find("Img");

            Image tmp = imgChild.GetComponent<Image>();
           
            //Color Doenst change like it should
            //Changes the background of the icon that is equppied
            if (HeadParts[i].equipped == true)
            {
                child.GetComponent<Image>().sprite = equippedSkin;
                child.GetComponent<Image>().color = new Color(35f / 255f, 118f / 255f, 24f / 255f);
            }
            else
            {
                child.GetComponent<Image>().sprite = notEquippedSkin;
                child.GetComponent<Image>().color = new Color(173f / 255f, 173f / 255f, 173f / 255f);
            }
            if (HeadParts[i].purchased == true)
            {
                child.GetComponent<Button>().enabled = true;
                //tmp.sprite = AnimHeadParts.parts[i];

                tmp.sprite = HeadParts[i].skin;

                Color tmpColor = tmp.color;
                tmpColor.a = 1.0f;
                tmp.color = tmpColor;
            }
            else//Not Bought
            {
                child.GetComponent<Button>().enabled = false;

                tmp.sprite = null;
                Color tmpColor = tmp.color;
                tmpColor.a = 0f;
                tmp.color = tmpColor;
            }

            Button butTmp = child.GetComponent<Button>();
            if (butTmp != null)
            {
                //Removes old listners if not done, errors will happen.
                butTmp.onClick.RemoveAllListeners();
                //adds a listener for specefik body part
                butTmp.onClick.AddListener(() => ChangeHeadPart(tmp));
            }
        }
    }
    public void SeeBodyParts()
    {
        //Must be placed before any logics are carried out
        if (whichChar == null)
        {
            GetPlayerNameFromSaveFile();
        }
        //var AnimBodyParts = gm.GetComponent<ShopSkinManagement>().ActuelBodyParts;
        List<Skins> BodyParts = new List<Skins>();
        try
        {
            switch (whichChar)
            {
                case "Girl(Clone)":
                    BodyParts = gm.GetComponent<ShopSkinManagement>().girlSkins.torso;
                    break;
                case "SimpleMonster(Clone)":
                    BodyParts = gm.GetComponent<ShopSkinManagement>().monsterSkins.torso;
                    break;
            }
        }
        catch (Exception)
        {
            Debug.Log("Cant find the skins");
        }
        HeadClicked = false;
        BodyClicked = true;
        LegClicked = false;
        for (int i = 0; i <= 5; i++)
        {
            // Construct the name of the child GameObject
            string childName = "Box (" + (i + 1) + ")";

            // Find the child GameObject by name
            Transform child = this.transform.Find(childName);
            // Find the Img child GameObject of the Box GameObject
            Transform imgChild = child.Find("Img");

            Image tmp = imgChild.GetComponent<Image>();
            //Sprite realSprite = imgChild.GetComponent<Image>().sprite;
            if (tmp != null)
            {
                //Color Doenst change like it should
                //Changes the background of the icon that is equppied
                if (BodyParts[i].equipped == true)
                {
                    child.GetComponent<Image>().sprite = equippedSkin;
                    child.GetComponent<Image>().color = new Color(35f / 255f, 118f / 255f, 24f / 255f);
                }
                else
                {
                    child.GetComponent<Image>().sprite = notEquippedSkin;
                    child.GetComponent<Image>().color = new Color(173f / 255f, 173f / 255f, 173f / 255f);
                }
                //Debug.Log("HeadParts count: " + BodyParts.Count);
                if (BodyParts[i].purchased == true)
                {
                    child.GetComponent<Button>().enabled = true;
                    //realSprite = AnimBodyParts.parts[i];

                    tmp.sprite = BodyParts[i].skin;

                    Color tmpColor = tmp.color;
                    tmpColor.a = 1.0f;
                    tmp.color = tmpColor;
                }
                else//Not bought
                {
                    child.GetComponent<Button>().enabled = false;

                    tmp.sprite = null;
                    Color tmpColor = tmp.color;
                    tmpColor.a = 0f;
                    tmp.color = tmpColor;
                }
            }
            Button butTmp = child.GetComponent<Button>();
            if (butTmp != null)
            {
                //Removes old listners if not done, errors will happen.
                butTmp.onClick.RemoveAllListeners();
                //adds a listener for specefik body part
                butTmp.onClick.AddListener(() => ChangeBodyPart(tmp));
            }
        }
    }
    public void SeeLegsParts()
    {
        //Must be placed before any logics are carried out
        if (whichChar == null)
        {
            GetPlayerNameFromSaveFile();
        }
        //var AnimLegParts = gm.GetComponent<ShopSkinManagement>().ActuelLegParts;
        List<Skins> LegParts = new List<Skins>();
        try
        {
            switch (whichChar)
            {
                case "Girl(Clone)":
                    LegParts = gm.GetComponent<ShopSkinManagement>().girlSkins.legs;
                    break;
                case "SimpleMonster(Clone)":
                    LegParts = gm.GetComponent<ShopSkinManagement>().monsterSkins.legs;
                    break;
            }
        }
        catch (Exception)
        {
            Debug.Log("Cant find the skins");
        }
        HeadClicked = false;
        BodyClicked = false;
        LegClicked = true;
        for (int i = 0; i <= 5; i++)
        {
            // Construct the name of the child GameObject
            string childName = "Box (" + (i + 1) + ")";

            // Find the child GameObject by name
            Transform child = this.transform.Find(childName);
            // Find the Img child GameObject of the Box GameObject
            Transform imgChild = child.Find("Img");

            Image tmp = imgChild.GetComponent<Image>();
            //Color Doenst change like it should
            //Changes the background of the icon that is equppied
            if (LegParts[i].equipped == true)
            {
                child.GetComponent<Image>().sprite = equippedSkin;
                child.GetComponent<Image>().color = new Color(35f / 255f, 118f / 255f, 24f / 255f);
            }
            else
            {
                child.GetComponent<Image>().sprite = notEquippedSkin;
                child.GetComponent<Image>().color = new Color(173f / 255f, 173f / 255f, 173f / 255f);
            }

            if (LegParts[i].purchased == true)
            {
                child.GetComponent<Button>().enabled = true;
                //realSprite = AnimLegParts.parts[i];

                tmp.sprite = LegParts[i].skin;

                Color tmpColor = tmp.color;
                tmpColor.a = 1.0f;
                tmp.color = tmpColor;
            }
            else//Not Bought
            {
                child.GetComponent<Button>().enabled = false;

                tmp.sprite = null;
                Color tmpColor = tmp.color;
                tmpColor.a = 0f;
                tmp.color = tmpColor;
            }

            Button butTmp = child.GetComponent<Button>();
            if (butTmp != null)
            {
                //Removes old listners if not done, errors will happen.
                butTmp.onClick.RemoveAllListeners();
                //adds a listener for specefik body part
                butTmp.onClick.AddListener(() => ChangelegPart(tmp));
            }
        }
    }


    //For changing the character parts
    public void ChangeHeadPart(Image im)
    {
        try
        {
            switch (gm.GetComponent<OldGameManager>().save.MonsterName)
            {
                case "Girl":
                    Debug.Log("ShowCusBodyParts/ChangeHeadPart/Head Sprite has changed");
                    playerHead.GetComponent<SpriteRenderer>().sprite = mapper.GetSpriteFromIcon(im.sprite);
                    foreach (var item in gm.GetComponent<ShopSkinManagement>().girlSkins.head)
                    {
                        item.equipped = false;
                        if (im.sprite.name == item.skin.name)
                        {
                            item.equipped = true;
                        }
                    }
                    break;
                case "SimpleMonster":
                    Debug.Log("ShowCusBodyParts/ChangeHeadPart/Head Sprite has changed");
                    playerHead.GetComponent<SpriteRenderer>().sprite = mapper.GetSpriteFromIcon(im.sprite);
                    foreach (var item in gm.GetComponent<ShopSkinManagement>().monsterSkins.head)
                    {
                        item.equipped = false;
                        if (im.sprite.name == item.skin.name)
                        {
                            item.equipped = true;
                        }
                    }
                    break;
            }

            SeeHeadParts();
        }
        catch (Exception) { Debug.Log("ShowCusBodyParts/ChangeHeadPart/An Error with changing heads!"); }

    }
    public void ChangeBodyPart(Image im)
    {
        try
        {
            switch (gm.GetComponent<OldGameManager>().save.MonsterName)
            {
                case "Girl":
                    Debug.Log("ShowCusBodyParts/ChangeBodyPart/Body Sprite has changed");
                    playerBody.GetComponent<SpriteRenderer>().sprite = mapper.GetSpriteFromIcon(im.sprite);
                    foreach (var item in gm.GetComponent<ShopSkinManagement>().girlSkins.torso)
                    {
                        item.equipped = false;
                        if (im.sprite.name == item.skin.name)
                        {
                            item.equipped = true;
                        }
                    }
                    break;
                case "SimpleMonster":
                    Debug.Log("ShowCusBodyParts/ChangeBodyPart/Body Sprite has changed");
                    playerBody.GetComponent<SpriteRenderer>().sprite = mapper.GetSpriteFromIcon(im.sprite);
                    foreach (var item in gm.GetComponent<ShopSkinManagement>().monsterSkins.torso)
                    {
                        item.equipped = false;
                        if (im.sprite.name == item.skin.name)
                        {
                            item.equipped = true;
                        }
                    }
                    break;
            }
            SeeBodyParts();
        }
        catch (Exception) { Debug.Log("ShowCusBodyParts/ChangeBodyPart/An Error with changing Body!"); }
    }
    public void ChangelegPart(Image im)
    {
        try
        {
            switch (gm.GetComponent<OldGameManager>().save.MonsterName)
            {
                case "Girl":
                    Debug.Log("ShowCusBodyParts/ChangelegPart/Leg sprite has changed");
                    playerLegs.GetComponent<SpriteRenderer>().sprite = mapper.GetSpriteFromIcon(im.sprite);
                    foreach (var item in gm.GetComponent<ShopSkinManagement>().girlSkins.legs)
                    {
                        item.equipped = false;
                        if (im.sprite.name == item.skin.name)
                        {
                            item.equipped = true;
                        }
                    }
                    break;
                case "SimpleMonster":
                    Debug.Log("ShowCusBodyParts/ChangelegPart/Leg sprite has changed");
                    playerLegs.GetComponent<SpriteRenderer>().sprite = mapper.GetSpriteFromIcon(im.sprite);
                    foreach (var item in gm.GetComponent<ShopSkinManagement>().monsterSkins.legs)
                    {
                        item.equipped = false;
                        if (im.sprite.name == item.skin.name)
                        {
                            item.equipped = true;
                        }
                    }
                    break;
            }
            SeeLegsParts();
        }
        catch (Exception) { Debug.Log("ShowCusBodyParts/ChangelegPart/An Erro with chaning legs!"); }
    }
}

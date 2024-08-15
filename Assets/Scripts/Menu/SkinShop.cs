using System;
using System.Collections.Generic;
using CORE;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinShop : MonoBehaviour
{
    #region feilds
    [SerializeField] private GameObject gm;
    [SerializeField] private IconSpriteMapper mapper;

    private GameObject playerHead;
    private GameObject playerBody;
    private GameObject playerLegs;

    [SerializeField] private GameObject purchaseBut;
    [SerializeField] private TextMeshProUGUI purchaseText;
    [SerializeField] private GameObject skinShopGO;
    [SerializeField] private GameObject priceForSkin;

    [SerializeField] private Sprite notEquippedSkin;
    [SerializeField] private Sprite equippedSkin;

    private Sprite originalHeadSprite;
    private Sprite originalBodySprite;
    private Sprite originalLegSprite;

    private string whichChar;

    bool HeadClicked = true;
    bool BodyClicked = false;
    bool LegClicked = false;
    #endregion
    public void OnEnable()
    {
        GetPlayerPartsInfo();
        try
        {
            originalHeadSprite = mapper.GetIconFromSprite(playerHead.GetComponent<SpriteRenderer>().sprite);
            originalBodySprite = mapper.GetIconFromSprite(playerBody.GetComponent<SpriteRenderer>().sprite);
            originalLegSprite = mapper.GetIconFromSprite(playerLegs.GetComponent<SpriteRenderer>().sprite);
            SeeHeadParts();
        }
        catch { }

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
            Debug.Log("SkinShop/GetPlayerPartsInfo/No Head was found");
        }
        if (torso != null)
        {
            playerBody = torso;
        }
        else
        {
            Debug.Log("SkinShop/GetPlayerPartsInfo/No Torso was found");
        }
        if (legs != null)
        {
            playerLegs = legs;
        }
        else
        {
            Debug.Log("SkinShop/GetPlayerPartsInfo/No Legs was found");
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
        HeadClicked = true;
        BodyClicked = false;
        LegClicked = false;


        for (int i = 0; i <= 5; i++)
        {
            //Name of the child GameObject
            string childName = "Box (" + (i + 1) + ")";

            // Find the child GameObject by name
            Transform child = this.transform.Find(childName);
            // Find the Img child GameObject of the Box GameObject
            Transform imgChild = child.Find("Img");
            Transform priceChild = priceForSkin.transform.Find("Price");

            TextMeshProUGUI price = priceChild.GetComponent<TextMeshProUGUI>();
            Image tmp = imgChild.GetComponent<Image>();

            // Debug.Log("HeadParts count: " + HeadParts.Count + " Which index " + i);

            tmp.sprite = HeadParts[i].skin;
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
                Color tmpColor = tmp.color;
                tmpColor.a = 1.0f;
                tmp.color = tmpColor;
            }
            else// Skin not bought
            {
                Color tmpColor = tmp.color;
                tmpColor.a = 0.5f;
                tmp.color = tmpColor;
            }


            Button butTmp = child.GetComponent<Button>();
            if (butTmp != null)
            {
                try
                {
                    int currentIndex = i;
                    //Removes old listners if not done, errors will happen.
                    butTmp.onClick.RemoveAllListeners();
                    //adds a listener for specefik body part
                    butTmp.onClick.AddListener(() => ChangeHeadPart(tmp, HeadParts, currentIndex, price));
                    butTmp.onClick.AddListener(() => HaveclickedHeadParts(currentIndex));
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
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
            Transform priceChild = priceForSkin.transform.Find("Price");

            // Find the Img child GameObject of the Box GameObject
            Transform imgChild = child.Find("Img");

            TextMeshProUGUI price = priceChild.GetComponent<TextMeshProUGUI>();
            Image tmp = imgChild.GetComponent<Image>();

            tmp.sprite = BodyParts[i].skin;

            //Color Doenst change like it should
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
            if (tmp != null)
            {
                if (BodyParts[i].purchased == true)
                {
                    child.GetComponent<Button>().enabled = true;
                    Color tmpColor = tmp.color;
                    tmpColor.a = 1.0f;
                    tmp.color = tmpColor;
                }
                else// Skin not bought
                {
                    Color tmpColor = tmp.color;
                    tmpColor.a = 0.5f;
                    tmp.color = tmpColor;
                }
            }
            Button butTmp = child.GetComponent<Button>();
            if (butTmp != null)
            {
                int currentIndex = i;
                //Removes old listners if not done, errors will happen.
                butTmp.onClick.RemoveAllListeners();
                //adds a listener for specefik body part
                butTmp.onClick.AddListener(() => ChangeBodyPart(tmp, BodyParts, currentIndex, price));
                butTmp.onClick.AddListener(() => HaveclickedBodyParts(currentIndex));
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
            Transform priceChild = priceForSkin.transform.Find("Price");

            // Find the Img child GameObject of the Box GameObject
            Transform imgChild = child.Find("Img");

            TextMeshProUGUI price = priceChild.GetComponent<TextMeshProUGUI>();
            Image tmp = imgChild.GetComponent<Image>();

            tmp.sprite = LegParts[i].skin;

            //Color Doenst change like it should
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
                Color tmpColor = tmp.color;
                tmpColor.a = 1.0f;
                tmp.color = tmpColor;
            }
            else// Skin not bought
            {
                Color tmpColor = tmp.color;
                tmpColor.a = 0.5f;
                tmp.color = tmpColor;
            }

            Button butTmp = child.GetComponent<Button>();
            if (butTmp != null)
            {
                int currentIndex = i;
                //Removes old listners if not done, errors will happen.
                butTmp.onClick.RemoveAllListeners();
                //adds a listener for specefik body part
                butTmp.onClick.AddListener(() => ChangelegPart(tmp, LegParts, currentIndex, price));
                butTmp.onClick.AddListener(() => HaveclickedLegsParts(currentIndex));
            }
        }
    }

    //For changing the character parts
    public void ChangeHeadPart(Image im, List<Skins> HeadParts, int index, TextMeshProUGUI price)
    {
        try
        {
            Debug.Log("SkinShop/ChangeHeadPart/ChangedHead");
            if (HeadParts[index].purchased == true)
            {
                foreach (var item in HeadParts)
                {
                    item.equipped = false;
                }
                HeadParts[index].equipped = true;
                priceForSkin.SetActive(false);
            }
            else
            {
                priceForSkin.SetActive(true);
                price.text = HeadParts[index].price.ToString();
            }
            playerHead.GetComponent<SpriteRenderer>().sprite = mapper.GetSpriteFromIcon(im.sprite);
            SeeHeadParts();
        }
        catch (Exception) { Debug.Log("SkinShop/ChangeHeadPart/An Error with changing heads!"); }

    }
    public void ChangeBodyPart(Image im, List<Skins> BodyParts, int index, TextMeshProUGUI price)
    {
        try
        {
            Debug.Log("ChangedBody");
            if (BodyParts[index].purchased == true)
            {
                foreach (var item in BodyParts)
                {
                    item.equipped = false;
                }
                BodyParts[index].equipped = true;
                priceForSkin.SetActive(false);
            }
            else
            {
                priceForSkin.SetActive(true);
                price.text = BodyParts[index].price.ToString();
            }
            playerBody.GetComponent<SpriteRenderer>().sprite = mapper.GetSpriteFromIcon(im.sprite);
            SeeBodyParts();
        }
        catch (Exception) { Debug.Log("SkinShop/ChangeBodyPart/An Error with changing Body!"); }

    }
    public void ChangelegPart(Image im, List<Skins> LegParts, int index, TextMeshProUGUI price)
    {
        try
        {
            Debug.Log("ChangedLeg");
            if (LegParts[index].purchased == true)
            {
                foreach (var item in LegParts)
                {
                    item.equipped = false;
                }
                LegParts[index].equipped = true;
                priceForSkin.SetActive(false);
            }
            else
            {
                priceForSkin.SetActive(true);
                price.text = LegParts[index].price.ToString();
            }
            playerLegs.GetComponent<SpriteRenderer>().sprite = mapper.GetSpriteFromIcon(im.sprite);
            SeeLegsParts();
        }
        catch (Exception) { Debug.Log("SkinShop/ChangelegPart/An Error with changing Legs!"); }

    }
    public void DisableSkinShop()
    {
        skinShopGO.SetActive(false);
        PlayerWorldMovement.allowedToMove = true;
    }
    public void EnableSkinShop()
    {
        skinShopGO.SetActive(true);
        PlayerWorldMovement.allowedToMove = false;
    }
    #region Change Colors
    private void ChangeColorHavePruchase()
    {
        Color color = purchaseBut.GetComponent<Image>().color;
        color.a = 0.50f;
        purchaseBut.GetComponent<Image>().color = color;
        var butText = purchaseText.GetComponent<TextMeshProUGUI>();
        color = Color.white;
        color.a = 0.25f;
        butText.color = color;
    }
    private void ChangeColorHaveNotPruchase()
    {
        Color color = purchaseBut.GetComponent<Image>().color;
        color.a = 1f;
        purchaseBut.GetComponent<Image>().color = color;
        var butText = purchaseText.GetComponent<TextMeshProUGUI>();
        color = Color.white;
        butText.color = color;
    }
    #endregion
    /// <summary>
    /// Call when player clicks on one of the Head skins
    /// </summary>
    /// <param name="index"></param>
    public void HaveclickedHeadParts(int index)
    {
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
            Debug.Log("SkinShop/HaveclickedHeadParts/Cant find the skins");
        }


        if (HeadParts[index].purchased == true)
        {
            purchaseBut.GetComponent<Button>().onClick.RemoveAllListeners();
            ChangeColorHavePruchase();
        }
        else
        {
            ChangeColorHaveNotPruchase();
            purchaseBut.GetComponent<Button>().onClick.RemoveAllListeners();
            if (gm.GetComponent<GeneralManagement>().CanAfford(HeadParts[index].price))
            {
                purchaseBut.GetComponent<Button>().onClick.AddListener(() =>
                {
                    bool bought = gm.GetComponent<GeneralManagement>().RemoveGold(HeadParts[index].price);
                    if (bought)
                    {
                        //BUY it 
                        HeadParts[index].purchased = true;
                        SeeHeadParts();
                    }
                    else
                    {
                        Debug.Log("SkinsShop/HaveclickedHeadParts/Something went wrong");
                    }
                });
            }
            else
            {
                Debug.Log("SkinsShop/HaveclickedHeadParts/Cant afford");
            }
        }
    }
    /// <summary>
    /// Call when player clicks on one of the Body Skins
    /// </summary>
    /// <param name="index"></param>
    public void HaveclickedBodyParts(int index)
    {
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
        Debug.Log(index);
        if (BodyParts[index].purchased == true)
        {
            purchaseBut.GetComponent<Button>().onClick.RemoveAllListeners();
            ChangeColorHavePruchase();
        }
        else
        {
            ChangeColorHaveNotPruchase();
            purchaseBut.GetComponent<Button>().onClick.RemoveAllListeners();
            if (gm.GetComponent<GeneralManagement>().CanAfford(BodyParts[index].price))
            {
                purchaseBut.GetComponent<Button>().onClick.AddListener(() =>
                {
                    bool bought = gm.GetComponent<GeneralManagement>().RemoveGold(BodyParts[index].price);
                    if (bought)
                    {
                        //BUY it 
                        BodyParts[index].purchased = true;
                        SeeBodyParts();
                    }
                    else
                    {
                        Debug.Log("SkinsShop/HaveclickedBodyParts/Something went wrong");
                    }
                });
            }
            else
            {
                Debug.Log("SkinsShop/HaveclickedBodyParts/Cant afford");
            }
        }
    }
    /// <summary>
    /// Call when player clicks on one of the Leg skins
    /// </summary>
    /// <param name="index"></param>
    public void HaveclickedLegsParts(int index)
    {
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
        Debug.Log(index);
        if (LegParts[index].purchased == true)
        {
            purchaseBut.GetComponent<Button>().onClick.RemoveAllListeners();
            ChangeColorHavePruchase();
        }
        else
        {
            ChangeColorHaveNotPruchase();
            purchaseBut.GetComponent<Button>().onClick.RemoveAllListeners();
            if (gm.GetComponent<GeneralManagement>().CanAfford(LegParts[index].price))
            {
                purchaseBut.GetComponent<Button>().onClick.AddListener(() =>
                {
                    bool bought = gm.GetComponent<GeneralManagement>().RemoveGold(LegParts[index].price);
                    if (bought)
                    {
                        //BUY it 
                        LegParts[index].purchased = true;
                        SeeLegsParts();
                    }
                    else
                    {
                        Debug.Log("SkinsShop/HaveclickedLegsParts/Something went wrong");


                    }
                });
            }
            else
            {
                Debug.Log("SkinsShop/HaveclickedLegsParts/Cant afford");
            }
        }
    }

    /// <summary>
    /// Checks to see if the player has unbought skins on when leaving the store, and reverts to the skin the player had on when coming in
    /// </summary>
    public void CloseSkinShop()
    {
        PlayerWorldMovement.allowedToMove = true;

        switch (gm.GetComponent<OldGameManager>().save.MonsterName)
        {
            case "Girl":
                foreach (var item in gm.GetComponent<ShopSkinManagement>().girlSkins.head)
                {
                    var tmp = mapper.GetIconFromSprite(playerHead.GetComponent<SpriteRenderer>().sprite);
                    if (tmp.name == item.skin.name)
                    {
                        if (!item.equipped)
                        {
                            playerHead.GetComponent<SpriteRenderer>().sprite = mapper.GetSpriteFromIcon(originalHeadSprite);
                        }
                    }
                }
                foreach (var item in gm.GetComponent<ShopSkinManagement>().girlSkins.torso)
                {
                    var tmp = mapper.GetIconFromSprite(playerBody.GetComponent<SpriteRenderer>().sprite);
                    if (tmp.name == item.skin.name)
                    {
                        if (!item.equipped)
                        {
                            playerBody.GetComponent<SpriteRenderer>().sprite = mapper.GetSpriteFromIcon(originalBodySprite);
                        }
                    }
                }
                foreach (var item in gm.GetComponent<ShopSkinManagement>().girlSkins.legs)
                {
                    var tmp = mapper.GetIconFromSprite(playerLegs.GetComponent<SpriteRenderer>().sprite);
                    if (tmp.name == item.skin.name)
                    {
                        if (!item.equipped)
                        {
                            playerLegs.GetComponent<SpriteRenderer>().sprite = mapper.GetSpriteFromIcon(originalLegSprite);
                        }
                    }
                }
                break;
            case "SimpleMonster":

                break;
        }
    }

}
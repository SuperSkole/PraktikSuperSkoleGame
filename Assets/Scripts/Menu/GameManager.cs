using Cinemachine;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    #region attributes
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TextMeshProUGUI nameInput;
    [SerializeField] private TextMeshProUGUI playerName;
    public PlayerData player;

    [Header("Player Start up")]
    [SerializeField] private Transform SpawnCharPoint;
    [SerializeField] private GameObject girlPrefab;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private ShopSkinManagement skinsMa = new ShopSkinManagement();
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private MoveFloatingNumbers moveFloatingNumbers;
    public IconSpriteMapper iconSpriteMapper;

    private GameObject loadedPlayer;


    [Header("Important Start up")]
    [SerializeField] private GameObject chosePlayerScreen;
    [SerializeField] private GameObject customizePlayerScreen;
    [SerializeField] private GameObject SkinShop;


    [Header("misc")]
    private string monsterName;
    public Color headColor;
    public Color BodyColor;
    public Color LegColor;

    public GameObject spriteHead;
    public GameObject spriteBody;
    public GameObject spriteLeg;
    public GameManager() { }

    public SaveData save;

    #endregion

    private void Start()
    {
        if (this.gameObject.GetComponent<SaveGameToJson>().IsThereSaveFile() == false)
        {
            PlayerWorldMovement.allowedToMove = false;
            chosePlayerScreen.SetActive(true);
            customizePlayerScreen.SetActive(true);
            SkinShop.SetActive(true);
        }
        else
        {
            //Debug.Log("NewGame/Start/Save file found loading from JSON");
            PlayerWorldMovement.allowedToMove = true;

            //Needs to be true for events to be added to player
            customizePlayerScreen.SetActive(true);

            //Needs to be true for events to be added to player
            SkinShop.SetActive(true);

            chosePlayerScreen.SetActive(false);
            //customizePlayerScreen.SetActive(false);
            //SkinShop.SetActive(false);
            this.gameObject.GetComponent<SaveGameToJson>().LoadFromJson();
        }

    }
    /// <summary>
    /// Creates a new CharacterController and sets the name of the player
    /// Only call on a fresh start
    /// </summary>
    public void CreateNewChar()
    {
        player = new PlayerData(
            monsterName, nameInput.text, 0, 0, 1, 
            SpawnCharPoint.position,
            headColor, BodyColor, LegColor,
            spriteHead.GetComponent<SpriteRenderer>().sprite,
            spriteBody.GetComponent<SpriteRenderer>().sprite,
            spriteLeg.GetComponent<SpriteRenderer>().sprite);

        playerName.text = player.playerName;
        PlayerWorldMovement.allowedToMove = true;
    }
    public void SetMonsterName(string monsterName)
    {
        this.monsterName = monsterName;
    }
    public PlayerData ReturnPlayer()
    {
        return player;
    }

    public void SetLoadGameInfo(SaveData saveData)
    {
        loadedPlayer = Instantiate(WhichPrefabGO(saveData.MonsterName), 
            saveData.SavedPlayerStartPostion.GetVector3(), 
            Quaternion.Euler(0f,90f,0f),
            SpawnCharPoint);
        loadedPlayer.GetComponent<PlayerWorldMovement>().GenerateInteractions();
        skinsMa.StartMapping();
        save = saveData;
        player = new PlayerData(
            saveData.MonsterName,
            saveData.PlayerName,
            saveData.GoldAmount,
            saveData.XPAmount,
            saveData.PlayerLevel,
            saveData.SavedPlayerStartPostion.GetVector3(),
            saveData.HeadColor.ToColor(),
            saveData.BodyColor.ToColor(),
            saveData.LegColor.ToColor(),
            WhichSprite(saveData, "Head"),
            WhichSprite(saveData, "Body"),
            WhichSprite(saveData, "Legs")
            ); ;

        SetWorldAndPlayerParts();

        //Sets the camera to follow the player
        virtualCamera.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        virtualCamera.LookAt = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /// <summary>
    /// Call when first loading, finds the player then sets correct INFO 
    /// from the player info that is set in SetLoadGameInfo
    /// Also sets the world info to match was has been saven
    /// </summary>
    private void SetWorldAndPlayerParts()
    {

        #region Sets UI Items accordingly
        playerName.text = player.playerName;
        var goldXP = this.gameObject.GetComponent<GeneralManagement>();
        goldXP.goldAmount = player.currentGoldAmount;
        goldXP.expAmount = player.currentXPAmount;
        goldXP.Level = player.currentLevel;
         goldXP.UpdateValues();
        #endregion

        #region Sets Player parts accordingly
        //The null conditional operator (?.) ensures that if the Transforsm is not found, it won't cause a NullReferenceException
        var head = loadedPlayer.transform.Find("Head")?.gameObject;
        head.GetComponent<SpriteRenderer>().sprite = player.spriteHead;
        head.GetComponent<SpriteRenderer>().color = player.CurrentHeadColor;

        var body = loadedPlayer.transform.Find("Mainbody")?.gameObject;
        body.GetComponent<SpriteRenderer>().sprite = player.spriteBody;
        body.GetComponent<SpriteRenderer>().color = player.CurrentBodyColor;

        var Legs = loadedPlayer.transform.Find("Legs")?.gameObject;
        Legs.GetComponent<SpriteRenderer>().sprite = player.spriteLeg;
        Legs.GetComponent<SpriteRenderer>().color = player.CurrentLegColor;

        spriteHead = loadedPlayer.transform.Find("Head")?.gameObject;
        spriteBody = loadedPlayer.transform.Find("Mainbody")?.gameObject;
        spriteLeg = loadedPlayer.transform.Find("Legs")?.gameObject;

        headColor = player.CurrentHeadColor;
        BodyColor = player.CurrentBodyColor;
        LegColor = player.CurrentLegColor;


        loadedPlayer.GetComponent<CharacterVisuelManagement>().JustCreatedChar(this.gameObject);
        #endregion

        #region Sets GameManger components accordingly
        // Determine the correct skin data list and parts collection based on the monster type.
        List<SkinData> skinDataList = save.MonsterName == "SimpleMonster" ? save.MonsterPurchasedSkins : save.GirlPurchasedSkins;
        CusParts cusParts = save.MonsterName == "SimpleMonster" ? skinsMa.monsterSkins : skinsMa.girlSkins;
        List<SkinData> headList = new List<SkinData>();
        List<SkinData> bodyList = new List<SkinData>();
        List<SkinData> legList = new List<SkinData>();

        foreach (var item in skinDataList)
        {
            switch (item.Skintype)
            {
                case "Head":
                    headList.Add(item);
                    break;
                case "Body":
                    bodyList.Add(item);
                    break;
                case "Legs":
                    legList.Add(item);
                    break;
            }
        }

        // Ensure the counts match
        if (headList.Count != cusParts.head.Count || bodyList.Count != cusParts.torso.Count || legList.Count != cusParts.legs.Count)
        {
            Debug.LogError("Mismatch between skin data lists and CusParts lists.");
            return;
        }

        // Update the CusParts lists with the loaded data
        for (int i = 0; i < cusParts.head.Count; i++)
        {
            //Debug.Log($"Setting head[{i}]: Purchased={headList[i].Purchased}, Equipped={headList[i].Equipped}");
            cusParts.head[i].purchased = headList[i].Purchased;
            cusParts.head[i].equipped = headList[i].Equipped;
        }
        for (int i = 0; i < cusParts.torso.Count; i++)
        {
            //Debug.Log($"Setting torso[{i}]: Purchased={bodyList[i].Purchased}, Equipped={bodyList[i].Equipped}");
            cusParts.torso[i].purchased = bodyList[i].Purchased;
            cusParts.torso[i].equipped = bodyList[i].Equipped;
        }
        for (int i = 0; i < cusParts.legs.Count; i++)
        {
            //Debug.Log($"Setting legs[{i}]: Purchased={legList[i].Purchased}, Equipped={legList[i].Equipped}");
            cusParts.legs[i].purchased = legList[i].Purchased;
            cusParts.legs[i].equipped = legList[i].Equipped;
        }
        switch (save.MonsterName)
        {
            case "Girl":
                skinsMa.girlSkins.head[0].purchased = true;
                break;
            case "SimpleMonster":


                break;
        }
        

        skinsMa.PrintList();

        #endregion



    }

    /// <summary>
    /// Retrieves the sprite for a specific body part based on the player's equipped skins.
    /// </summary>
    /// <param name="data">The save data containing the player's current state and skin information.</param>
    /// <param name="bodyPart">The body part for which to retrieve the sprite ("Head", "Body", or "Legs").</param>
    /// <returns>The sprite associated with the equipped skin for the specified body part, or null if no equipped skin is found.</returns>
    private Sprite WhichSprite(SaveData data, string bodyPart)
    {
        // Determine the correct skin data list and parts collection based on the monster type.
        List<SkinData> skinDataList = data.MonsterName == "SimpleMonster" ? data.MonsterPurchasedSkins : data.GirlPurchasedSkins;
        CusParts cusParts = data.MonsterName == "SimpleMonster" ? skinsMa.monsterSkins : skinsMa.girlSkins;

        // Iterate through the list of skin data to find an equipped skin.
        foreach (var item in skinDataList)
        {
            if (item.Purchased && item.Equipped)
            {
                switch (bodyPart)
                {
                    case "Head":
                        // Attempt to find and return the equipped head sprite.
                        var headSprite = FindEquippedSprite(cusParts.head, item.SkinName);
                        if (headSprite != null)
                        {
                            Sprite properSprite = iconSpriteMapper.GetSpriteFromIcon(headSprite);
                            if (properSprite != null)
                            {
                                return properSprite;
                            }
                        }
                        break;
                    case "Body":
                        // Attempt to find and return the equipped body sprite.
                        var bodySprite = FindEquippedSprite(cusParts.torso, item.SkinName);
                        if (bodySprite != null)
                        {
                            Sprite properSprite = iconSpriteMapper.GetSpriteFromIcon(bodySprite);
                            if (properSprite != null)
                            {
                                return properSprite;
                            }
                        }
                        break;
                    case "Legs":
                        // Attempt to find and return the equipped leg sprite.
                        var legSprite = FindEquippedSprite(cusParts.legs, item.SkinName);
                        if (legSprite != null)
                        {
                            Sprite properSprite = iconSpriteMapper.GetSpriteFromIcon(legSprite);
                            if (properSprite != null)
                            {
                                return properSprite;
                            }
                        }
                        break;
                }
            }
        }

        // Return null if no equipped sprite is found.
        return null;
    }

    /// <summary>
    /// Searches for a sprite in a list of skins based on the skin name.
    /// </summary>
    /// <param name="skins">The list of skins to search through.</param>
    /// <param name="skinName">The name of the skin to find.</param>
    /// <returns>The sprite associated with the specified skin name, or null if not found.</returns>
    private Sprite FindEquippedSprite(List<Skins> skins, string skinName)
    {
        // Iterate through the list of skins to find a matching skin name.
        foreach (var skin in skins)
        {
            if (skin.skin.name == skinName)
            {
                // Return the sprite if a matching skin name is found.
                return skin.skin;
            }
        }

        // Return null if no matching skin name is found.
        return null;
    }
    public GameObject WhichPrefabGO(string monsterName)
    {
        switch (monsterName)
        {
            case "Girl":
                return girlPrefab;
            case "SimpleMonster":
                return monsterPrefab;
            default:
                return null;
        }
    }

    /// <summary>
    /// When completing a mission and loading back into the main world, 
    /// call this inorder to get the xp and gold from the mission
    /// </summary>
    public void UpdateGoldAndXPAmounts()
    {
        if (StateNameController.CheckIfXPHasGained())
        {
            var xp = StateNameController.GetXPandGold().Item1;
            var gold = StateNameController.GetXPandGold().Item2;

            print($"This much xp from last game: {xp} and this nuch gold: {gold}");

            if (xp > 0) { moveFloatingNumbers.MoveXp(xp); }
            if (gold > 0) { moveFloatingNumbers.MoveGold(gold); }



            this.gameObject.GetComponent<GeneralManagement>().AddEXP(xp);
            this.gameObject.GetComponent<GeneralManagement>().AddGold(gold);
            StateNameController.ResetXPandGoldandCheck();

        }
    }
    /// <summary>
    /// Call from a button of npc before switching scene
    /// Updates the position of the player so the player can spawn the correct place
    /// </summary>
    public void UpdatePlayerCurrentPosition()
    {
        player.currentPosition = loadedPlayer.transform.position;
    }
}


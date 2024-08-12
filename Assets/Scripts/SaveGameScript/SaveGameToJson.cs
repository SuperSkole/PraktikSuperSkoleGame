using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveGameToJson : MonoBehaviour
{
    [SerializeField] private NewGame gm;
    [SerializeField] private ShopSkinManagement shopSkinManagement;

    private string SaveName = "/SaveGameDataFile.json";
    /// <summary>
    /// Saves the current state of the player and game to a JSON file.
    /// </summary>
    public void SaveToJson()
    {
        // Create a new instance of SaveData to store all the relevant data for saving.
        SaveData data = new SaveData();

        // Populate the SaveData object with player's basic information.
        data.PlayerName = gm.player.playerName; // The player's name.
        data.MonsterName = gm.player.monsterName; // The player's chosen monster's name.
        data.GoldAmount = gm.player.currentGoldAmount; // The player's current gold amount.
        data.XPAmount = gm.player.currentXPAmount; // The player's current experience points.
        data.PlayerLevel = gm.player.currentLevel; // The player's current level.

        // Save the player's current position in the game world.
        data.SavedPlayerStartPostion = new SavePlayerPosition(gm.player.currentPosition);

        // Save the player's customization choices for colors.
        data.HeadColor = new SerializableColor(gm.player.CurrentHeadColor); // The color of the player's head.
        data.BodyColor = new SerializableColor(gm.player.CurrentBodyColor); // The color of the player's body.
        data.LegColor = new SerializableColor(gm.player.CurrentLegColor); // The color of the player's legs.

        // Save the skin data for both girl and monster characters.
        data.GirlPurchasedSkins.AddRange(GetSkinData(shopSkinManagement.girlSkins)); // Add purchased girl skins to save data.
        data.MonsterPurchasedSkins.AddRange(GetSkinData(shopSkinManagement.monsterSkins)); // Add purchased monster skins to save data.

        // Convert the SaveData object to a JSON string.
        string json = JsonUtility.ToJson(data, true); // 'true' for pretty print.

        // Write the JSON string to a file at the specified path.
        File.WriteAllText(Application.dataPath + SaveName, json); 
    }
    public void LoadFromJson()
    {
        if (File.Exists(Application.dataPath + SaveName))
        {
            string json = File.ReadAllText(Application.dataPath + SaveName);
            SaveData data = JsonUtility.FromJson<SaveData>(json);


            // Load skin data
            foreach (var skinData in data.GirlPurchasedSkins)
            {
                SetSkinData(shopSkinManagement.girlSkins, skinData);
            }
            // Load skin data
            foreach (var skinData in data.MonsterPurchasedSkins)
            {
                SetSkinData(shopSkinManagement.monsterSkins, skinData);
            }

            //Important to be placed at BUTTOM!
            gm.SetLoadGameInfo(data);
            if (StateNameController.CheckIfXPHasGained())
            {
                gm.UpdateGoldAndXPAmounts();
            }
        }
    }

    public bool IsThereSaveFile()
    {
        return File.Exists(Application.dataPath + SaveName);

    }
    /// <summary>
    /// Extracts the skin data from CusParts and returns a list of SkinData.
    /// </summary>
    /// <param name="cusParts">The CusParts instance containing the skins to be processed.</param>
    /// <returns>A list of SkinData representing the current state of the skins.</returns>
    private List<SkinData> GetSkinData(CusParts cusParts)
    {
        // Create a new list to hold the SkinData objects
        List<SkinData> skinDataList = new List<SkinData>();

        // Iterate through each skin in the head list and add its data to the list
        foreach (var skin in cusParts.head)
        {
            skinDataList.Add(new SkinData("Head", skin.skin.name, skin.purchased, skin.equipped));
        }

        // Iterate through each skin in the torso list and add its data to the list
        foreach (var skin in cusParts.torso)
        {
            skinDataList.Add(new SkinData("Body", skin.skin.name, skin.purchased, skin.equipped));
        }

        // Iterate through each skin in the legs list and add its data to the list
        foreach (var skin in cusParts.legs)
        {
            skinDataList.Add(new SkinData("Legs", skin.skin.name, skin.purchased, skin.equipped));
        }

        // Return the populated list of SkinData
        return skinDataList;
    }

    /// <summary>
    /// Updates the CusParts instances with the loaded skin data.
    /// </summary>
    /// <param name="cusParts">The CusParts instance to be updated.</param>
    /// <param name="skinData">The SkinData instance containing the data to update the skins with.</param>
    private void SetSkinData(CusParts cusParts, SkinData skinData)
    {
        // Iterate through each skin in the head list and update its properties if it matches the provided skin data
        foreach (var skin in cusParts.head)
        {
            if (skin.skin.name == skinData.SkinName)
            {
                skin.purchased = skinData.Purchased;
                skin.equipped = skinData.Equipped;
            }
        }

        // Iterate through each skin in the torso list and update its properties if it matches the provided skin data
        foreach (var skin in cusParts.torso)
        {
            if (skin.skin.name == skinData.SkinName)
            {
                skin.purchased = skinData.Purchased;
                skin.equipped = skinData.Equipped;
            }
        }

        // Iterate through each skin in the legs list and update its properties if it matches the provided skin data
        foreach (var skin in cusParts.legs)
        {
            if (skin.skin.name == skinData.SkinName)
            {
                skin.purchased = skinData.Purchased;
                skin.equipped = skinData.Equipped;
            }
        }
    }
}

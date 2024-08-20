// using System.Collections.Generic;
// using System.IO;
// using CORE;
// using UnityEngine;
//
// public class SaveGameToJson : MonoBehaviour
// {
//     
//     [SerializeField] private ShopSkinManagement shopSkinManagement;
//
//     private string SaveName = "/SaveGameDataFile.json";
//     /// <summary>
//     /// Saves the current state of the player and game to a JSON file.
//     /// </summary>
//     public void SaveToJson()
//     {
//         // Create a new instance of SaveData to store all the relevant data for saving.
//         SaveDataDTO dataDto = new SaveDataDTO();
//
//         // Populate the SaveData object with player's basic information.
//         dataDto.PlayerName = gm.player.PlayerName; // The player's name.
//         dataDto.MonsterName = gm.player.MonsterTypeID.ToString(); // The player's chosen monster's name.
//         dataDto.GoldAmount = gm.player.CurrentGoldAmount; // The player's current gold amount.
//         dataDto.XPAmount = gm.player.CurrentXPAmount; // The player's current experience points.
//         dataDto.PlayerLevel = gm.player.CurrentLevel; // The player's current level.
//
//         // Save the player's current position in the game world.
//         dataDto.SavedPlayerStartPostion = new SavePlayerPosition(gm.player.CurrentPosition);
//
//         // Save the player's customization choices for colors.
//         dataDto.HeadColor = new SerializableColor(gm.player.currentHeadColor); // The color of the player's head.
//         dataDto.BodyColor = new SerializableColor(gm.player.currentBodyColor); // The color of the player's body.
//         dataDto.LegColor = new SerializableColor(gm.player.currentLegColor); // The color of the player's legs.
//
//         // Save the skin data for both girl and monster characters.
//         dataDto.GirlPurchasedSkins.AddRange(GetSkinData(shopSkinManagement.girlSkins)); // Add purchased girl skins to save data.
//         dataDto.MonsterPurchasedSkins.AddRange(GetSkinData(shopSkinManagement.monsterSkins)); // Add purchased monster skins to save data.
//
//         // Convert the SaveData object to a JSON string.
//         string json = JsonUtility.ToJson(dataDto, true); // 'true' for pretty print.
//
//         // Write the JSON string to a file at the specified path.
//         File.WriteAllText(Application.dataPath + SaveName, json); 
//     }
//     public void LoadFromJson()
//     {
//         if (File.Exists(Application.dataPath + SaveName))
//         {
//             string json = File.ReadAllText(Application.dataPath + SaveName);
//             SaveDataDTO dataDto = JsonUtility.FromJson<SaveDataDTO>(json);
//
//
//             // Load skin data
//             foreach (var skinData in dataDto.GirlPurchasedSkins)
//             {
//                 SetSkinData(shopSkinManagement.girlSkins, skinData);
//             }
//             // Load skin data
//             foreach (var skinData in dataDto.MonsterPurchasedSkins)
//             {
//                 SetSkinData(shopSkinManagement.monsterSkins, skinData);
//             }
//
//             //Important to be placed at BUTTOM!
//             gm.SetLoadGameInfo(dataDto);
//             if (StateNameController.CheckIfXPHasGained())
//             {
//                 gm.UpdateGoldAndXPAmounts();
//             }
//         }
//     }
//
//     public bool IsThereSaveFile()
//     {
//         return File.Exists(Application.dataPath + SaveName);
//
//     }
//     /// <summary>
//     /// Extracts the skin data from CusParts and returns a list of SkinData.
//     /// </summary>
//     /// <param name="cusParts">The CusParts instance containing the skins to be processed.</param>
//     /// <returns>A list of SkinData representing the current state of the skins.</returns>
//     private List<SkinData> GetSkinData(CusParts cusParts)
//     {
//         // Create a new list to hold the SkinData objects
//         List<SkinData> skinDataList = new List<SkinData>();
//
//         // Iterate through each skin in the head list and add its data to the list
//         foreach (var skin in cusParts.head)
//         {
//             skinDataList.Add(new SkinData("Head", skin.skin.name, skin.purchased, skin.equipped));
//         }
//
//         // Iterate through each skin in the torso list and add its data to the list
//         foreach (var skin in cusParts.torso)
//         {
//             skinDataList.Add(new SkinData("Body", skin.skin.name, skin.purchased, skin.equipped));
//         }
//
//         // Iterate through each skin in the legs list and add its data to the list
//         foreach (var skin in cusParts.legs)
//         {
//             skinDataList.Add(new SkinData("Legs", skin.skin.name, skin.purchased, skin.equipped));
//         }
//
//         // Return the populated list of SkinData
//         return skinDataList;
//     }
//
//     /// <summary>
//     /// Updates the CusParts instances with the loaded skin data.
//     /// </summary>
//     /// <param name="cusParts">The CusParts instance to be updated.</param>
//     /// <param name="skinData">The SkinData instance containing the data to update the skins with.</param>
//     private void SetSkinData(CusParts cusParts, SkinData skinData)
//     {
//         // Iterate through each skin in the head list and update its properties if it matches the provided skin data
//         foreach (var skin in cusParts.head)
//         {
//             if (skin.skin.name == skinData.SkinName)
//             {
//                 skin.purchased = skinData.isPurchased;
//                 skin.equipped = skinData.isEquipped;
//             }
//         }
//
//         // Iterate through each skin in the torso list and update its properties if it matches the provided skin data
//         foreach (var skin in cusParts.torso)
//         {
//             if (skin.skin.name == skinData.SkinName)
//             {
//                 skin.purchased = skinData.isPurchased;
//                 skin.equipped = skinData.isEquipped;
//             }
//         }
//
//         // Iterate through each skin in the legs list and update its properties if it matches the provided skin data
//         foreach (var skin in cusParts.legs)
//         {
//             if (skin.skin.name == skinData.SkinName)
//             {
//                 skin.purchased = skinData.isPurchased;
//                 skin.equipped = skinData.isEquipped;
//             }
//         }
//     }
// }

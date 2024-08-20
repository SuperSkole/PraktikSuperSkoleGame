// using System.Collections.Generic;
//
// public class CharacterSkinManager
// {
//     public void LoadSkins(SaveData saveData, ShopSkinManagement shopSkinManagement)
//     {
//         foreach (var skinData in saveData.GirlPurchasedSkins)
//         {
//             SetSkinData(shopSkinManagement.girlSkins, skinData);
//         }
//         foreach (var skinData in saveData.MonsterPurchasedSkins)
//         {
//             SetSkinData(shopSkinManagement.monsterSkins, skinData);
//         }
//     }
//
//     public void SaveSkins(SaveData saveData, ShopSkinManagement shopSkinManagement)
//     {
//         saveData.GirlPurchasedSkins.AddRange(GetSkinData(shopSkinManagement.girlSkins));
//         saveData.MonsterPurchasedSkins.AddRange(GetSkinData(shopSkinManagement.monsterSkins));
//     }
//
//     private List<SkinData> GetSkinData(CusParts cusParts)
//     {
//         List<SkinData> skinDataList = new List<SkinData>();
//         // Processing each skin part, e.g., head, torso, legs
//         return skinDataList;
//     }
//
//     private void SetSkinData(CusParts cusParts, SkinData skinData)
//     {
//         // Update specific skin parts based on loaded data
//     }
//     
//     
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
//                 skin.purchased = skinData.Purchased;
//                 skin.equipped = skinData.Equipped;
//             }
//         }
//
//         // Iterate through each skin in the torso list and update its properties if it matches the provided skin data
//         foreach (var skin in cusParts.torso)
//         {
//             if (skin.skin.name == skinData.SkinName)
//             {
//                 skin.purchased = skinData.Purchased;
//                 skin.equipped = skinData.Equipped;
//             }
//         }
//
//         // Iterate through each skin in the legs list and update its properties if it matches the provided skin data
//         foreach (var skin in cusParts.legs)
//         {
//             if (skin.skin.name == skinData.SkinName)
//             {
//                 skin.purchased = skinData.Purchased;
//                 skin.equipped = skinData.Equipped;
//             }
//         }
//     }
// }
// using System;
// using System.Collections.Generic;
// using System.IO;
// using CORE;
// using CORE.Scripts;
// using UnityEngine;
//
// namespace LoadSave
// {
//     public class LoadGameManager
//     {
//         public string SaveDirectory = Path.Combine(Application.dataPath, "Saves");
//
//         private void Awake()
//         {
//             if (!Directory.Exists(SaveDirectory))
//             {
//                 Directory.CreateDirectory(SaveDirectory);
//             }
//         }
//
//         public List<string> GetAllSaveFiles()
//         {
//             List<string> saves = new List<string>();
//             string[] files = Directory.GetFiles(SaveDirectory, "*.json");
//             foreach (var file in files)
//             {
//                 saves.Add(Path.GetFileNameWithoutExtension(file));
//             }
//             
//             return saves;
//         }
//         
//         public string LoadJsonData(string fileName)
//         {
//             string filePath = Path.Combine(SaveDirectory, fileName);
//             if (File.Exists(filePath))
//             {
//                 try
//                 {
//                     return File.ReadAllText(filePath);
//                 }
//                 catch (Exception ex)
//                 {
//                     Debug.LogError("Error reading file: " + ex.Message);
//                     return null;
//                 }
//             }
//
//             Debug.LogError("File not found: " + filePath);
//             return null;
//         }
//
//         public SaveDataDTO LoadGameDataSync(string fileName)
//         {
//             string filePath = Path.Combine(SaveDirectory, fileName + ".json");
//             if (File.Exists(filePath))
//             {
//                 string json = File.ReadAllText(filePath);
//                 return JsonUtility.FromJson<SaveDataDTO>(json);
//             }
//             
//             return null;
//         }
//         
//         public void LoadGameDataAsync(string fileName, Action<SaveDataDTO> callback)
//         {
//             string filePath = Path.Combine(SaveDirectory, fileName + ".json");
//             if (File.Exists(filePath))
//             {
//                 string json = File.ReadAllText(filePath);
//                 SaveDataDTO data = JsonUtility.FromJson<SaveDataDTO>(json);
//                 callback?.Invoke(data);
//             }
//             else
//             {
//                 Debug.LogError("File not found: " + filePath);
//                 callback?.Invoke(null);
//             }
//         }
//         
//         public string GetClosestSaveFile(string username, string monsterName, DateTime referenceTime)
//         {
//             string filenamePattern = $"{username}_{monsterName}_*.json";
//             string dateFormat = "ddMMHHmm";  
//             return DataTimeHelpers.FindClosestFileByTimestamp(Application.dataPath, filenamePattern, referenceTime, dateFormat);
//         }
//         
//         public string GetSaveFilePath(string hashedUsername)
//         {
//             return Path.Combine(SaveDirectory, $"{hashedUsername}_save.json");
//         }
//         
//         public void LoadGame(string hashedUsername)
//         {
//             string filePath = Path.Combine(SaveDirectory, $"{hashedUsername}_save.json");
//
//             if (File.Exists(filePath))
//             {
//                 string json = File.ReadAllText(filePath);
//                 SaveDataDTO data = JsonUtility.FromJson<LoadSave.SaveDataDTO>(json);
//             }
//             else
//             {
//                 Debug.LogError($"Save file not found for user: {hashedUsername}");
//             }
//         }
//     }
// }

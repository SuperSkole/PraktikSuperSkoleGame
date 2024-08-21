using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Scenes.Minigames.MonsterTower.Scrips.DataPersistence.Data;

namespace Scenes.Minigames.MonsterTower
{

    /// <summary>
    /// Handles the writing and reading to a datafile. 
    /// </summary>
    /// 
    public class FileDataHandler
    {
        private string dataDirPath = "";
        private string dataFileName = "";

        public FileDataHandler(string dataDirPath, string dataFileName)
        {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName;
        }

        // The fullpath is defined and there is a check if the file exits on the path. 
        // If not an error is logged. 
        // The filestream and reader is used and the data to load is defined and returned.
        public GameData Load()
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);

            GameData loadedData = null;

            if (File.Exists(fullPath))
            {
                try //why are we trying here? what can give an error?
                {
                    string dataToLoad = "";

                    FileStream stream = new FileStream(fullPath, FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    dataToLoad = reader.ReadToEnd();

                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

                }
                catch (Exception e)
                {
                    Debug.LogError("Error Ocurred when trying to load data to file:" + fullPath + "/n" + e);
                }
            }

            return loadedData;
        }



        //Saves the data to fullpath by using a filestream and streamwriter to write data to the path defined. 
        // An error is logged if nothing could be saved to the defined path. 
        public void Save(GameData data)
        {

            string fullPath = Path.Combine(dataDirPath, dataFileName);

            try //why are we trying here? what can give an error?
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToStore = JsonUtility.ToJson(data, true);

                FileStream stream = new FileStream(fullPath, FileMode.Create);
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(dataToStore);

            }
            catch (Exception e)
            {
                Debug.LogError("Error Ocurred when trying to save data to file:" + fullPath + "/n" + e);

            }

        }

    }
}

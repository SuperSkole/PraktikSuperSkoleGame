using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

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
        //If not an error is logged. 
        // The filestream and reader is used and the data to load is defined and returned.
        public GameData Load()
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);

            GameData loadedData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }

                    }

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

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToStore = JsonUtility.ToJson(data, true);

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }

            }
            catch (Exception e)
            {
                Debug.LogError("Error Ocurred when trying to save data to file:" + fullPath + "/n" + e);

            }

        }

    }
}

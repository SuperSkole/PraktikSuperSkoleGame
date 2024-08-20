using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface an object can use to save and load data. 
public interface IDataPersistence
{

    void LoadData(GameData data);
    void SaveData(ref GameData data);

}

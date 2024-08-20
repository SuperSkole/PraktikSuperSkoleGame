using Scenes.Minigames.MonsterTower.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    namespace Scenes.Minigames.MonsterTower.Scrips
{
    public interface IDataPersistence1
    {

        void LoadData(GameData1 data);
        void SaveData(ref GameData1 data);

    }
}


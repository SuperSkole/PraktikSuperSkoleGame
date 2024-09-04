using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Scenes.Minigames.MonsterTower.Scrips.DataPersistence.Data
{

    /// <summary>
    /// Holds gamedata on MonsterTower
    /// </summary>

    [System.Serializable]
    public class GameData
    {

        public List<BrickLane> BrickLanes=new List<BrickLane>();

        public int currentQuestionIndex;

        public string[] questions;








    }
}

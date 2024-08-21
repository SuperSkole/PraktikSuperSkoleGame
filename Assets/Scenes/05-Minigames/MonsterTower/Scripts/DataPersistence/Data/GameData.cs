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
        public List<BrickLane> BrickLanes;
        public int currentQuestionIndex;
    }
}

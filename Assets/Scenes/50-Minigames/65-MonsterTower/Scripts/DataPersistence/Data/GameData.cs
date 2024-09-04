using System.Collections.Generic;

namespace Scenes._50_Minigames._65_MonsterTower.Scripts.DataPersistence.Data
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

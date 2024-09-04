using System.Collections.Generic;

namespace Scenes._50_Minigames._65_MonsterTower.Scripts
{
    /// <summary>
    /// Holds a list of brickdata and the correctImageIndex. 
    /// Both are needed to load a tower. 
    /// </summary>
    [System.Serializable]
    public class BrickLane
    {
        // Start is called before the first frame update

        public List<BrickData> bricks = new List<BrickData>();

        public int correctImageIndex;

        public BrickLane(int correctImageIndex)
        {

            this.correctImageIndex = correctImageIndex;





        }


    }
}

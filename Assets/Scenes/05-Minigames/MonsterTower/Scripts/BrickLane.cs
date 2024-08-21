using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Minigames.MonsterTower
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

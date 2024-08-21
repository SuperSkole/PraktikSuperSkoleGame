using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Scenes.Minigames.MonsterTower
{
    //Holds a sentence symbolising the images that need to be displayed on the brick. 
    [System.Serializable]
    public class BrickData
    {

        public string input;
        public BrickData(string input)
        {
            this.input = input;

        }



    }
}

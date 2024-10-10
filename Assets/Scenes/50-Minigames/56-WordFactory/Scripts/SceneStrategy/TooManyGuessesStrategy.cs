using Scenes._50_Minigames._56_WordFactory.Scripts.Managers;
using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.SceneStrategy
{
    /// <summary>
    /// Strategy for displaying the "For mange gæt" screen.
    /// </summary>
    public class TooManyGuessesStrategy : IEndSceneStrategy
    {
        public void DisplayEndSceneContent(FactoryEndSceneManager uiFactoryManager)
        {
            uiFactoryManager.SetEndSceneText("For mange gæt");
            uiFactoryManager.SetEndSceneBackgroundColor(Color.red);
        }
    }
}
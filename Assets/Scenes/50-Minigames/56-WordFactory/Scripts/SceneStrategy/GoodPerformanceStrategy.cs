using Scenes._50_Minigames._56_WordFactory.Scripts.Managers;
using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.SceneStrategy
{
    /// <summary>
    /// Strategy for displaying the "Du gjorde det godt" screen.
    /// </summary>
    public class GoodPerformanceStrategy : IEndSceneStrategy
    {
        public void DisplayEndSceneContent(FactoryEndSceneManager uiFactoryManager)
        {
            uiFactoryManager.SetEndSceneText("Du gjorde det godt");
            uiFactoryManager.SetEndSceneBackgroundColor(Color.green);
        }
    }
}
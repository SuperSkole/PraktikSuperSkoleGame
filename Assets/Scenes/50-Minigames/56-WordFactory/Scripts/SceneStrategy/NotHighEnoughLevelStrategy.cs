using Scenes._50_Minigames._56_WordFactory.Scripts.Managers;
using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.SceneStrategy
{
    /// <summary>
    /// Strategy for displaying the "Du har ikke lavet nok bogstaver" screen.
    /// </summary>
    public class NotHighEnoughLevelStrategy : IEndSceneStrategy
    {
        public void DisplayEndSceneContent(FactoryEndSceneManager uiFactoryManager)
        {
            uiFactoryManager.SetEndSceneText("Du har ikke lavet nok bogstaver, kom tilbage senere");
            uiFactoryManager.SetEndSceneBackgroundColor(Color.yellow);
        }
    }
}
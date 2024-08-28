using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._05_Minigames.WordFactory.Scripts
{
    public class LoadingSceneController : MonoBehaviour
    {
        public void StartOneWheelGame() => LoadMiniGameWithGears(1);
        public void StartTwoWheelGame() => LoadMiniGameWithGears(2);
        public void StartThreeWheelGame() => LoadMiniGameWithGears(3);

        /// <summary>
        /// Loads the WordFactory mini game scene with a specified number of gears.
        /// </summary>
        /// <param name="numberOfGears">The number of gears to use in the mini game.</param>
        public void LoadMiniGameWithGears(int numberOfGears)
        {
            // Set the number of gears
            GameConfig.NumberOfGears = numberOfGears; 
            SceneManager.LoadScene(SceneNames.Factory); 
        }
    }
}

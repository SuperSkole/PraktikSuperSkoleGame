using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._05_Minigames.WordFactory.Scripts
{
    public class LoadingSceneController : MonoBehaviour
    {
        public void Load1WheelMiniGame()
        {
            SceneManager.LoadScene("WordFactory"); 
        }
        public void Load2WheelMiniGame()
        {
            SceneManager.LoadScene("WordFactory"); 
        }
    }
}

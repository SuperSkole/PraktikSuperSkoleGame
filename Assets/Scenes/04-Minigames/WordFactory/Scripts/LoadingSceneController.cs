using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Minigames.WordFactory.Scripts
{
    public class LoadingSceneController : MonoBehaviour
    {
        public void LoadMiniGame()
        {
            SceneManager.LoadScene("WordFactory"); 
        }
    }
}

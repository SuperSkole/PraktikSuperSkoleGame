using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Scripts
{
    public class ChangeScene : MonoBehaviour
    {

        [SerializeField]
        private string sceneName;

 
        public void NextScene()
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError("Scene name is not set in the inspector!");
            }
        }
    }
}

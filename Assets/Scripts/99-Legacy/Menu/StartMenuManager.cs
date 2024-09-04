using UnityEngine;
using UnityEngine.SceneManagement;

namespace _99_Legacy.Menu
{
    public class StartMenuManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void StartNewGame()
        {
            SceneManager.LoadScene("Town");
        }
        public void LoadNewGame()
        {

        }
    }
}

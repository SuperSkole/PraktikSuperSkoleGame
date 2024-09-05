using UnityEngine;
using UnityEngine.SceneManagement;

namespace _99_Legacy
{
    public class SceneManagerScript : MonoBehaviour
    {
        public static SceneManagerScript Instance { get; private set; }
        public int SceneID { get; private set; }

        void Awake()
        {
            // Singleton pattern to ensure only one instance of the script exists
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Persist through scene loads
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // Initialize the SceneID based on the loaded scene
            InitializeSceneID();
        }

        void InitializeSceneID()
        {
            Scene scene = SceneManager.GetActiveScene();
            string sceneName = scene.name;

            switch (sceneName)
            {
                case "Town":
                    SceneID = 0;
                    break;
                case "58-MiniRacingGame":
                    SceneID = 1;
                    break;
                case "MiniWritingGame":
                    SceneID = 2;
                    break;
                // Add more cases for other scenes
                default:
                    SceneID = -1; // Default case if scene name doesn't match any known scenes
                    break;
            }
            Debug.Log($"Scene loaded: {sceneName} with SceneID: {SceneID}");
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            InitializeSceneID();
        }
    }
}
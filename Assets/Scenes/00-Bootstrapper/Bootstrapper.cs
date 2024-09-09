// inspired by Jason Weimann's Bootstrapper
// https://www.youtube.com/watch?v=o03NpUdpdrc

using System.Collections;
using System.Threading.Tasks;
using CORE;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._00_Bootstrapper
{
    /// <summary>
    /// Manages initial scene loading and setup for the game.
    /// </summary>
    public class Bootstrapper : MonoBehaviour
    {
        /// <summary>
        /// Asynchronously initializes Unity services and loads necessary scenes at start.
        /// Using async Task instead of async void for better error handling and control flow.
        /// </summary>
        private async void Start()
        {
            Application.runInBackground = true;
            await UnityServices.InitializeAsync();

            // Load LoginScene scene additively if only the initial scene is loaded.
            if (SceneManager.loadedSceneCount == 1)
                SceneManager.LoadScene(SceneConfig.InitialScene, LoadSceneMode.Additive);
            
            // Register for the sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDestroy()
        {
            // Unregister the sceneLoaded event when this object is destroyed
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        /// <summary>
        /// Handles logic when a new scene is loaded.
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Scene Loaded");
            // Check if the loaded scene is NOT the LoginScene
            if (this != null && scene.name != SceneNames.Login)
            {
                Debug.Log($"Scene loaded: {scene.name}, loading PlayerScene...");
                StartCoroutine(LoadPlayerScene());
            }
        }

        /// <summary>
        /// Loads the PlayerScene and performs any necessary game setup after it is fully loaded.
        /// </summary>
        private IEnumerator LoadPlayerScene()
        {
            // Load PlayerScene additively
            AsyncOperation loadPlayerScene = SceneManager.LoadSceneAsync(SceneNames.Player, LoadSceneMode.Additive);
            
            // Wait until the PlayerScene is fully loaded
            yield return new WaitUntil(() => loadPlayerScene is { isDone: true });

            // Perform initializations after PlayerScene is loaded
            GameManager.Instance.IsNewGame = true;
            GameManager.Instance.IsPlayerBootstrapped = true;
            GameManager.Instance.CurrentUser = "TEST";
            GameManager.Instance.CurrentMonsterName = "TESTMonster";
            GameManager.Instance.PlayerData.MonsterName = "TESTMonster";
        }

        /// <summary>
        /// Ensures the Bootstrapper scene is loaded before any other scene loads.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            // Load Bootstrapper scene if it's not already loaded.
            if (!SceneManager.GetSceneByName("00-"+nameof(Bootstrapper)).isLoaded)
                SceneManager.LoadScene("00-" + nameof(Bootstrapper));

            // Manage scenes specifically in the Unity editor.
#if UNITY_EDITOR
            // Retrieve the currently active scene to check if it's valid.
            var currentlyLoadedEditorScene = SceneManager.GetActiveScene();
            
            // If the current scene is valid, load it asynchronously in additive mode, so we can test during dev fase
            if (currentlyLoadedEditorScene.IsValid())
                SceneManager.LoadSceneAsync(currentlyLoadedEditorScene.name, LoadSceneMode.Additive);
#else
        // In the final build, load splash scene additively,
        // ensuring that it's ready for player interaction without replacing the current scene setup.
        
        SceneManager.LoadScene(SceneConfig.InitialScene, LoadSceneMode.Additive);
#endif
        }
    }
    
    /// <summary>
    /// Provides a central reference for scene names to manage scene transitions.
    /// </summary>
    public static class SceneConfig
    {
        public const string InitialScene = SceneNames.Login; // TODO: Update to "SplashScene" when made.
    }
}
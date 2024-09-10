// inspired by Jason Weimann's Bootstrapper
// https://www.youtube.com/watch?v=o03NpUdpdrc

using System.Threading.Tasks;
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
        private async Task Start()
        {
            Application.runInBackground = true;
            await UnityServices.InitializeAsync();

            // Load LoginScene scene additively if only the initial scene is loaded.
            if (SceneManager.loadedSceneCount == 1)
                SceneManager.LoadScene(SceneConfig.InitialScene, LoadSceneMode.Additive);
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
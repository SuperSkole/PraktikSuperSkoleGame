using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Bootstrapper
{
    public class Bootstrapper : MonoBehaviour
    {
        // Using async Task instead of async void for better error handling and control flow.
        async Task Start()
        {
            Application.runInBackground = true;
            await UnityServices.InitializeAsync();

            // Load LoginScene scene additively if only the initial scene is loaded.
            // todo change to splashscene
            if (SceneManager.loadedSceneCount == 1)
                SceneManager.LoadScene("00-LoginScene", LoadSceneMode.Additive);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            // Load Bootstrapper scene if it's not already loaded.
            if (!SceneManager.GetSceneByName(nameof(Bootstrapper)).isLoaded)
                SceneManager.LoadScene(nameof(Bootstrapper));

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

        // todo: change to splash
        SceneManager.LoadScene("00-LoginScene", LoadSceneMode.Additive);
#endif
        }
    }
}
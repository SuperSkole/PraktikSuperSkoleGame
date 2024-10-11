using System;
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
        private const int MaxRetries = 3;

        /// <summary>
        /// Asynchronously initializes Unity services and loads necessary scenes at start.
        /// </summary>
        private async void Awake()
        {
            //Debug.Log("Initializing Unity services");

            int attempt = 0;
            bool success = false;

            while (attempt < MaxRetries && !success)
            {
                try
                {
                    attempt++;
                    await UnityServices.InitializeAsync();

                    if (UnityServices.State == ServicesInitializationState.Initialized)
                    {
                        //Debug.Log("Unity services initialized successfully on attempt: " + attempt);
                        success = true;
                    }
                    else
                    {
                        //Debug.LogError("Unity services not fully initialized.");
                    }
                }
                catch (Exception ex)
                {
                    //Debug.LogError($"Initialization attempt {attempt} failed.");
                    //Debug.LogException(ex);

                    if (attempt >= MaxRetries)
                    {
                        //Debug.LogError("All initialization attempts failed.");
                        throw; // TODO handle failure after all attempts fail
                    }

                    // Wait 1 second before retrying
                    await Task.Delay(1000); 
                }
            }

            // Proceed with the rest of your setup after Unity services are initialized
            if (success)
            {
                StartGameSetup();
            }
        }

        /// <summary>
        /// Continues the game setup after Unity services are initialized.
        /// </summary>
        private void StartGameSetup()
        {
            Application.runInBackground = true;
            
            // Load LoginScene scene additively if only the initial scene is loaded.
            if (SceneManager.loadedSceneCount == 1)
            {
                SceneManager.LoadScene(SceneConfig.InitialScene, LoadSceneMode.Additive);
            }
        }

        /// <summary>
        /// Ensures the Bootstrapper scene is loaded before any other scene loads.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            // Load Bootstrapper scene if it's not already loaded.
            if (!SceneManager.GetSceneByName("00-"+nameof(Bootstrapper)).isLoaded)
            {
                SceneManager.LoadScene("00-" + nameof(Bootstrapper));
            }

#if UNITY_EDITOR
            var currentlyLoadedEditorScene = SceneManager.GetActiveScene();
            if (currentlyLoadedEditorScene.IsValid())
            {
                SceneManager.LoadSceneAsync(currentlyLoadedEditorScene.name, LoadSceneMode.Additive);
            }
#else
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

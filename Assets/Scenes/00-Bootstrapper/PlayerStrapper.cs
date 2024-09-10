using System.Collections;
using CORE;
using Scenes._02_LoginScene.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._00_Bootstrapper
{
    /// <summary>
    /// Handles the loading of the PlayerScene directly for development purposes,
    /// bypassing other initial scenes when not starting from the LoginScene.
    /// </summary>
    public class PlayerStrapper : MonoBehaviour
    {
        private AuthenticationManager authenticationManager;

        private void Awake()
        {
            authenticationManager = gameObject.AddComponent<AuthenticationManager>();
        }

        private IEnumerator Start()
        {
            Debug.Log("Active scene at start: " + SceneManager.GetActiveScene().name);
            Debug.Log("Total loaded scenes: " + SceneManager.sceneCount);

            // Loop through all loaded scenes to check if LoginScene is active
            bool loginSceneActive = false;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == SceneNames.Login)
                {
                    loginSceneActive = true;
                    break;
                }
            }

            // Only proceed to load PlayerScene if LoginScene is not active
            if (!loginSceneActive)
            {
                // Asynchronous sign-in
                yield return authenticationManager.SignInAnonymouslyAsync();
                
                InitializePlayerSettings();
         
                Debug.Log("Loading PlayerScene...");
                AsyncOperation loadPlayerScene = SceneManager.LoadSceneAsync(SceneNames.Player, LoadSceneMode.Additive);

                // Wait until the PlayerScene is fully loaded
                yield return new WaitUntil(() => loadPlayerScene is { isDone: true });
            }
            else
            {
                Debug.Log("LoginScene is active. Bypassing PlayerScene load.");
            }
        }

        /// <summary>
        /// Sets up default values for testing the PlayerScene.
        /// </summary>
        private void InitializePlayerSettings()
        {
            GameManager.Instance.IsNewGame = true;
            GameManager.Instance.IsPlayerBootstrapped = true;
            GameManager.Instance.CurrentUser = "TEST";
            GameManager.Instance.CurrentMonsterName = "TESTMonster";
            GameManager.Instance.PlayerData.MonsterName = "TESTMonster";
            Debug.Log("PlayerScene loaded with test settings.");
        }

        // Ensure this script only compiles and runs in the Unity Editor
#if UNITY_EDITOR
        [MenuItem("Custom/Load Player Scene for Testing")]
        private static void LoadPlayerSceneForTesting()
        {
            SceneManager.LoadScene(SceneNames.Player, LoadSceneMode.Additive);
        }
#endif
    }
}

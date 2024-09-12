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
        private IAuthenticationService authService;
        
        private IEnumerator Start()
        {
            authService = new AnonymousAuthenticationService();
            
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
                yield return authService.SignInAsync();
                
                InitializePlayerSettings();
         
                Debug.Log("Loading PlayerScene...");
                AsyncOperation loadPlayerScene = SceneManager.LoadSceneAsync(SceneNames.Player, LoadSceneMode.Additive);

                // Wait until the PlayerScene is fully loaded
                yield return new WaitUntil(() => loadPlayerScene is { isDone: true });
                
                // Find and disable the player UI Canvas
                DisablePlayerCanvas();
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
        
        /// <summary>
        /// Finds the Canvas in the scene and disables it.
        /// </summary>
        private void DisablePlayerCanvas()
        {
            // Look for any Canvas object in the PlayerScene
            Canvas playerCanvas = FindObjectOfType<Canvas>();

            if (playerCanvas != null)
            {
                playerCanvas.gameObject.SetActive(false);
                Debug.Log("Player UI Canvas has been disabled.");
            }
            else
            {
                Debug.LogWarning("No Player UI Canvas found to disable.");
            }
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

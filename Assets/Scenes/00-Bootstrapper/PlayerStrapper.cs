using System.Collections;
using CORE;
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
        // Use this for initialization
        private IEnumerator Start()
        {
            Debug.Log("Player strapper started");
            // Check if the current active scene is not the LoginScene
            if (SceneManager.GetActiveScene().name != SceneNames.Login)
            {
                // Load PlayerScene additively
                AsyncOperation loadPlayerScene = SceneManager.LoadSceneAsync(SceneNames.Player, LoadSceneMode.Additive);
            
                // Wait until the PlayerScene is fully loaded
                yield return new WaitUntil(() => loadPlayerScene is { isDone: true });

                // Perform initializations after PlayerScene is loaded
                InitializePlayerSettings();
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

            // Additional debug or setup code can be added here
            Debug.Log("PlayerScene loaded with test settings.");
        }

        // Ensure this script only compiles and runs in the Unity Editor
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Custom/Load Player Scene for Testing")]
        private static void LoadPlayerSceneForTesting()
        {
            SceneManager.LoadScene(SceneNames.Player, LoadSceneMode.Additive);
        }
#endif
    }
}
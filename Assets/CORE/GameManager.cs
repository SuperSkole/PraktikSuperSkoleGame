using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CORE
{
    public class GameManager : MonoBehaviour
    {
        // Player and game Data
        public PlayerData PlayerData { get; set; }

        
        public string CurrentUsername { get; private set; }
        public string CurrentPlayerName { get; private set; }
        public string CurrentSaveFileName { get; private set; }
        
        public static GameManager Instance { get; private set; }
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); 
            }
            else
            {
                Destroy(gameObject); 
            }
        }

        #region Login Region
        public void SetUserDuringLogin()
        {
            // Find the username input field in login scene
            GameObject inputFieldObject = GameObject.Find("UsernameInputField");
            TMP_InputField inputField = inputFieldObject.GetComponent<TMP_InputField>(); 

            if (inputField != null)
            {
                CurrentUsername = inputField.text;  
                Debug.Log("Username set to: " + CurrentUsername);
            }
            else
            {
                Debug.Log("No TMP Input Field found in the scene!");
            }
        }
        #endregion Login Region
        
        

        public void LoadGame(string saveFileName)
        {
            // Logic to load game data
            Debug.Log("Loading game from: " + saveFileName);
            
        }

        public void SaveGame()
        {
            // save logic, using savemanager
            Debug.Log("Game Saved!");
        }

        public void ExitGame()
        {
            // Cleanup or save state before exiting
            Application.Quit();
        }
    }
}
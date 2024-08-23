using CORE;
using LoadSave;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.StartScene.Scripts
{
    /// <summary>
    /// controlling the game loading process.
    /// It does not get involved with how data is loaded
    /// or how player objects are instantiated
    /// </summary>
    public class LoadGameController : MonoBehaviour
    {
        [SerializeField] private LoadGameSetup loadGameSetup;

        public static LoadGameController Instance;

        public void RegisterPanel(SavePanel panel) 
        {
            panel.OnLoadRequested += HandleLoadRequest;
        }
        
        private void Awake() 
        {
            Instance = this;
        }
        
        private void HandleLoadRequest(string fileName)
        {
            Debug.Log("LoadGameController-HandleLoadRequest: Handling load request for file: " + fileName);
            GameManager.Instance.LoadManager.LoadGameDataAsync(fileName, OnDataLoaded); 
        }

        private void OnDataLoaded(SaveDataDTO data)
        {
            if (data != null)
            {
                Debug.Log("Data loaded successfully.");
                loadGameSetup.SetupPlayer(data);
                SceneManager.LoadScene("02-PlayerHouse");
            }
            else
            {
                Debug.LogError("Failed to load game data.");
            }
        }

        private void OnDestroy()
        {
            SavePanel[] panels = FindObjectsOfType<SavePanel>();
            foreach (var panel in panels)
            {
                panel.OnLoadRequested -= HandleLoadRequest;
            }
        }
    }
}
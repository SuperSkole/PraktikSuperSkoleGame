using UnityEngine;

namespace Scenes.StartScene.Scripts
{
    /// <summary>
    /// controlling the game loading process.
    /// It does not get involved with how data is loaded
    /// or how player objects are instantiated
    /// </summary>
    public class LoadGameController : MonoBehaviour
    {
        [SerializeField] private LoadGameManager loadGameManager;
        [SerializeField] private LoadGameSetup loadGameSetup;

        public static LoadGameController Instance;

        void Awake() {
            Instance = this;
        }

        public void RegisterPanel(SavePanel panel) {
            panel.OnLoadRequested += HandleLoadRequest;
        }

        
        public void HandleLoadRequest(string fileName)
        {
           
            Debug.Log("Handling load request for file: " + fileName);
            loadGameManager.LoadGameDataAsync(fileName, OnDataLoaded); 
        }

        private void OnDataLoaded(SaveDataDTO data)
        {
            if (data != null)
            {
                Debug.Log("Data loaded successfully.");
                loadGameSetup.SetupPlayer(data);
            }
            else
            {
                Debug.LogError("Failed to load game data.");
            }
        }

        void OnDestroy()
        {
            SavePanel[] panels = FindObjectsOfType<SavePanel>();
            foreach (var panel in panels)
            {
                panel.OnLoadRequested -= HandleLoadRequest;
            }
        }
    }
}
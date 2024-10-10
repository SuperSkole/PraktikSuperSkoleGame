using Scenes._10_PlayerScene.Scripts;
using Scenes._50_Minigames._56_WordFactory.Scripts.SceneStrategy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.Managers
{
    public class FactoryEndSceneManager : MonoBehaviour
    {
        [SerializeField] private Text endSceneText;
        [SerializeField] private Image endSceneBackground;
        [SerializeField] private Image exitImage;
        
        /// <summary>
        /// Called when the "Exit" button is clicked.
        /// </summary>
        public void OnExitButton()
        {
            Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<AutoMovePlayerInFactory>());
            
            // Load the main scene
            Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<AutoMovePlayerInFactory>());
            SceneManager.LoadScene(SceneNames.Main); 
        }

        public void SetEndSceneText(string message)
        {
            endSceneText.text = message;
        }

        public void SetEndSceneBackgroundColor(Color color)
        {
            endSceneBackground.color = color;
        }
        
        private IEndSceneStrategy endSceneStrategy;
    
        [SerializeField] private UIFactoryManager uiFactoryManager;

        /// <summary>
        /// Sets the strategy for the end scene.
        /// </summary>
        /// <param name="strategy">The strategy to be used.</param>
        public void SetEndSceneStrategy(IEndSceneStrategy strategy)
        {
            endSceneStrategy = strategy;
        }

        private void Start()
        {
            // Display content based on the current strategy
            endSceneStrategy?.DisplayEndSceneContent(this);
        }
    }
}
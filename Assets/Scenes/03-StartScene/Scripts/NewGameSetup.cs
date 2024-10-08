using CORE;
using Scenes._10_PlayerScene.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes._03_StartScene.Scripts
{
    public class NewGameSetup : MonoBehaviour
    {
        // Fields required for setting up a new game
        [SerializeField] private Transform spawnCharPoint;
        [SerializeField] private TMP_InputField monsterNameInput;
        [SerializeField] private TextMeshProUGUI monsterNameFeedback;
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Image acceptButton;

        private MonsterNameInputValidationController monsterNameValidator;
        
        private void Start()
        {
            monsterNameValidator = GetComponent<MonsterNameInputValidationController>();
            monsterNameInput.onValueChanged.AddListener(delegate { ValidateMonsterName(); });
            ValidateMonsterName(); 
        }

        private void ValidateMonsterName()
        {
            bool isValid = monsterNameValidator.ValidateMonsterName(monsterNameInput.text, monsterNameFeedback);
            acceptButton.enabled = isValid;
        }
        
        public void OnClick()
        {
            // Set the new game flag in GameManager
            GameManager.Instance.IsNewGame = true;

            // Set the player name in GameManager for use during player setup
            GameManager.Instance.CurrentMonsterName = monsterNameInput.text;

            // Load the PlayerScene asynchronously
            AsyncOperation playerSceneLoad = SceneManager.LoadSceneAsync(SceneNames.Player, LoadSceneMode.Additive);

            // Once the PlayerScene is loaded, PlayerManager's Awake() will automatically handle the player setup
            if (playerSceneLoad != null)
            {
                playerSceneLoad.completed += op =>
                {
                    SceneLoader.Instance.LoadScene(SceneNames.House);
                };
            }
        }
    }
}
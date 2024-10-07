using CORE;
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
            SetupPlayer();
            SceneManager.LoadSceneAsync(SceneNames.Player, LoadSceneMode.Additive);
            SceneLoader.Instance.LoadScene(SceneNames.House);
        }   
        
        private void SetupPlayer()
        {
            GameManager.Instance.IsNewGame = true;
            
            GameManager.Instance.CurrentMonsterName = monsterNameInput.text;
            //GameManager.Instance.CurrentMonsterColor = ChosenMonsterColor;
            
            GameManager.Instance.PlayerData.MonsterName = monsterNameInput.text;
            //GameManager.Instance.PlayerData.MonsterColor = ChosenMonsterColor;
        }
    }
}
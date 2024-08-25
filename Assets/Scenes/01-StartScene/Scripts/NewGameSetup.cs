using CORE;
using LoadSave;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._01_StartScene.Scripts
{
    public class NewGameSetup : MonoBehaviour
    {
        // Fields required for setting up a new game
        [SerializeField] private Transform spawnCharPoint;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private GameObject playerPrefab;

        public string ChosenMonsterColor;
        
        public void OnClick()
        {
            SetUpPlayer();
            SceneManager.LoadScene(SceneNames.House);
            SceneManager.LoadSceneAsync(SceneNames.Player, LoadSceneMode.Additive);
        }   
        
        private void SetUpPlayer()
        {
            GameManager.Instance.CurrentMonsterName = nameInput.text;
            GameManager.Instance.CurrentMonsterColor = ChosenMonsterColor;
        }
    }
}
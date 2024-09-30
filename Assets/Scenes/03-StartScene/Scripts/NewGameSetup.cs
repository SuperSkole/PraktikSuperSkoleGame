using CORE;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._03_StartScene.Scripts
{
    public class NewGameSetup : MonoBehaviour
    {
        // Fields required for setting up a new game
        [SerializeField] private Transform spawnCharPoint;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private GameObject playerPrefab;

        //public string ChosenMonsterColor;
        
        public void OnClick()
        {
            SetupPlayer();
            SceneLoader.Instance.LoadSceneInBackground(SceneNames.House, SceneNames.Main);
            SceneManager.LoadSceneAsync(SceneNames.Player, LoadSceneMode.Additive);
        }   
        
        private void SetupPlayer()
        {
            GameManager.Instance.IsNewGame = true;
            
            GameManager.Instance.CurrentMonsterName = nameInput.text;
            //GameManager.Instance.CurrentMonsterColor = ChosenMonsterColor;
            
            GameManager.Instance.PlayerData.MonsterName = nameInput.text;
            //GameManager.Instance.PlayerData.MonsterColor = ChosenMonsterColor;
        }
    }
}
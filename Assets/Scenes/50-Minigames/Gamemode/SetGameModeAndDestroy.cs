using System;
using System.Collections.Generic;
using CORE;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._50_Minigames.Gamemode
{
    public class SetGameModeAndDestroy : MonoBehaviour
    {
        [SerializeField] public int sceneID;
        IGameModeSetter modeSetter;
        private IGenericGameMode gamemode;
        private IGameRules gameRule;
        private delegate void SceneSwitch();

        [SerializeField] private List<GameObject> buttons;

        [SerializeField] private TextMeshProUGUI title;

        [SerializeField]private bool usePlayerLevel = false;
        [SerializeField]private TextMeshProUGUI errorText;
        SceneSwitch sceneSwitcher;

        [SerializeField] private int playerLevel = 3;
        //tells the scene manager to call the OnSceneLoaded function whenever a scene is loaded
        private void Start()
        {

            SceneManager.sceneLoaded += OnSceneLoaded;
            playerLevel = GameManager.Instance.PlayerData.CurrentLevel;
            if (playerLevel == 0 && usePlayerLevel)
            {
                playerLevel = 4;
            }
            switch (sceneID)
            {
                case 0:
                    modeSetter = new MonsterTowerSetter();
                    sceneSwitcher = new SceneSwitch(SwitchScenes.SwitchToTowerScene);
                    if(usePlayerLevel)
                    {
                        title.text = "Monstertårn";
                    }
                    break;

                case 1:
                    modeSetter = new SymbolEaterSetter();
                    sceneSwitcher = new SceneSwitch(SwitchScenes.SwitchToSymbolEaterScene);
                    if(usePlayerLevel)
                    {
                        title.text = "Grovæder";
                    }
                    
                    break;
                case 2:
                    modeSetter = new LetterGardenSetter();
                    sceneSwitcher = new SceneSwitch(SwitchScenes.SwitchToLetterGardenScene);
                    if(usePlayerLevel)
                    {
                        title.text = "Bogstavshave";
                    }
                    break;
                case 3:
                    modeSetter = new MiniRacingSetter();
                    sceneSwitcher = new SceneSwitch(SwitchScenes.SwitchToRacerScene);
                    if (usePlayerLevel)
                    {
                        title.text = "Racing";
                    }
                    break;
                case 4:
                    modeSetter = new BankFrontSetter();
                    sceneSwitcher = new SceneSwitch(SwitchScenes.SwitchToBankFrontScene);
                    if(usePlayerLevel)
                    {
                        title.text = "Bank Hovedindgang";
                    }
                    break;

                case 5:
                    modeSetter = new PathOfDangerSetter();
                    sceneSwitcher = new SceneSwitch(SwitchScenes.SwitchToPathOfDanger);
                    if (usePlayerLevel)
                    {
                        title.text = "Den Farlige Rute";
                    }
                    break;
                case 6:
                    modeSetter = new WordProdutionLineSetter();
                    sceneSwitcher = new SceneSwitch(SwitchScenes.SwitchToProductionLine);
                    if (usePlayerLevel)
                    {
                        title.text = "Ord Produktion B\u00E5nd";
                    }
                    break;
            }
            if(usePlayerLevel)
            {
                SetGameRulesAndGameMode(playerLevel);
                if(gamemode == null && gameRule == null)
                {
                    buttons[0].SetActive(false);
                    errorText.text = "Du kan ikke spille det her lige nu. Prøv igen Senere";
                }
                else
                {
                    errorText.text = "";
                }
            }
            
        }


        /// <summary>
        /// Finds an object with the tag "setup" in the scene, then runs the expected MiniGameSetup interface's method, then kills itself to avoid memory leak
        /// neither paramater is important to the method, but is required to set when using this method
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="loadSceneMode"></param>
        void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            GameObject parentToTarget = GameObject.FindGameObjectWithTag("Setup");
            IMinigameSetup target = parentToTarget.GetComponent<IMinigameSetup>();
            target.SetupGame(gamemode, gameRule);


            SceneManager.sceneLoaded -= OnSceneLoaded;


            Destroy(gameObject);
        }

        public void SetGameRulesAndGameMode(int level)
        {
            (IGameRules, IGenericGameMode) gamerulesAndGamemode = modeSetter.DetermineGamemodeAndGameRulesToUse(level - 1);
            gameRule = gamerulesAndGamemode.Item1;
            gamemode = gamerulesAndGamemode.Item2;
        }
        /// <summary>
        /// sets a gamemode in this object, so that OnSceneLoaded can set the correct gamemode when entering the scene
        /// </summary>
        /// <param name="gamemodeID">The gamemode we are setting</param>
        public void Setgamemode(string mode)
        {
            gamemode = modeSetter.SetMode(mode);
        }
        /// <summary>
        /// sets a gamerule in this object, so that OnSceneLoaded can set the correct GameRule when entering the scene
        /// </summary>
        /// <param name="gameRuleID"></param>
        public void SetGameRules(string gamerules)
        {
            gameRule = modeSetter.SetRules(gamerules);
        }

        public void SelfDestruct()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }

        public void OnClick(int mod)
        {
            SetGameRulesAndGameMode(playerLevel + mod);
            sceneSwitcher();
        }
    }
}
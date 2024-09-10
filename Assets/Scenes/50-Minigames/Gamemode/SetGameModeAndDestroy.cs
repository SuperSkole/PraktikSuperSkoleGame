using System;
using System.Collections.Generic;
using CORE;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using TMPro;
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

        SceneSwitch sceneSwitcher;
        [SerializeField] private int playerLevel = 3;
        //tells the scene manager to call the OnSceneLoaded function whenever a scene is loaded
        private void Start()
        {

            SceneManager.sceneLoaded += OnSceneLoaded;
            playerLevel = GameManager.Instance.PlayerData.CurrentLevel;
            if (playerLevel == 0)
            {
                playerLevel = 4;
            }
            switch (sceneID)
            {
                case 0:
                    modeSetter = new MonsterTowerSetter();
                    sceneSwitcher = new SceneSwitch(SwitchScenes.SwitchToTowerScene);
                    title.text = "Monstertårn";
                    break;

                case 1:
                    modeSetter = new SymbolEaterSetter();
                    sceneSwitcher = new SceneSwitch(SwitchScenes.SwitchToSymbolEaterScene);
                    title.text = "Grovæder";
                    break;
                case 2:
                    modeSetter = new LetterGardenSetter();
                    sceneSwitcher = new SceneSwitch(SwitchScenes.SwitchToLetterGardenScene);
                    title.text = "Bogstavshave";
                    break;
            }

            //Destroys the first button if the player is level 1 or if no game mode exist for it.
            if (playerLevel == 1)
            {
                Destroy(buttons[0]);
            }
            else
            {
                Setgamemode(playerLevel - 1);
                SetGameRules(playerLevel - 1);
                if (gamemode == null && gameRule == null)
                {
                    Destroy(buttons[0]);
                }
            }
            //Destroys the last button if the player is max level or if no gamemode exist for it.
            if (playerLevel == 5)
            {
                Destroy(buttons[2]);
            }
            else
            {
                Setgamemode(playerLevel + 1);
                SetGameRules(playerLevel + 1);
                if (gamemode == null && gameRule == null)
                {
                    Destroy(buttons[2]);
                }
            }
            //Destroys the middle button if no gamemode exists for it
            Setgamemode(playerLevel);
            SetGameRules(playerLevel);
            if (gamemode == null && gameRule == null)
            {
                Destroy(buttons[1]);
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
        /// <summary>
        /// sets a gamemode in this object, so that OnSceneLoaded can set the correct gamemode when entering the scene
        /// </summary>
        /// <param name="gamemodeID">The gamemode we are setting</param>
        public void Setgamemode(int level)
        {
            gamemode = modeSetter.SetMode(level - 1);
        }
        /// <summary>
        /// sets a gamerule in this object, so that OnSceneLoaded can set the correct GameRule when entering the scene
        /// </summary>
        /// <param name="gameRuleID"></param>
        public void SetGameRules(int level)
        {
            gameRule = modeSetter.SetRules(level - 1);
        }

        public void SelfDestruct()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }

        public void OnClick(int mod)
        {
            Setgamemode(playerLevel + mod);
            SetGameRules(playerLevel + mod);
            sceneSwitcher();
        }
    }
}
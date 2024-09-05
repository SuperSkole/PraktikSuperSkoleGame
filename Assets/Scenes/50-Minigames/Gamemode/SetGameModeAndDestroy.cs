using CORE.Scripts;
using CORE.Scripts.Game_Rules;
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
        //tells the scene manager to call the OnSceneLoaded function whenever a scene is loaded
        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            switch(sceneID)
            {
                case 0:
                    modeSetter = new MonsterTowerSetter();
                    break;

                case 1:
                    modeSetter = new SymbolEaterSetter();
                    break;
                case 2:
                    modeSetter = new LetterGardenSetter();
                    break;
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
        public void Setgamemode(string gameModeID)
        {
            gamemode = modeSetter.SetMode(gameModeID);
        }
        /// <summary>
        /// sets a gamerule in this object, so that OnSceneLoaded can set the correct GameRule when entering the scene
        /// </summary>
        /// <param name="gameRuleID"></param>
        public void SetGameRules(string gameRuleID)
        {
            gameRule = modeSetter.SetRules(gameRuleID);
        }

        public void SelfDestruct()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
    }
}
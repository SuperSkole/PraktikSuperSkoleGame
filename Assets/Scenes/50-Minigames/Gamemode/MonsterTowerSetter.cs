using System.Collections.Generic;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._50_Minigames._65_MonsterTower.Scrips.MTGameModes;
using UnityEngine;

namespace Scenes._50_Minigames.Gamemode
{
    public class MonsterTowerSetter: IGameModeSetter
    {
        private List<string> gamemodes = new List<string>()
        {
            "",
            "",
            "",
            "level 4",
            "level 5",
        };


        private List<string> gamerules = new List<string>()
        {
            "",
            "",
            "",
            "",
            "",
        };
        /// <summary>
        /// returns a gamemode of the Monster Tower type
        /// </summary>
        /// <param name="mode">The mode we are looking for</param>
        /// <returns></returns>
        public IGenericGameMode SetMode(int level)
        {
            IMTGameMode modeReturned;
            string mode = gamemodes[level];
            switch (mode)
            {
                case "sentences":
                    modeReturned = new SentenceToPictures();
                    break;

                case "shoot picture":
                    modeReturned = new ShootPicture();
                    break;

                case "level 4":
                    modeReturned = new Level4();
                    
                    break;

                case "level 5":
                    modeReturned = new Level5();
                    break;

                default:
                    Debug.Log("given mode was not among expected options, returning null");
                    modeReturned = null;
                    break;
            }
            return modeReturned;
        }

        /// <summary>
        /// returns a gamerule set
        /// </summary>
        /// <param name="rules">The rules we are looking for</param>
        /// <returns></returns>
        public IGameRules SetRules(int level)
        {
            IGameRules rulesReturned;
            string rules = gamerules[level];
            switch(rules)
            {
                default:
                    Debug.Log("given ruleset was not among expected options, returning null");
                    rulesReturned = null;
                    break;
            }

            return rulesReturned;
        }
    }
}
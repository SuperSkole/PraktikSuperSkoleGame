using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scenes.Minigames.MonsterTower.Scrips.MTGameModes;
using CORE.Scripts.GameRules;
using CORE.Scripts;

namespace Scenes.GameMode
{
    public class MonsterTowerSetter: IGameModeSetter
    {
        /// <summary>
        /// returns a gamemode of the Monster Tower type
        /// </summary>
        /// <param name="mode">The mode we are looking for</param>
        /// <returns></returns>
        public IGenericGameMode SetMode(string mode)
        {
            IMTGameMode modeReturned;
            switch (mode)
            {
                case "sentences":
                    modeReturned = new SentenceToPictures();
                    break;



                default:
                    Debug.Log("given mode was not among expected options, setting to default mode");
                    modeReturned = new SentenceToPictures();
                    break;
            }
            return modeReturned;
        }

        /// <summary>
        /// returns a gamerule set
        /// </summary>
        /// <param name="rules">The rules we are looking for</param>
        /// <returns></returns>
        public IGameRules SetRules(string rules)
        {
            IGameRules rulesReturned;
            
            switch(rules)
            {
                default:
                    Debug.Log("given ruleset was not among expected options, setting to default ruleset");
                    rulesReturned = new SpellWord();
                    break;
            }

            return rulesReturned;
        }
    }
}
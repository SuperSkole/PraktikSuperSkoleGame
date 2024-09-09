using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._50_Minigames._65_MonsterTower.Scrips.MTGameModes;
using UnityEngine;

namespace Scenes._50_Minigames.Gamemode
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

                case "shoot picture":
                    modeReturned = new ShootPicture();
                    break;

                case "shoot vowel":
                    modeReturned = new ShootVowel();

                    break;
                case "level 4":
                    modeReturned = new Level4();

                    break;
                case "level 5":
                    modeReturned = new Level5();

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
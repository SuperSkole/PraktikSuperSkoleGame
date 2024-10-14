using System.Collections.Generic;
using Analytics;
using CORE;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Letters;
using Scenes._50_Minigames._65_MonsterTower.Scrips.MTGameModes;
using UnityEngine;

namespace Scenes._50_Minigames.Gamemode
{
    public class MonsterTowerSetter: IGameModeSetter
    {
        public List<IGenericGameMode> gameModes = new List<IGenericGameMode>()
        {
            null,
            null,
            null,
            new Level4(),
            new Level5()
        };


        public List<IGenericGameMode> letterGameModes = new List<IGenericGameMode>()
        {
            null,
            null,
            null,
            new Level4(),
            new Level5()
        };

        public List<IGenericGameMode> wordGameModes = new List<IGenericGameMode>()
        {
            null,
            null,
            null,
            new Level4_Words(),
            new Level5_Words()
        };


        private List<IGameRules> gamerules = new List<IGameRules>()
        {
            null,
            null,
            null,
            null,
            null,
        };

        public (IGameRules, IGenericGameMode) DetermineGamemodeAndGameRulesToUse(int level)
        {
            GameManager.Instance.PerformanceWeightManager.SetEntityWeight("ko", 60);
            List<ILanguageUnit> languageUnits = GameManager.Instance.DynamicDifficultyAdjustmentManager.GetNextLanguageUnitsBasedOnLevel(80);

            ILanguageUnit languageUnit = GameManager.Instance.DynamicDifficultyAdjustmentManager.GetNextLanguageUnitsBasedOnLevel(1)[0];
            IGenericGameMode mode = null;

            switch (languageUnit.LanguageUnitType)
            {
                case LanguageUnit.Letter:
                    mode = letterGameModes[Random.Range(3, 5)];
                    break;
                case LanguageUnit.Word:
                    mode = wordGameModes[3];
                    break;
            }

            

            LetterData letterData = (LetterData)languageUnits[0];
            if (GameManager.Instance.PlayerData.PlayerLanguageLevel < 2 && (letterData.Category == LetterCategory.Consonant || letterData.Category == LetterCategory.Vowel))
            {
                List<ILanguageUnit> letters = new List<ILanguageUnit>();
                foreach (var item in languageUnits)
                {
                    if (item.LanguageUnitType == LanguageUnit.Letter)
                    {
                        if (letterData.Category == LetterCategory.Consonant || letterData.Category == LetterCategory.Vowel)
                        {
                            letters.Add(item);
                        }
                    }
                }

                if(letters.Count<3)
                {
                    return (null, null);
                }


            }
           


            return (null, mode);
     
        }


        /// <summary>
        /// returns a gamemode of the Monster Tower type
        /// </summary>
        /// <param name="level">The playerlevel used as index on the gamemode list</param>
        /// <returns></returns>
        public IGenericGameMode SetMode(int level)
        {
            if(letterGameModes.Count > level && level >= 0)
            {
                return letterGameModes[level];
            }
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a gamemode based on a given string
        /// </summary>
        /// <param name="gamemode">the string representation of the given gamemode</param>
        /// <returns>the desired gamemode or the default one if the desired gamemode could not be found</returns>
        public IGenericGameMode SetMode(string gamemode)
        {
            IMTGameMode modeReturned;
            switch (gamemode)
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

                case "sentences words":
                    modeReturned = new SentenceToPictures_Words();
                break;

                case "shoot picture words":
                    modeReturned = new ShootPicture_Words();
                    break;
                default:
                    Debug.Log("given mode was not among expected options, returning default gamemode");
                    modeReturned = new SentenceToPictures();
                    break;
            }
            return modeReturned;
        }

        /// <summary>
        /// returns a gamerule set
        /// </summary>
        /// <param name="level">The level to use as index for the desired gamerules</param>
        /// <returns></returns>
        public IGameRules SetRules(int level)
        {
            if(gamerules.Count > level && level >= 0)
            {
                return gamerules[level];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// returns a gamerule set
        /// </summary>
        /// <param name="rules">The rules we are looking for</param>
        /// <returns>the desired gamerules. Otherwise returns the default set</returns>
        public IGameRules SetRules(string gamerules)
        {
            IGameRules rulesReturned;
            switch(gamerules)
            {
                default:
                    Debug.Log("given ruleset was not among expected options, returning default gamerules");
                    rulesReturned = new SpellWord();
                    break;
            }
            return rulesReturned;
        }



    }
}
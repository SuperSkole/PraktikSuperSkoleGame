using CORE;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._50_Minigames._58_MiniRacingGame.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scenes._50_Minigames.Gamemode
{

    public class WordProdutionLineSetter : IGameModeSetter
    {



        public (IGameRules, IGenericGameMode) DetermineGamemodeAndGameRulesToUse(int level)
        {
            GameManager.Instance.PerformanceWeightManager.SetEntityWeight("ko", 60);

            if (GameManager.Instance.DynamicDifficultyAdjustmentManager.GetNextLanguageUnitsBasedOnLevel(1)[0].LanguageUnitType == Analytics.LanguageUnit.Word)
            {
                return (new DynamicGameRules(), null);
            }
            //WordproductionLine only supports Words
            else
            {
                return (null, null);
            }
        }

        public IGenericGameMode SetMode(int level)
        {
            return null;
        }

        public IGenericGameMode SetMode(string gamemode)
        {
            return null;
        }

        public IGameRules SetRules(int level)
        {
            return null;
        }

        public IGameRules SetRules(string gamerules)
        {
            return null;
        }

    }

}
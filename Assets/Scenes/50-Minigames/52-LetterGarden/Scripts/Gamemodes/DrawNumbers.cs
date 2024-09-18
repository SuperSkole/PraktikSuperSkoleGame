
using System.Collections.Generic;
using Scenes.Minigames.LetterGarden.Scripts;
using System.Linq;
using UnityEngine;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;

namespace Scenes.Minigames.LetterGarden.Scripts.Gamemodes {
    /// <summary>
    /// A LettergardenGamemode implementation where the player should draw random numbers
    /// </summary>
    public class DrawNumbers : LettergardenGameMode
    {
        /// <summary>
        /// creates a list of SplineSymbolDataHolders of a given length
        /// </summary>
        /// <param name="amount">How many elements should be in the list</param>
        /// <returns>A list of SplineSymbolDataHolders</returns>
        public List<SplineSymbolDataHolder> GetSymbols(int amount, IGameRules gameRules)
        {
            List<SplineSymbolDataHolder> result = new List<SplineSymbolDataHolder>();
            List<int>usedNumbers = new List<int>();
            //Adds a given amount of random numbers to the result list.
            for (int i = 0; i < amount; i++){
                int randomIndex = Random.Range(0, 10);
                while(usedNumbers.Contains(randomIndex)){
                    randomIndex = Random.Range(0, 10);
                }
                usedNumbers.Add(randomIndex);
                result.Add(new SplineSymbolDataHolder(SymbolManager.numbersObjects[randomIndex], SymbolManager.numbers[randomIndex], System.Convert.ToChar(randomIndex)));
            }
            return result;
        }

        public void SetUpGameModeDescription(ActiveLetterHandler activeLetterHandler)
        {
            activeLetterHandler.descriptionText.text = "L\u00e6r at tegne tal. Tryk og hold nede imens du f\u00f8lger biens bane";
        }

        public bool UseBee()
        {
            return true;
        }

    }
}
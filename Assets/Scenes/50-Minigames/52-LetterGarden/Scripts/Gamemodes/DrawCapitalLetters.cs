
using System.Collections.Generic;
using Scenes.Minigames.LetterGarden.Scripts;
using System.Linq;
using UnityEngine;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;

namespace Scenes.Minigames.LetterGarden.Scripts.Gamemodes {
    /// <summary>
    /// A LettergardenGamemode implementation where the player should draw random capital letters
    /// </summary>
    public class DrawCapitalLetters : LettergardenGameMode
    {
        /// <summary>
        /// creates a list of SplineSymbolDataHolders of a given length
        /// </summary>
        /// <param name="amount">How many elements should be in the list</param>
        /// <returns>A list of SplineSymbolDataHolders</returns>
        public List<SplineSymbolDataHolder> GetSymbols(int amount, IGameRules gameRules)
        {
            List<SplineSymbolDataHolder> result = new List<SplineSymbolDataHolder>();
            List<char> usedLetters = new List<char>();
            //Adds a given amount of random capital letters to the result list.
            for (int i = 0; i < amount; i++){
                char randomIndex = LetterManager.GetRandomLetter();
                while(usedLetters.Contains(randomIndex)){
                    randomIndex = LetterManager.GetRandomLetter();
                }
                usedLetters.Add(randomIndex);
                result.Add(new SplineSymbolDataHolder(SymbolManager.capitalLettersObjects[randomIndex], SymbolManager.capitalLetters[randomIndex], randomIndex));
            }
            return result;
        }

        public void SetUpGameModeDescription(ActiveLetterHandler activeLetterHandler)
        {
            activeLetterHandler.descriptionText.text = "L�r at tegne store bogstaver. Tryk og hold nede imens du f�lger biens bane";
        }

        public bool UseBee()
        {
            return true;
        }

    }
}
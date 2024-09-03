
using System.Collections.Generic;
using Scenes.Minigames.LetterGarden.Scripts;
using System.Linq;
using UnityEngine;
using CORE.Scripts;

namespace Scenes.Minigames.LetterGarden.Scripts.Gamemodes {
    /// <summary>
    /// A LettergardenGamemode implementation where the player should draw random lowercase letters
    /// </summary>
    public class DrawLowercaseLetters : LettergardenGameMode
    {
        /// <summary>
        /// creates a list of SplineSymbolDataHolders of a given length
        /// </summary>
        /// <param name="amount">How many elements should be in the list</param>
        /// <returns>A list of SplineSymbolDataHolders</returns>
        public List<SplineSymbolDataHolder> GetSymbols(int amount)
        {
            List<SplineSymbolDataHolder> result = new List<SplineSymbolDataHolder>();
            //Adds a given amount of random lowercase letters to the result list.
            for (int i = 0; i < amount; i++){
                char randomIndex = LetterManager.GetRandomLetter();
                result.Add(new SplineSymbolDataHolder(SymbolManager.lowercaseLettersObjects[randomIndex], SymbolManager.lowercaseLetters[randomIndex], randomIndex));
            }
            return result;
        }
    }
}
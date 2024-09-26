
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
    public class DrawWithOrWithOutBee : LettergardenGameMode
    {
        /// <summary>
        /// creates a list of SplineSymbolDataHolders of a given length
        /// </summary>
        /// <param name="amount">How many elements should be in the list</param>
        /// <returns>A list of SplineSymbolDataHolders</returns>
        public List<SplineSymbolDataHolder> GetSymbols(int amount, IGameRules gameRules)
        {
            List<SplineSymbolDataHolder> result = new List<SplineSymbolDataHolder>();
            List<string> usedLetters = new List<string>();
            //Adds a given amount of random letters to the result list based on the given game rules.
            for (int i = 0; i < amount; i++)
            {
                string letter = gameRules.GetCorrectAnswer();
                
                while(usedLetters.Contains(letter))
                {
                    letter = gameRules.GetCorrectAnswer();
                }
                usedLetters.Add(letter);
                if(Random.Range(0, 2) == 0)
                {
                    result.Add(new SplineSymbolDataHolder(SymbolManager.capitalLettersObjects[letter[0]], SymbolManager.capitalLetters[letter[0]], letter[0]));
                }
                else
                {
                    result.Add(new SplineSymbolDataHolder(SymbolManager.lowercaseLettersObjects[letter[0]], SymbolManager.lowercaseLetters[letter[0]], letter.ToLower()[0]));
                }
            }
            return result;
        }

        public void SetUpGameModeDescription(ActiveLetterHandler activeLetterHandler)
        {
            activeLetterHandler.descriptionText.text = "Tegn bogstaver. F\u00F8lg bien med musen eller lyt til lyden af bogstavet \n Tryk på [Mellemrum] for at høre bogstavet du skal tegne";
        }

        public bool UseBee()
        {
            return 0 == Random.Range(0, 2);
        }

    }
}
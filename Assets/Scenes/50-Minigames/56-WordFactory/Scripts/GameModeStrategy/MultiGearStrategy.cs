using System;
using System.Collections.Generic;
using System.Linq;
using CORE.Scripts;
using Scenes._50_Minigames._56_WordFactory.Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.GameModeStrategy
{
    /// <summary>
    /// Strategy for handling game modes with more than 2 gears.
    /// </summary>
    public class MultiGearStrategy : IGearStrategy
    {
        public List<List<char>> GetLettersForGears()
        {
            int numberOfGears = WordFactoryGameManager.Instance.GetNumberOfGears();
            int numberOfTeeth = WordFactoryGameManager.Instance.GetNumberOfTeeth();
            
            var words = WordFactoryGameManager.Instance.WordList;
            
            //Debug.Log("chosen words: " + string.Join(", ", words));

            if (words.Count < numberOfGears)
            {
                Debug.LogError("Not enough valid words available.");
                return null;
            }

            List<List<char>> gearLetters = SplitWordsIntoLetters(words, numberOfGears);

            FillRemainingLetters(gearLetters, numberOfTeeth);

            return gearLetters;
        }

        private List<List<char>> SplitWordsIntoLetters(List<string> words, int numberOfGears)
        {
            List<List<char>> gearLetters = new List<List<char>>();
            for (int i = 0; i < numberOfGears; i++)
            {
                gearLetters.Add(new List<char>());
            }

            foreach (var word in words)
            {
                for (int gearIndex = 0; gearIndex < numberOfGears; gearIndex++)
                {
                    if (gearIndex < word.Length)
                    {
                        gearLetters[gearIndex].Add(word[gearIndex]);
                    }
                    else
                    {
                        gearLetters[gearIndex].Add(LetterManager.GetRandomLetters(1).First());
                    }
                }
            }

            return gearLetters;
        }
        
        private void FillRemainingLetters(List<List<char>> gearLetters, int numberOfTeeth)
        {
            // Set to track used letters to avoid duplicates
            HashSet<char> usedLetters = new HashSet<char>();
        
            // Add the already used letters from gearLetters to the set
            foreach (var gear in gearLetters)
            {
                foreach (var letter in gear)
                {
                    usedLetters.Add(letter);
                }
            }
        
            foreach (var gear in gearLetters)
            {
                while (gear.Count < numberOfTeeth)
                {
                    char newLetter;
                    do
                    {
                        // Generate a new letter
                        newLetter = LetterManager.GetRandomLetters(1).First();
                    } while (usedLetters.Contains(newLetter)); 
        
                    // Add the new unique letter to the gear and track it
                    gear.Add(newLetter);
                    usedLetters.Add(newLetter);
                }
            }
        }
    }
}

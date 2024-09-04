using System.Collections.Generic;
using System.Linq;
using CORE.Scripts;
using Scenes._05_Minigames._56_WordFactory.Scripts;
using Scenes._05_Minigames._56_WordFactory.Scripts.Managers;
using UnityEngine;

namespace Scenes._05_Minigames.WordFactory.Scripts
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
            int difficulty = WordFactoryGameManager.Instance.GetDifficultyLevel();

            int numberOfWords = numberOfTeeth - difficulty;

            List<string> words = WordsManager.GetRandomWordsByLengthAndCount(numberOfGears, numberOfWords);

            if (words.Count < numberOfWords)
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
            foreach (var gear in gearLetters)
            {
                while (gear.Count < numberOfTeeth)
                {
                    gear.Add(LetterManager.GetRandomLetters(1).First());
                }
            }
        }
    }
}

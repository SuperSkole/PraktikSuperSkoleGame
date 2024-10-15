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

            // usedLettersPerGear is a list of HashSets, each containing the letters already used for each gear
            List<HashSet<char>> usedLettersPerGear = new List<HashSet<char>>();
            for (int i = 0; i < numberOfGears; i++)
            {
                usedLettersPerGear.Add(new HashSet<char>());
            }

            foreach (var word in words)
            {
                for (int gearIndex = 0; gearIndex < numberOfGears; gearIndex++)
                {
                    if (gearIndex < word.Length)
                    {
                        char letter = word[gearIndex];
                
                        // check if the letter is already used
                        if (!usedLettersPerGear[gearIndex].Contains(letter))
                        {
                            gearLetters[gearIndex].Add(letter);
                            usedLettersPerGear[gearIndex].Add(letter);
                        }
                        else
                        {
                            // if the letter is already used, add a random letter
                            char newLetter;
                            do
                            {
                                newLetter = LetterManager.GetRandomLetters(1).First();
                            } while (usedLettersPerGear[gearIndex].Contains(newLetter));

                            gearLetters[gearIndex].Add(newLetter);
                            usedLettersPerGear[gearIndex].Add(newLetter);
                        }
                    }
                    else
                    {
                        // if the word is shorter than the number of gears, add random letters
                        char newLetter;
                        do
                        {
                            newLetter = LetterManager.GetRandomLetters(1).First();
                        } while (usedLettersPerGear[gearIndex].Contains(newLetter));

                        gearLetters[gearIndex].Add(newLetter);
                        usedLettersPerGear[gearIndex].Add(newLetter);
                    }
                }
            }

            return gearLetters;
        }

        
        private void FillRemainingLetters(List<List<char>> gearLetters, int numberOfTeeth)
        {
            foreach (var gear in gearLetters)
            {
                // Create a HashSet and include all letters already assigned from words
                HashSet<char> usedLettersOnGear = new HashSet<char>(gear);
                
                // Add all already assigned letters to the HashSet
                foreach (char letter in gear)
                {
                    usedLettersOnGear.Add(letter);
                }

                // Fill the remaining teeth with unique letters
                while (gear.Count < numberOfTeeth)
                {
                    // Generate a new random letter
                    char newLetter;

                    // Keep trying until we find a unique letter for the current gear
                    do
                    {
                        newLetter = LetterManager.GetRandomLetters(1).First();
                    } while (usedLettersOnGear.Contains(newLetter));

                    // Add the letter to the gear and mark it as used
                    gear.Add(newLetter);
                    usedLettersOnGear.Add(newLetter); // Track letters for this gear
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Analytics;
using Letters;
using Scenes._10_PlayerScene.Scripts;
using Unity;
using UnityEngine;
using Words;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace CORE.Scripts.Game_Rules 
{
    public class DynamicGameRules : IGameRules
    {
        private int maxVowelLevel = 1;
        string correctAnswer;
        string word;
        int index;
        int remainingLetterIndex = 1;
        private List<char> wrongAnswerList;

        private List<ILanguageUnit> languageUnits;
        private List<ILanguageUnit> languageUnitsList;

        private bool usesSequence = false;
        
        /// <summary>
        /// Retrieves a correct answer
        /// </summary>
        /// <returns>a correct answer</returns>
        public string GetCorrectAnswer()
        {
            //gets a random letter from the languageunits list if it contains more than one element
            if(languageUnits[0].LanguageUnitType == LanguageUnit.Letter)
            {
                if(languageUnits.Count > 1)
                {
                    return languageUnits[Random.Range(0, languageUnits.Count)].Identifier;
                }
            }
            return  correctAnswer;
        }

        /// <summary>
        /// Returns a string used in information display in minigames
        /// </summary>
        /// <returns>an information string</returns>
        public string GetDisplayAnswer()
        {
            string displayString = "";
            //Sets the display string to the correctanswer if it is a letter and there are no extra letters is in languageUnits. In that case it returns the type of letter 
            if(languageUnits[0].LanguageUnitType == LanguageUnit.Letter)
            {

                displayString = correctAnswer;
                LetterData letterData = (LetterData)languageUnits[0];
                
                if(languageUnits.Count > 1 && letterData.ErrorCategory == LetterCategory.Consonant)
                {
                    displayString = "vokaler";
                }
                else if(languageUnits.Count > 1 && letterData.ErrorCategory == LetterCategory.Vowel)
                {
                    displayString = "konsonanter";
                }
            }
            //for now returns the word to ensure compatability with existing gamemodes but should be removed once the GetSecondaryAnswer() is properly implemented
            else if(languageUnits[0].LanguageUnitType == LanguageUnit.Word)
            {
                return word;
            }
            return displayString;
        }

        /// <summary>
        /// Returns a wrong answer
        /// </summary>
        /// <returns>a wrong answer of the specified type</returns>
        public string GetWrongAnswer()
        {
            //Returns a letter from the wronganswerlist if a word is currently not used
            if(!usesSequence)
            {
                return wrongAnswerList[Random.Range(0, wrongAnswerList.Count)].ToString();
            }
            //Returns a letter from the current word tat is not the one the player is looking for
            else if(remainingLetterIndex < word.Length)
            {
                char letter = word[remainingLetterIndex];
                remainingLetterIndex++;
                return letter.ToString();
            }
            //If all letters from the list has been added a random letter is added which is not the correctanswer
            else 
            {
                char answer = wrongAnswerList[Random.Range(0, wrongAnswerList.Count)];
                while(answer.ToString() == correctAnswer)
                {
                    answer = wrongAnswerList[Random.Range(0, wrongAnswerList.Count)];
                }
                return answer.ToString();
            }
        }

        /// <summary>
        /// Checks if a symbol is the correct one
        /// </summary>
        /// <param name="symbol">The symbol to checked</param>
        /// <returns>Whether it is the correct symbol</returns>
        public bool IsCorrectSymbol(string symbol)
        {
            //if a sequence is not used it returns whether the lowered versions of the symbol and the correctanswer is equal to each other
            if(!usesSequence && (symbol.ToLower() == correctAnswer.ToLower() || LanguageUnitsContainsIdentifier(symbol)))
            {
                return true;
            }
            //if a sequence is used it returns true after moving on to the next letter in the word
            else if(usesSequence && symbol.ToLower() == correctAnswer.ToLower())
            {
                index++;
                if(index < word.Length)
                {
                    correctAnswer = word[index].ToString();
                }
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Returns whether the sequence is complete
        /// </summary>
        /// <returns>whether the sequence is complete</returns>
        public bool SequenceComplete()
        {
            return index >= word.Length;
        }

        /// <summary>
        /// Sets the correct answer based on input from the DDA
        /// </summary>
        public void SetCorrectAnswer()
        {
            
            languageUnitsList = GameManager.Instance.DynamicDifficultyAdjustmentManager
                    .GetNextLanguageUnitsBasedOnLevel(10);
            
            if(languageUnits == null)
            {
                languageUnits = new List<ILanguageUnit>();
            }
            else {
                languageUnits.Clear();
            }
            languageUnits.Add(languageUnitsList[Random.Range(0, 10)]);
            //Checks which mode the gamerules should run in
            switch(languageUnits[0].LanguageUnitType)
            {
                //if a letter it sets up the correct answer and determines which type of letter to use for wrong letters
                case LanguageUnit.Letter:
                    LetterData letterData = (LetterData)languageUnits[0];
                    correctAnswer = letterData.Identifier;
                    DetermineWrongLetterCategory(letterData.ErrorCategory);
                    break;
                //If a word it sets up the word and retrieves the first letter of it and sets up the wrong answer list
                case LanguageUnit.Word:
                    usesSequence = true;
                    WordData wordData = (WordData)languageUnits[0];
                    correctAnswer = wordData.Identifier[0].ToString();
                    word = wordData.Identifier;
                    wrongAnswerList = LetterRepository.GetAllLetters().ToList();
                    break;
                case LanguageUnit.Sentence:
                default:
                    Debug.LogError("Unknown Language Unit");
                    break;
            }
        }

        /// <summary>
        /// Determines which lettercategory to use for wronganswers
        /// </summary>
        /// <param name="letterCategory">the lettercategory to use for wrong letters</param>
        private void DetermineWrongLetterCategory(LetterCategory letterCategory)
        {
            switch(letterCategory)
            {
                //for consonants and vowels if the player is low enough level it also sets up so correct answer looks for a random correct letter
                case LetterCategory.Consonant:
                    wrongAnswerList = LetterRepository.GetConsonants().ToList();
                    languageUnits = languageUnitsList;
                    break;
                case LetterCategory.Vowel:
                    wrongAnswerList = LetterRepository.GetConsonants().ToList();
                    languageUnits = languageUnitsList;
                    break;
                case LetterCategory.All:
                    wrongAnswerList = LetterRepository.GetAllLetters().ToList();
                    wrongAnswerList.Remove(correctAnswer[0]);
                    break;
                default:
                    Debug.LogError("unknown letter category");
                    break;
            }
        }

        public string GetSecondaryAnswer()
        {
            if(languageUnits.Count > 1)
            {
                string res = "";
                foreach(ILanguageUnit languageUnit in languageUnits)
                {
                    res += languageUnit.Identifier;
                }
                return res;
            }
            return word;
        }


        private bool LanguageUnitsContainsIdentifier(string identifier)
        {
            foreach(ILanguageUnit languageUnit in languageUnits)
            {
                if(languageUnit.Identifier.ToLower() == identifier.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
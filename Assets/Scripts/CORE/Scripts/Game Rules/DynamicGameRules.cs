
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Analytics;
using Letters;
using Unity;
using UnityEngine;
using Words;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace CORE.Scripts.Game_Rules 
{
    public class DynamicGameRules : IGameRules
    {
        string correctAnswer;
        string word;
        int index;
        int remainingLetterIndex = 1;
        private List<char> wrongAnswerList;

        private List<ILanguageUnit> languageUnits;

        private bool usesSequence = false;
        
        public string GetCorrectAnswer()
        {
            if(languageUnits[0].LanguageUnitType == LanguageUnit.Letter)
            {
                LetterData letterData = (LetterData)languageUnits[0];
                if((letterData.Category == LetterCategory.Vowel || letterData.Category == LetterCategory.Consonant) && languageUnits.Count > 0)
                {
                    return languageUnits[Random.Range(0, languageUnits.Count)].Identifier;
                }
            }
            return  correctAnswer;
        }

        public string GetDisplayAnswer()
        {
            string displayString = "";
            if(languageUnits[0].LanguageUnitType == LanguageUnit.Letter)
            {
                displayString = correctAnswer;
            }
            else if(languageUnits[0].LanguageUnitType == LanguageUnit.Word)
            {
                return word;
            }
            return displayString;
        }

        public string GetWrongAnswer()
        {
            if(!usesSequence)
            {
                return wrongAnswerList[Random.Range(0, wrongAnswerList.Count)].ToString();
            }
            else if(remainingLetterIndex < word.Length)
            {
                char letter = word[remainingLetterIndex];
                remainingLetterIndex++;
                return letter.ToString();
            }
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

        public bool IsCorrectSymbol(string symbol)
        {
            if(!usesSequence && (symbol.ToLower() == correctAnswer.ToLower() || LanguageUnitsContainsIdentifier(symbol)))
            {
                return true;
            }
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

        public bool SequenceComplete()
        {
            return index >= word.Length;
        }

        public void SetCorrectAnswer()
        {
            
            List<ILanguageUnit> languageUnitsList = GameManager.Instance.DynamicDifficultyAdjustmentManager
                    .GetNextLanguageUnitsBasedOnLevel(10);
            if(languageUnits == null)
            {
                languageUnits = new List<ILanguageUnit>();
            }
            else {
                languageUnits.Clear();
            }
            languageUnits.Add(languageUnitsList[Random.Range(0, 10)]);
            switch(languageUnits[0].LanguageUnitType)
            {
                case LanguageUnit.Letter:
                    LetterData letterData = (LetterData)languageUnits[0];
                    correctAnswer = letterData.Identifier;
                    DetermineWrongLetterCategory(letterData.ErrorCategory);
                    break;
                case LanguageUnit.Word:
                    usesSequence = true;
                    WordData wordData = (WordData)languageUnits[0];
                    correctAnswer = wordData.Identifier[0].ToString();
                    word = wordData.Identifier;
                    wrongAnswerList = LetterRepository.GetAllLetters().ToList(); // wordrepository
                    break;
                case LanguageUnit.Sentence:
                default:
                    Debug.LogError("Unknown Language Unit");
                    break;
            }
        }

        private void DetermineWrongLetterCategory(LetterCategory letterCategory)
        {
            switch(letterCategory)
            {
                case LetterCategory.Consonant:
                    wrongAnswerList = LetterRepository.GetConsonants().ToList();
                    break;
                case LetterCategory.Vowel:
                    wrongAnswerList = LetterRepository.GetConsonants().ToList();
                    break;
                case LetterCategory.All:
                    Debug.Log("error letter category was all");
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
            return word;
        }

        public string GetRandomCorrectAnswer()
        {
            return "";
        }

        private bool LanguageUnitsContainsIdentifier(string identifier)
        {
            foreach(ILanguageUnit languageUnit in languageUnits)
            {
                if(languageUnit.Identifier == identifier)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
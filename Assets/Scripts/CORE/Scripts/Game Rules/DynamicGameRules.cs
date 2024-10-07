
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
        
        public string GetCorrectAnswer()
        {
            if(languageUnits[0].LanguageUnitType == LanguageUnit.Letter)
            {
                if(languageUnits.Count > 1)
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
                    if(PlayerManager.Instance != null && PlayerManager.Instance.PlayerData.CurrentLevel <= maxVowelLevel)
                    {
                        languageUnits = languageUnitsList;
                    }
                    break;
                case LetterCategory.Vowel:
                    wrongAnswerList = LetterRepository.GetConsonants().ToList();
                    if(PlayerManager.Instance != null && PlayerManager.Instance.PlayerData.CurrentLevel <= maxVowelLevel)
                    {
                        languageUnits = languageUnitsList;
                    }
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
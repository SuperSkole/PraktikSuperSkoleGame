
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

        private bool usesSequence = false;
        
        public string GetCorrectAnswer()
        {
            return  correctAnswer;
        }

        public string GetDisplayAnswer()
        {
            return word;
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
            if(!usesSequence && symbol.ToLower() == correctAnswer.ToLower())
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
            GameManager.Instance.PerformanceWeightManager.SetEntityWeight("ø", 60);
            GameManager.Instance.PerformanceWeightManager.SetEntityWeight("X", 60);
            GameManager.Instance.PerformanceWeightManager.SetEntityWeight("ko", 60);
            List<ILanguageUnit> languageUnitList = GameManager.Instance.DynamicDifficultyAdjustmentManager
                    .GetNextLanguageUnitsBasedOnLevel(10);
            switch(languageUnitList[0].LanguageUnitType)
            {
                case LanguageUnit.Letter:
                    LetterData letterData = (LetterData)languageUnitList[0];
                    correctAnswer = letterData.Identifier;
                    DetermineWrongLetterCategory(letterData.ErrorCategory);
                    break;
                case LanguageUnit.Word:
                    usesSequence = true;
                    WordData wordData = (WordData)languageUnitList[0];
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
    }
}
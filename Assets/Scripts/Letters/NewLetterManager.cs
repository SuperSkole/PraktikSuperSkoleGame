using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CORE.Scripts
{
    public class NewLetterManager : ILetterManager
    {
        private readonly ILetterProvider letterProvider;
        private readonly IRandomLetterSelector randomLetterSelector;
        private readonly IWeightManager weightManager;
        
        public NewLetterManager()
        {
            letterProvider = new LetterProvider();
            randomLetterSelector = new RandomLetterSelector();
            weightManager = GameManager.Instance.WeightManager;
            //InitializeWeights();
        }

        // Dependency Injection Constructor
        public NewLetterManager(ILetterProvider letterProvider, IRandomLetterSelector randomLetterSelector, IWeightManager weightManager)
        {
            this.letterProvider = letterProvider;
            this.randomLetterSelector = randomLetterSelector;
            this.weightManager = weightManager;
            //InitializeWeights();
        }

        public List<char> GetRandomLetters(int count)
        {
            var allDanishLetters = letterProvider.GetAllLetters();
            return allDanishLetters.OrderBy(_ => Random.value).Take(count).ToList();
        }

        public char GetRandomLetter()
        {
            var letters = letterProvider.GetAllLetters();
            return randomLetterSelector.GetRandomLetter(letters);
        }

        public char GetWeightedVowel() { throw new System.NotImplementedException(); }

        public char GetWeightedConsonant() { throw new System.NotImplementedException(); }

        public char GetRandomVowel()
        {
            var vowels = letterProvider.GetVowels();
            return randomLetterSelector.GetRandomLetter(vowels);
        }

        public char GetRandomConsonant()
        {
            var consonants = letterProvider.GetConsonants();
            return randomLetterSelector.GetRandomLetter(consonants);
        }

        public char GetWeightedLetter()
        {
            var currentWeights = weightManager.GetCurrentWeights();
            return randomLetterSelector.GetWeightedRandomLetter(currentWeights);
        }

        public void InitializeWeights() { throw new System.NotImplementedException(); }

        public void SetLetterWeight(char letter, int weight) { throw new System.NotImplementedException(); }

        public void UpdateLetterWeight(char letter, bool isCorrect)
        {
            weightManager.UpdateWeight(letter, isCorrect);
        }

        public int GetLetterWeight(char letter) { throw new System.NotImplementedException(); }
    }
}
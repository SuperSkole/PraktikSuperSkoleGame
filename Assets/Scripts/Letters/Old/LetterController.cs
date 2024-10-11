// using System.Collections.Generic;
// using System.Linq;
// using Analytics;
// using CORE;
// using CORE.Scripts;
// using UnityEngine;
//
// namespace Letters
// {
//     public class LetterController : ILetterController
//     {
//         private readonly ILetterRepository letterRepository;
//         private readonly IRandomLetterSelector randomLetterSelector;
//         private readonly WeightManager weightManager;
//         
//         public LetterController()
//         {
//             letterRepository = new LetterRepository();
//             randomLetterSelector = new RandomLetterSelector();
//             weightManager = GameManager.Instance.WeightManager;
//             //InitializeWeights();
//         }
//
//         // Dependency Injection Constructor
//         public LetterController(ILetterRepository letterRepository, IRandomLetterSelector randomLetterSelector, WeightManager weightManager)
//         {
//             this.letterRepository = letterRepository;
//             this.randomLetterSelector = randomLetterSelector;
//             this.weightManager = weightManager;
//             //InitializeWeights();
//         }
//
//         public List<char> GetRandomLetters(int count)
//         {
//             var allDanishLetters = letterRepository.GetAllLetters();
//             return allDanishLetters.OrderBy(_ => Random.value).Take(count).ToList();
//         }
//
//         public char GetRandomLetter()
//         {
//             var letters = letterRepository.GetAllLetters();
//             return randomLetterSelector.GetRandomLetter(letters);
//         }
//
//         public char GetWeightedVowel() { throw new System.NotImplementedException(); }
//
//         public char GetWeightedConsonant() { throw new System.NotImplementedException(); }
//
//         public char GetRandomVowel()
//         {
//             var vowels = letterRepository.GetVowels();
//             return randomLetterSelector.GetRandomLetter(vowels);
//         }
//
//         public char GetRandomConsonant()
//         {
//             var consonants = letterRepository.GetConsonants();
//             return randomLetterSelector.GetRandomLetter(consonants);
//         }
//
//         public char GetWeightedLetter()
//         {
//             var currentWeights = weightManager.GetCurrentWeights();
//             return randomLetterSelector.GetWeightedRandomLetter(currentWeights);
//         }
//
//         public void InitializeWeights() { throw new System.NotImplementedException(); }
//
//         public void SetLetterWeight(char letter, int weight) { throw new System.NotImplementedException(); }
//
//         public void UpdateLetterWeight(char entity, bool isCorrect)
//         {
//             weightManager.UpdateWeight(entity, isCorrect);
//         }
//
//         public int GetLetterWeight(char letter) { throw new System.NotImplementedException(); }
//     }
// }
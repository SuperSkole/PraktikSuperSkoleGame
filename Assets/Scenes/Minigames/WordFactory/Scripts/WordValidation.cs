using CORE.Scripts;
using UnityEngine;

namespace Scenes.Minigames.WordFactory.Scripts
{
    public class WordValidation : MonoBehaviour
    {
        public bool IsValidWord(string word)
        {
            return LettersAndWordsManager.GetValidWords().Contains(word.ToUpper());
        }
    }
}
using UnityEngine;

namespace Scenes.Minigames.WordFactory.Scripts
{
    public class WordValidation : MonoBehaviour
    {
        public bool IsValidWord(string word)
        {
            return LetterAndWordCollections.GetValidWords().Contains(word.ToUpper());
        }
    }
}
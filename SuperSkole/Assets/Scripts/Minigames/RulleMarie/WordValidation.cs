using UnityEngine;

namespace Minigames.RulleMarie
{
    public class WordValidation : MonoBehaviour
    {
        public bool IsValidWord(string word)
        {
            return LetterAndWordCollections.GetValidWords().Contains(word.ToUpper());
        }
    }
}
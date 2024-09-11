using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts
{
    public static class WordBlockUtilities
    {
        /// <summary>
        /// Method to find the latest block based on its name including the specific word.
        /// </summary>
        /// <param name="word">The word associated with the block.</param>
        /// <returns>The GameObject of the block if found, otherwise null.</returns>
        public static GameObject FindCurrentBlock(string word)
        {
            string targetName = $"WordBlock-{word}";  
            
            // Find the block by its unique name
            GameObject block = GameObject.Find(targetName);  

            if (block == null)
            {
                Debug.LogError($"No block found for the word: {word}");
            }
            
            return block;
        }
    }
}
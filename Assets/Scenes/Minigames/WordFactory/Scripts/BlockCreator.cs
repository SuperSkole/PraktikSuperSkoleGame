using TMPro;
using UnityEngine;

namespace Scenes.Minigames.WordFactory.Scripts
{
    public class BlockCreator : MonoBehaviour
    {
        [SerializeField] private GameObject blockPrefab;
        [SerializeField] private Transform spawnPoint; 

        // Method to handle valid word event
        public void HandleValidWord(string validWord)
        {
            CreateBlock(validWord);
        }

        /// <summary>
        /// Creates a 3D block with the given word displayed on it.
        /// </summary>
        /// <param name="word">The word to display on the block.</param>
        public void CreateBlock(string word)
        {
            // Instantiate the block at the spawn point's position
            GameObject block = Instantiate(blockPrefab, spawnPoint.position, Quaternion.identity);
            
            // Find the Canvas component first
            Canvas canvas = block.GetComponentInChildren<Canvas>();
            if (canvas == null)
            {
                Debug.Log("Failed to find Canvas on block");
                return;
            }

            // Find the TextMeshPro component within the Canvas
            TextMeshProUGUI textMeshPro = canvas.GetComponentInChildren<TextMeshProUGUI>();
            if (textMeshPro == null)
            {
                Debug.Log("Failed to find TextMeshProUGUI on canvas on block");
                return;
            }

            if (textMeshPro != null)
            {
                textMeshPro.text = word;
            }
        }
    }
}
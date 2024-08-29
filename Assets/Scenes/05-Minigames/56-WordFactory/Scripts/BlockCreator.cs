using TMPro;
using UnityEngine;

namespace Scenes._05_Minigames._56_WordFactory.Scripts
{
    public class BlockCreator : MonoBehaviour
    {
        [SerializeField] private GameObject blockPrefab;
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private Transform spawnPoint; 
        public float letterSpacing = 1.0f;

        // Method to handle valid word event
        public void HandleValidWord(string validWord)
        {
            //BuildWord(validWord);
            CreateBlock(validWord);
        }
        
        /// <summary>
        /// Build a 3D word using TextMeshPro objects.
        /// </summary>
        /// <param name="word">The word to display in 3D.</param>
        public void BuildWord(string word)
        {
            // Clear previous word (if any)
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // Create each letter as a separate 3D TextMeshPro object
            for (int i = 0; i < word.Length; i++)
            {
                Vector3 position = spawnPoint.position + new Vector3(i * letterSpacing, 0, 0);
                GameObject letterObject = Instantiate(textPrefab, position, Quaternion.identity, transform);
            
                // Update the TextMeshPro component with the correct letter
                TextMeshPro textMeshPro = letterObject.GetComponent<TextMeshPro>();
                if (textMeshPro != null)
                {
                    textMeshPro.text = word[i].ToString();
                }
            }
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
using System.Collections.Generic;
using CORE.Scripts;
using Scenes._50_Minigames._56_WordFactory.Scripts.GameModeStrategy;
using Scenes._50_Minigames._56_WordFactory.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts
{
    public class LetterHandler : MonoBehaviour
    {
        [SerializeField] private WordValidator wordValidator;

        private IGearStrategy gearStrategy;

        private void Awake()
        {
            // Retrieve the gear strategy from the WordFactoryGameManager
            gearStrategy = WordFactoryGameManager.Instance.GetGearStrategy();

            if (gearStrategy == null)
            {
                Debug.LogError("Gear strategy is not initialized in LetterHandler.");
            }
        }

        public void DistributeLetters()
        {
            List<List<char>> gearLetters = GetLettersForGears();
            AssignLettersToGears(gearLetters);
        }

        public void ResetLetters()
        {
            List<List<char>> gearLetters = GetLettersForGears();
            AssignLettersToGears(gearLetters);

            // Reset the consonant block if using SingleGearStrategy
            if (WordFactoryGameManager.Instance.GetNumberOfGears() == 1)
            {
                ResetConsonantBlock();
            }
        }

        private void ResetConsonantBlock()
        {
            GearGenerator gearGenerator = FindObjectOfType<GearGenerator>();
            if (gearGenerator != null)
            {
                gearGenerator.ClearConsonantBlock();
            }
        }


        private List<List<char>> GetLettersForGears()
        {
            if (gearStrategy == null)
            {
                Debug.LogError("Gear strategy is null when attempting to get letters for gears.");
                return null;
            }
            
            return gearStrategy.GetLettersForGears();
        }

        private void AssignLettersToGears(List<List<char>> gearLetters)
        {
            if (gearLetters == null) return;

            List<GameObject> gears = WordFactoryGameManager.Instance.GetGears();
            for (int gearIndex = 0; gearIndex < gears.Count; gearIndex++)
            {
                GameObject gear = gears[gearIndex];
                var teethContainer = gear.transform.Find("TeethContainer");
                if (teethContainer != null)
                {
                    int toothIndex = 0;
                    foreach (Transform tooth in teethContainer)
                    {
                        if (toothIndex < gearLetters[gearIndex].Count)
                        {
                            AddLetterToTooth(tooth.gameObject, gearLetters[gearIndex][toothIndex]);
                        }
                        
                        toothIndex++;
                    }
                }
            }
        }

        private void AddLetterToTooth(GameObject tooth, char letter)
        {
            Canvas canvas = tooth.GetComponentInChildren<Canvas>();
            if (canvas == null)
            {
                Debug.Log("Failed to find Canvas on tooth");
                return;
            }

            TextMeshProUGUI textMeshPro = canvas.GetComponentInChildren<TextMeshProUGUI>();
            if (textMeshPro == null)
            {
                Debug.Log("Failed to find TextMeshProUGUI on tooth");
                return;
            }

            textMeshPro.rectTransform.localPosition = new Vector3(0, -1f, 0);
            textMeshPro.text = letter.ToString();
            textMeshPro.fontSize = 1;
            textMeshPro.color = Color.black;
        }
    }
}

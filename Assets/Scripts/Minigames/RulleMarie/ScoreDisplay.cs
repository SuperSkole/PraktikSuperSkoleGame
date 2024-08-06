using TMPro;
using UnityEngine;

namespace Minigames.RulleMarie
{
    public class ScoreDisplay : MonoBehaviour, IScoreDisplay
    {
        private TextMeshProUGUI scoreText;

        private void Awake()
        {
            scoreText = GetComponent<TextMeshProUGUI>();
            if (scoreText == null)
            {
                Debug.LogError("TextMeshProUGUI component is not found on the ScoreText GameObject.");
            }
        }

        public void UpdateScoreText(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score;
            }
        }
    }
}
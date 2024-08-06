using UnityEngine;

namespace Minigames.RulleMarie.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }
        [SerializeField] private GameObject scoreTextObject;
        private IScoreDisplay scoreDisplay;

        private int score;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            if (scoreTextObject != null)
            {
                scoreDisplay = scoreTextObject.GetComponent<IScoreDisplay>();
                if (scoreDisplay == null)
                {
                    Debug.LogError("IScoreDisplay component not found on ScoreText object.");
                }
            }
            else
            {
                Debug.LogError("ScoreText object is not assigned in the inspector.");
            }
        }

        public void AddScore(int points)
        {
            if (scoreDisplay == null)
            {
                Debug.LogError("ScoreDisplay is not initialized.");
                return;
            }

            score += points;
            scoreDisplay.UpdateScoreText(score);
        }
    }
}
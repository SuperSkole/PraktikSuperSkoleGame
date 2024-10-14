using CORE;
using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.Managers
{
    public class ScoreManager : PersistentSingleton<ScoreManager>
    {
        [SerializeField] private GameObject scoreTextObject;
        private IScoreDisplay scoreDisplay;

        private int score;

        protected override void Awake()
        {
            base.Awake(false);
            
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
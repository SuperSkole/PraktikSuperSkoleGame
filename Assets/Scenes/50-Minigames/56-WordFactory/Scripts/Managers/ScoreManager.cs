using CORE;
using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.Managers
{
    public class ScoreManager : PersistentSingleton<ScoreManager>
    {
        [SerializeField] private GameObject scoreTextObject;
        private IScoreDisplay scoreDisplay;

        private int score;

        // public static ScoreManager Instance { get; private set; }
        // private void Awake()
        // {
        //     if (Instance != null && Instance != this)
        //     {
        //         Destroy(gameObject);
        //     }
        //     else
        //     {
        //         Instance = this;
        //     }
        //
        //     if (scoreTextObject != null)
        //     {
        //         scoreDisplay = scoreTextObject.GetComponent<IScoreDisplay>();
        //         if (scoreDisplay == null)
        //         {
        //             Debug.LogError("IScoreDisplay component not found on ScoreText object.");
        //         }
        //     }
        //     else
        //     {
        //         Debug.LogError("ScoreText object is not assigned in the inspector.");
        //     }
        // }

        protected override void Awake()
        {
            base.Awake();
            
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
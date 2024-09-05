using UnityEngine;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    public class GameTimer : MonoBehaviour
    {
        public float TimeElapsed { get; private set; } = 0f;
        private bool isRunning = false;

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartTimer()
        {
            isRunning = true;
            TimeElapsed = 0f;
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void StopTimer()
        {
            isRunning = false;
        }

        /// <summary>
        /// Updates the timer.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void UpdateTimer(float deltaTime)
        {
            if (isRunning)
            {
                TimeElapsed += deltaTime;
            }
        }
    }
}

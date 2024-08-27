using UnityEngine;

namespace Scenes.Minigames.MiniRacingGame.Scripts
{
    public class GameTimer : MonoBehaviour
    {

        //Script is not yet implemented xD

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

        //public string GetFormattedTime()
        //{
        //    return string.Format("{0:00}:{1:00}.{2:000}", Mathf.FloorToInt(TimeElapsed / 60), Mathf.FloorToInt(TimeElapsed) % 60, Mathf.FloorToInt(TimeElapsed * 1000 % 1000));
        //}
    }
}

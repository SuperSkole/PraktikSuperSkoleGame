using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    public class SayWordAgain : MonoBehaviour
    {
        public delegate void TriggerWordHit();
        public static event TriggerWordHit OnWordTriggered;
        private bool exited = false;
        /// <summary>
        /// Sends a signal to have a word repeated when triggered
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("ActiveCar"))
            {
                if (!exited)
                {
                    exited = true;
                    OnWordTriggered();
                }
            }
        }

    }
}

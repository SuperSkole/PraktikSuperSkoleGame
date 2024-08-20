using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Minigames.MonsterTower.Scrips
{

    public class DeathTimer : MonoBehaviour
    {

        public float targetTime = 10;

        void Update()
        {

            targetTime -= Time.deltaTime;

            if (targetTime <= 0.0f)
            {
                TimerEnded();
            }

        }

        public void TimerEnded()
        {
            Destroy(gameObject);

        }

    
    }

}
using UnityEngine;

namespace Scenes._50_Minigames._65_MonsterTower.Scripts
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
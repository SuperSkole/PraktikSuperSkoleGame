using UnityEngine;

namespace Scenes._50_Minigames._65_MonsterTower.Scripts
{


    public class Brick : MonoBehaviour
    {
    // Start is called before the first frame update
        public Sprite sprite;
        public Sprite correctSprite;

        public bool checkCollision = false;
        public bool isShootable = false;
        public bool isCorrect = false;
  



        void Update()
        {
            if (checkCollision && isCorrect)
            {
                gameObject.GetComponentInParent<TowerManager>().correctAnswer = true;
                checkCollision = false;
            }
        }
    }

}
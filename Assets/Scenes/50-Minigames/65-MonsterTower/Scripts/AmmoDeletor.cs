using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes._50_Minigames._65_MonsterTower.Scrips
{


    public class AmmoDeletor : MonoBehaviour
    {
        [SerializeField] GameObject particals;
        Brick targetBrick;
        /// <summary>
        /// when anything enters the trigger it spawns the particals and destroyes this gameobject
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            Instantiate(particals,transform.position,Quaternion.identity);
            targetBrick.checkCollision = true;

            Destroy(gameObject);
        }

        public void Hitbox(Brick brick)
        {
            targetBrick = brick;
        }
    }

}



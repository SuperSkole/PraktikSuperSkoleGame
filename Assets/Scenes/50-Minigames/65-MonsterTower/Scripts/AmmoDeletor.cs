using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Minigames.MonsterTower.Scrips
{


    public class AmmoDeletor : MonoBehaviour
    {
        [SerializeField] GameObject particals;

        public TowerManager towerManager;
        Brick targetBrick;
        /// <summary>
        /// when anything enters the trigger it spawns the particals and destroyes this gameobject
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if(towerManager.rowToDelete==towerManager.towerHeight-1)
            {
                particals.transform.localScale = new Vector3(3, 3, 3);
                Instantiate(particals, transform.position, Quaternion.identity);
            }
            else
            {

                Instantiate(particals, transform.position, Quaternion.identity);
            }

         
            targetBrick.checkCollision = true;

            Destroy(gameObject);
        }

        public void Hitbox(Brick brick)
        {
            targetBrick = brick;
        }
    }

}



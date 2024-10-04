using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Scenes._50_Minigames._65_MonsterTower.Scripts;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Scenes._50_Minigames._65_MonsterTower.Scrips
{
    public class CatapultAming : MonoBehaviour
    {
        [SerializeField] GameObject prjectipePrefab;
        [SerializeField] GameObject catapult;
        [SerializeField] GameObject catapultArm;
        [SerializeField] Transform shootPos;
        [SerializeField] float hight = 1;
        [SerializeField] LayerMask placementLayermask;
        private GameObject ammoToShoot;
        bool isShooting = false;
        float rotateThisMuch = 0;
        static float rotateAmount = 5f;
        static float rotateSpeed = 100f;
        static float rotateActualSpeed = 1f / rotateSpeed;

        bool catapultIsLoaded=false;
        /// <summary>
        /// calcolates the terejectory to hit at point and returnes a velosity to hit it
        /// </summary>
        /// <param name="target">the target you want to hit</param>
        /// <returns></returns>
        Vector3 CalcolateTerejectory(Vector3 target)
        {
          
            Vector3 output;
            float grav = Physics.gravity.y;
            hight = 4;
            do
            {
                hight++;
                float displaymentY = target.y - shootPos.position.y;
                Vector3 displaymentXZ = new Vector3(target.x - shootPos.position.x, 0, target.z - shootPos.position.z);

                Vector3 velocetyY = Vector3.up * Mathf.Sqrt(-2 * grav * hight);
                Vector3 velocetyXZ = displaymentXZ / (Mathf.Sqrt(-2 * hight / grav) + Mathf.Sqrt(2 * (displaymentY - hight) / grav));
                output = velocetyXZ + velocetyY;
            }
            while (output.IsNaN());
            return output;
        }

        /// <summary>
        /// Sets an ammoBlock on the catapult
        /// </summary>
        /// <param name="block"></param>
        public void SetAmmo(GameObject block)
        {
           ammoToShoot=Instantiate(prjectipePrefab, shootPos.position, quaternion.identity);
            catapultIsLoaded = true;
        }

        /// <summary>
        /// the func that is called when we want to shoot at a target.
        /// it spawns and fires a projectile
        /// </summary>
        /// <param name="target">the target you want to hit</param>
        public IEnumerator Shoot(Vector3 target, Brick brick, MonsterTowerManager manager)
        {
            if (catapultIsLoaded == true)
            {
                if (!isShooting)
                {
                    

                    isShooting = true;
                    while (rotateThisMuch < 85)
                    {
                        catapultArm.gameObject.transform.Rotate(0, -rotateAmount, 0);
                        rotateThisMuch += rotateAmount;
                        yield return new WaitForSecondsRealtime(rotateActualSpeed);
                    }

                    manager.flyingProjectileSound.Play();
                    // sets the word being shot onto the brick projectile. 
                    for (int i = 0; i < prjectipePrefab.transform.childCount; i++)
                    {
                        prjectipePrefab.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = manager.words[manager.ammoCount - 1];

                    }
                    //destroying the displayprefab and launching the real ammo
                    Destroy(ammoToShoot);
                    ammoToShoot = Instantiate(prjectipePrefab, shootPos.position, quaternion.identity);
                    AmmoDeletor ammoDeleterComp = ammoToShoot.GetComponent<AmmoDeletor>();
                   
                    ammoDeleterComp.Hitbox(brick);
                    ammoDeleterComp.towerManager = manager.towerManager;

                    manager.RemoveAmmo();
                   
                  
                    Rigidbody rb = ammoToShoot.GetComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.velocity = CalcolateTerejectory(target);
                   
                    while (rotateThisMuch > 0)
                    {
                        catapultArm.gameObject.transform.Rotate(0, rotateAmount / 5f, 0);
                        rotateThisMuch -= rotateAmount / 5f;
                        yield return new WaitForSeconds(rotateActualSpeed);
                    }

                  

                    isShooting = false;
                    catapultIsLoaded = false;
                    manager.ammoLoaded = false;
                }
                yield return null;
            }
        }
    }

}
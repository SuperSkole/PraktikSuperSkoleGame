using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Scenes.Minigames.MonsterTower.Scrips
{
    public class CatapultAming : MonoBehaviour
    {
        [SerializeField] GameObject prjectipePrefab;
        [SerializeField] GameObject catapult;
        [SerializeField] GameObject catapultArm;
        [SerializeField] Transform shootPos;
        [SerializeField] float hight = 1;
        bool isShooting = false;
        float rotateThisMuch = 0;
        static float rotateAmount = 5f;
        static float rotateSpeed = 100f;
        static float rotateActualSpeed = 1f / rotateSpeed;
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
        /// the func that is called when we want to shoot at a target.
        /// it spawns and fires a projectile
        /// </summary>
        /// <param name="target">the target you want to hit</param>
        public IEnumerator Shoot(Vector3 target, Brick brick, MonsterTowerManager manager)
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

                for (int i = 0; i < prjectipePrefab.transform.childCount; i++)
                {
                    prjectipePrefab.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "word test";

                }

                manager.RemoveAmmo();
                GameObject temp = Instantiate(prjectipePrefab, shootPos.position, quaternion.identity);
                Rigidbody rb = temp.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.velocity = CalcolateTerejectory(target);
                temp.GetComponent<AmmoDeletor>().Hitbox(brick);

                while (rotateThisMuch > 0)
                {
                    catapultArm.gameObject.transform.Rotate(0, rotateAmount / 5f, 0);
                    rotateThisMuch -= rotateAmount / 5f;
                    yield return new WaitForSeconds(rotateActualSpeed);
                }
                isShooting = false;
            }
            yield return null;
        }
    }

}
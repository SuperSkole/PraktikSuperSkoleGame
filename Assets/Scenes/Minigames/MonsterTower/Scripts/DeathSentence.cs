using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Minigames.MonsterTower.Scrips
{

    public class DeathSentence : MonoBehaviour
    {
        [SerializeField] float ageLimit;
        float age;

        /// <summary>
        /// counts up untill the agelimit and then destroyes this gameobject
        /// </summary>
        void Update()
        {
            if (age > ageLimit)
            {
                Destroy(gameObject);
                return;
            }

            age += Time.deltaTime;
        }
    }

}
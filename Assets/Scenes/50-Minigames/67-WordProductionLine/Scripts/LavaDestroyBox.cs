using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scenes._50_Minigames._67_WordProductionLine.Scripts
{

    public class LavaDestroyBox : MonoBehaviour
    {
        [SerializeField]
        private ProductionLineObjectPool objectPool;

        /// <summary>
        /// Lava pool that deactivate boxes when the collide with the lava.
        /// </summary>
        /// <param name="other"></param>
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "ProductionCube")
            {
                objectPool.ResetCube(other.gameObject);
            }
        }
    }
}
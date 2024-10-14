using Assets.Scenes._50_Minigames._67_WordProductionLine.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scenes._50_Minigames._67_WordProductionLine.Scripts
{

    public class LavaDestroyBox : MonoBehaviour
    {

        [SerializeField]
        private AudioClip cubeSmelt;

        /// <summary>
        /// Lava pool that deactivate boxes when the collide with the lava.
        /// </summary>
        /// <param name="other"></param>
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("ProductionCube"))
            {
                other.gameObject.GetComponentInChildren<IBox>().ResetCube();
                ProductionLineManager.ResetCubes(other.gameObject);
                ProductionLineManager.ResetLine();

                AudioManager.Instance.PlaySound(cubeSmelt, SoundType.SFX);
            }
        }
    }
}
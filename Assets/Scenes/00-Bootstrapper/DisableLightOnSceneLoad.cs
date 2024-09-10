using System.Collections;
using UnityEngine;

namespace Scenes._00_Bootstrapper
{
    public class DisableLightOnSceneLoad : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(WaitASec());
        }

        private IEnumerator WaitASec()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}

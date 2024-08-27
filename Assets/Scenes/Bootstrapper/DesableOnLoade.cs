using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

public class DesableOnLoade : MonoBehaviour
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

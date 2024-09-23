using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDestroyBox : MonoBehaviour
{

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "ProductionCube")
        {
            other.gameObject.SetActive(false);
        }
    }
}

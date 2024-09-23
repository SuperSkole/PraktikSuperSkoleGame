using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDestroyBox : MonoBehaviour
{
    /// <summary>
    /// Lava pool that deactivate boxes when the collide with the lava.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "ProductionCube")
        {
            other.gameObject.SetActive(false);
        }
    }
}

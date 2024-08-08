using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDeletor : MonoBehaviour
{
    [SerializeField] GameObject particals;

    /// <summary>
    /// when anything enters the trigger it spawns the particals and destroyes this gameobject
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(particals,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}

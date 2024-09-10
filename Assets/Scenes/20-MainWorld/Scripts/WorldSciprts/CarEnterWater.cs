using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEnterWater : MonoBehaviour
{
    [SerializeField] private Transform CarRespawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            transform.position = CarRespawnPoint.position;
        }    
    }

}

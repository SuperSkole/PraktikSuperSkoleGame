using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaser : MonoBehaviour
{

    [SerializeField] GameObject laserPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Instantiates a laser if the spacebar is pressed
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 spawnPos = transform.GetChild(0).transform.position;

            Instantiate(laserPrefab, spawnPos, transform.rotation);
        }

        
    }
}

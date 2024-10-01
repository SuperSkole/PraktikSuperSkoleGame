using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaser : MonoBehaviour
{

    [SerializeField] GameObject laserPrefab;

    private float shootCooldown = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shootCooldown -= Time.deltaTime;
        //Instantiates a laser after the cooldown time if the spacebar is pressed
        if(Input.GetKeyDown(KeyCode.Space) && shootCooldown<=0)
        {
            Vector3 spawnPos = transform.GetChild(0).transform.position;

            Instantiate(laserPrefab, spawnPos, transform.rotation);

            shootCooldown = 1;
        }

        
    }
}

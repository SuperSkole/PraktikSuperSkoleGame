using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject hexagon;

    [SerializeField] GameObject pentagon;

    [SerializeField] GameObject square;

    [SerializeField] GameObject triangle;

    float timer=0;

    float spawnFrequency = 5;

    int polygonIndex;

    Vector3 spawnPosition;
    void Start()
    {
        spawnPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer>=spawnFrequency)
        {
             polygonIndex=Random.Range(0, 4);

            switch (polygonIndex)
            {
                case 0:
                    Instantiate(triangle, spawnPosition, Quaternion.identity, gameObject.transform);
                    break;
                case 1:
                    Instantiate(square, spawnPosition, Quaternion.identity, gameObject.transform);
                    break;

                case 2:
                    Instantiate(pentagon, spawnPosition, Quaternion.identity, gameObject.transform);
                    break;
                case 3:
                    Instantiate(hexagon,spawnPosition,Quaternion.identity, gameObject.transform);
                    break;

            }
            timer = 0;
        }
        
    }
}

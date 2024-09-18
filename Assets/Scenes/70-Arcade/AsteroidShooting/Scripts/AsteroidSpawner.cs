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

    [SerializeField] float minXForce;
    [SerializeField] float maxXForce;
    [SerializeField] float minYForce;
    [SerializeField] float maxYForce;
    [SerializeField] float speed;
    float timer=0;

    float spawnFrequency = 10;

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
            timer = 0;
            polygonIndex =Random.Range(0, 4);
            Vector3 randomForce = new Vector3(Random.Range(minXForce, maxXForce), Random.Range(minYForce, maxYForce), 0);

            
            switch (polygonIndex)
            {
                case 0:
                   
                   var spawnedTriangle= Instantiate(triangle, spawnPosition, Quaternion.identity, gameObject.transform);
                    spawnedTriangle.GetComponent<Rigidbody2D>().AddForce(randomForce * speed);
                    break;
                case 1:
                    var spawnedSquare=Instantiate(square, spawnPosition, Quaternion.identity, gameObject.transform);
                    spawnedSquare.GetComponent<Rigidbody2D>().AddForce(randomForce * speed);
                    break;

                case 2:
                    var spawnedPentagon=Instantiate(pentagon, spawnPosition, Quaternion.identity, gameObject.transform);
                    spawnedPentagon.GetComponent<Rigidbody2D>().AddForce(randomForce * speed);
                    break;
                case 3:
                    var spawnedHexagon=Instantiate(hexagon,spawnPosition,Quaternion.identity, gameObject.transform);
                    spawnedHexagon.GetComponent<Rigidbody2D>().AddForce(randomForce * speed);
                    break;

            }
            
        }
        
    }
}

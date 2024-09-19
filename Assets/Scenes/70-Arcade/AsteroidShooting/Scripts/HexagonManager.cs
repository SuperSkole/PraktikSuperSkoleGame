using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonManager : MonoBehaviour
{


    [SerializeField] GameObject pentagon;

    [SerializeField] GameObject explosionPrefab;

    public AsteroidSpawner asteroidSpawner;

    [SerializeField] float minXForce;
    [SerializeField] float maxXForce;
    [SerializeField] float minYForce;
    [SerializeField] float maxYForce;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        minXForce = 0.5f;
        maxXForce = 1;
        minYForce = -1;
        maxYForce = 1;
        speed = 3000;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="PlayerProjectile")
        {
            asteroidSpawner.score += 25;
            Instantiate(explosionPrefab, gameObject.transform.position, transform.rotation, transform.parent);
           var spawnedPentagon= Instantiate(pentagon,gameObject.transform.position, transform.rotation,transform.parent);
            Vector3 randomForce = new Vector3(Random.Range(minXForce, maxXForce), Random.Range(minYForce, maxYForce), 0);
            spawnedPentagon.GetComponent<Rigidbody2D>().AddForce(randomForce*speed);
            spawnedPentagon.GetComponent<PentagonManager>().asteroidSpawner = asteroidSpawner;

            var spawnedPentagon2 = Instantiate(pentagon, gameObject.transform.position, transform.rotation, transform.parent);
            Vector3 randomForce2 = new Vector3(randomForce.x, -randomForce.y, 0);
            spawnedPentagon2.GetComponent<Rigidbody2D>().AddForce(randomForce2 * speed);
            spawnedPentagon2.GetComponent<PentagonManager>().asteroidSpawner = asteroidSpawner;

            Destroy(gameObject);

            Destroy(collision.gameObject);
            
        }
    }
}

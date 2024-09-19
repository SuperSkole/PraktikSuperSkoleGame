using Scenes._50_Minigames._65_MonsterTower.Scrips;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareManager : MonoBehaviour
{

    [SerializeField] GameObject triangle;
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
        speed = 7000;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="PlayerProjectile")
        {
            asteroidSpawner.score += 75;
            Instantiate(explosionPrefab, gameObject.transform.position, transform.rotation, transform.parent);
            var spawnedTriangle= Instantiate(triangle,gameObject.transform.position, transform.rotation,transform.parent);
            Vector3 randomForce = new Vector3(Random.Range(minXForce, maxXForce), Random.Range(minYForce, maxYForce), 0);
            spawnedTriangle.GetComponent<Rigidbody2D>().AddForce(randomForce*speed);
            spawnedTriangle.GetComponent<TriangleManager>().asteroidSpawner = asteroidSpawner;

            var spawnedTriangle2 = Instantiate(triangle, gameObject.transform.position, transform.rotation, transform.parent);
            Vector3 randomForce2 = new Vector3(randomForce.x, -randomForce.y, 0);
            spawnedTriangle2.GetComponent<Rigidbody2D>().AddForce(randomForce2 * speed);
            spawnedTriangle2.GetComponent<TriangleManager>().asteroidSpawner = asteroidSpawner;

            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}

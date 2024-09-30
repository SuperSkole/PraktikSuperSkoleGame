using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonManager : MonoBehaviour
{


 
    [SerializeField] GameObject triangle;

    [SerializeField] GameObject explosionPrefab;

    public AsteroidGameManager gameManager;

    [SerializeField] float minXForce;
    [SerializeField] float maxXForce;
    [SerializeField] float minYForce;
    [SerializeField] float maxYForce;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {

        //setting som standard values for the random ranges of forces on the X and Y axes.
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

    /// <summary>
    /// When colliding with a projectile two new asteroids with one less angle than the current are spawned.
    /// The score is also updated and the old asteroid and the projectile that hit it is destroyed
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag=="PlayerProjectile")
        {
            gameManager.score += 25;

            for (int i = 0; i < 6; i++)
            {
                SpawnTriangle(minXForce, maxXForce, minYForce, maxYForce);
            }

            Destroy(gameObject);

            Destroy(collision.gameObject);
            
        }
    }

    void SpawnTriangle(float minXForce, float maxXForce, float minYForce,float maxYForce)
    {
        Instantiate(explosionPrefab, gameObject.transform.position, transform.rotation, transform.parent);
        var spawnedTriangle = Instantiate(triangle, gameObject.transform.position, transform.rotation, transform.parent);
        Vector3 randomForce = new Vector3(Random.Range(minXForce, maxXForce), Random.Range(minYForce, maxYForce), 0);
        spawnedTriangle.GetComponent<Rigidbody2D>().AddForce(randomForce * speed);
        spawnedTriangle.GetComponent<TriangleManager>().gameManager = gameManager;

    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagonManager : MonoBehaviour
{


    [SerializeField] GameObject square;
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

        minXForce = 0.5f;
        maxXForce = 1;
        minYForce = -1;
        maxYForce = 1;
        speed = 5000;

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
            gameManager.score += 50;
            Instantiate(explosionPrefab, gameObject.transform.position, transform.rotation, transform.parent);
            var spawnedSquare = Instantiate(square,gameObject.transform.position, transform.rotation,transform.parent);
            Vector3 randomForce = new Vector3(Random.Range(minXForce, maxXForce), Random.Range(minYForce, maxYForce), 0);
            spawnedSquare.GetComponent<Rigidbody2D>().AddForce(randomForce*speed);
            spawnedSquare.GetComponent<SquareManager>().gameManager = gameManager;


            var spawnedSquare2 = Instantiate(triangle, gameObject.transform.position, transform.rotation, transform.parent);
            Vector3 randomForce2 = new Vector3(randomForce.x, -randomForce.y, 0);
            spawnedSquare2.GetComponent<Rigidbody2D>().AddForce(randomForce2 * speed);
            spawnedSquare2.GetComponent<SquareManager>().gameManager = gameManager;

            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}

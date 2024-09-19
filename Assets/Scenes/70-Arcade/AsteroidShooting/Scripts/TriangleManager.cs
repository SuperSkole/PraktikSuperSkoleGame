using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleManager : MonoBehaviour
{

  
    [SerializeField] GameObject explosionPrefab;
    public AsteroidSpawner asteroidSpawner;
    // Start is called before the first frame update
    void Start()
    {
     

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="PlayerProjectile")
        {
            asteroidSpawner.score += 100;
            Instantiate(explosionPrefab, gameObject.transform.position, transform.rotation, transform.parent);

            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}

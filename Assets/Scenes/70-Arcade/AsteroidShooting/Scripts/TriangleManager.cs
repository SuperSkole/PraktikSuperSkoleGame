using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleManager : MonoBehaviour
{

  
    [SerializeField] GameObject explosionPrefab;
    public AsteroidGameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
     

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    /// <summary>
    /// When a player projectile hits the triangle it updates the score, instantiates an explosion and destroys itself and the projectile. 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="PlayerProjectile")
        {
            gameManager.score += 100;
            Instantiate(explosionPrefab, gameObject.transform.position, transform.rotation, transform.parent);

            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}

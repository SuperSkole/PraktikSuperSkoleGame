using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAsteroidContact : MonoBehaviour
{
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] GameObject explosionPrefab;

    public int lifePoints = 3;

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

        if(collision.gameObject.tag=="Asteroid")
        {
            Instantiate(explosionPrefab, gameObject.transform.position, transform.rotation, transform.parent);

            lifePoints -= 1;
            if (lifePoints > 0)
            {
                gameObject.transform.position = spawnPoint;
            }
            else
            {
                Destroy(gameObject);
            }


        }
    }
}

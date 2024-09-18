using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonManager : MonoBehaviour
{


    [SerializeField] GameObject pentagon;

    [SerializeField] GameObject explosionPrefab;
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
            Instantiate(explosionPrefab, gameObject.transform.position, transform.rotation, transform.parent);
            Instantiate(pentagon,gameObject.transform.position, transform.rotation,transform.parent);
            
            Destroy(gameObject);

            Destroy(collision.gameObject);
            
        }
    }
}

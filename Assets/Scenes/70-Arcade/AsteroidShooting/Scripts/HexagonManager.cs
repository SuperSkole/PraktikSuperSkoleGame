using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonManager : MonoBehaviour
{


    [SerializeField] GameObject pentagon;

    [SerializeField] GameObject square;

    [SerializeField] GameObject triangle;

    int projectileHitAmount=0;

    SpriteRenderer spriteRenderer;




    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="PlayerProjectile")
        {
            switch(projectileHitAmount)
            {
                case 0:
                    
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }
    }
}

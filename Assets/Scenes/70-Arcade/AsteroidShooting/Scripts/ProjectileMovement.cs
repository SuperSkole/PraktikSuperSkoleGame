using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    Rigidbody2D rigidbody;
    [SerializeField] int projectileSpeed;
    // Start is called before the first frame update
    void Start()
    {
        // A force is added based on the transform.up vector. 
        rigidbody = GetComponent<Rigidbody2D>();
        projectileSpeed = 10000;

        rigidbody.AddForce(new Vector2(transform.up.x, transform.up.y) * projectileSpeed);
    }
    
    private void FixedUpdate()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
       
        // setup so when the projectile is outside of the cameraview it destroys itself. 
        Vector3 viewPortPoint=Camera.main.WorldToViewportPoint(transform.position);

        if(viewPortPoint.x<0 || viewPortPoint.x > 1)
        {
            Destroy(gameObject);
        }
       
        if(viewPortPoint.y<0 || viewPortPoint.y>1)
        {
            Destroy(gameObject);
        }


       
    }

    /// <summary>
    /// Making sure that playerprojectile can't do physics collisions with other playerprojectiles. 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="PlayerProjectile")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CapsuleCollider2D>(),gameObject.GetComponent<CapsuleCollider2D>());
        }

      

    }
}

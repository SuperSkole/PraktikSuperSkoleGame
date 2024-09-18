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
}

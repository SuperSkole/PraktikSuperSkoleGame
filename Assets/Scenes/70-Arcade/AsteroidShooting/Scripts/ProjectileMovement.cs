using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    Rigidbody rigidbody;
    [SerializeField] int projectileSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        projectileSpeed = 100;
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(transform.up * projectileSpeed);
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

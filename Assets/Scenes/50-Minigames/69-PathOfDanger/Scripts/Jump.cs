using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Jump : MonoBehaviour
{
    public Rigidbody rigidbody;

    [SerializeField] float yForce=10;

    private bool isJumping = false;

    private bool canJump = true;

    public PathOfDangerManager manager;

    public GameObject shadowPrefab;

    private GameObject spawnedShadow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    private void Update()
    {
        //checks spacebar input and if canJump is true which is only the case when the player is colliding with another collider. 
        // That makes is there to make sure you can't jump mid air. 
        if (Input.GetKeyDown(KeyCode.Space) && canJump == true)
        {
            isJumping = true;
            canJump = false;
           

            
        }
        if(spawnedShadow!=null)
        {
            spawnedShadow.transform.position = new Vector3(transform.position.x,spawnedShadow.transform.position.y,transform.position.z);

        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            manager.PlaySoundFromHearLetterButton();
        }
       
    }

    void FixedUpdate()
    {
        if(isJumping==true)
        { 
            rigidbody.AddForce(new Vector3(0, yForce, 0),ForceMode.Impulse);
            isJumping = false;

            CastShadow();

        }
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
        if(spawnedShadow!=null)
        {
            Destroy(spawnedShadow);
        }
    }

    void CastShadow()
    {
        Ray ray = new Ray(gameObject.transform.position,new Vector3(0,-1,0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            var posOffset = new Vector3(0, 0.5f, 0);
            
            spawnedShadow=Instantiate(shadowPrefab, hit.point+posOffset,shadowPrefab.gameObject.transform.rotation);
        }
        
    }
}

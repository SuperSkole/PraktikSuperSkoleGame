using Scenes._10_PlayerScene.Scripts;
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
    private Vector3 posOffset;
    private GameObject spawnedShadow;

    [SerializeField] float zOffset=-0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    private void Update()
    {
        //checks spacebar input and if canJump is true which is only the case when the player is colliding with another collider. 
        // That makes is there to make sure you can't jump mid air. 
        if (Input.GetKeyDown(KeyCode.Space) && canJump == true && manager.hasAnsweredWrong==false)
        {
            isJumping = true;
            canJump = false;
           

            
        }

        // Makes sure the spawned shadow is following the player in the x and z plane. 
        if(spawnedShadow!=null)
        {
            Vector3 centerPos = manager.spawnedPlayer.transform.GetChild(0).position;
            var pos = new Vector3(centerPos.x, spawnedShadow.transform.position.y, centerPos.z);

            spawnedShadow.transform.position = pos;

        }

        // Plays the current sound in the hearLetter button. 
        if(Input.GetKeyDown(KeyCode.F))
        {
            manager.PlaySoundFromHearLetterButton();
        }
       
    }

    void FixedUpdate()
    {
        // Adds a force to the players rigidbody and cast a shadow on the ground. 
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
        //Destroys the shadown when colliding with an object. 
        if(spawnedShadow!=null)
        {
            Destroy(spawnedShadow);
        }
    }


    /// <summary>
    /// Instantiates a shadowprefab on the position where the raycast hits. The raycast is shooting along the y axis in a downwars direction. 
    /// </summary>
    void CastShadow()
    {
        Ray ray = new Ray(gameObject.transform.position,new Vector3(0,-1,0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
           Vector3 centerPos= manager.spawnedPlayer.transform.GetChild(0).position;
            var pos = new Vector3(centerPos.x, hit.point.y+0.5f,centerPos.z);
            
            spawnedShadow=Instantiate(shadowPrefab, pos,shadowPrefab.gameObject.transform.rotation);
        }
        
    }
}

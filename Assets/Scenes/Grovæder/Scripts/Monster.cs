using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for monster chasing the player in the grovæder game
/// </summary>
public class Monster : MonoBehaviour
{
    /// <summary>
    /// Whether the monster can attempt to walk towards the player
    /// </summary>
    private bool canWalk = true;

    /// <summary>
    /// The player the monster tries to catch
    /// </summary>
    [SerializeField]private GameObject playerObject;
    /// <summary>
    /// The time in seconds between the monsters attempts to move toward the player
    /// </summary>
    [SerializeField]private float walkDelay = 1.5f;

    [SerializeField]private int throwRange = 5;

    /// <summary>
    /// The point the monster is currently moving towards.
    /// </summary>
    [SerializeField]Vector3 currentDestination;
    private float speed = 0.5f;

    private bool throwingPlayer = false;

    private bool releasingPlayer = false;

    private Vector3 playerDestination;

    private Player player;

    private float throwAngle = 0;

    private MeshRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        currentDestination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Throws the player if they have reached the center of the tile.

        //Finds the direction towards the player and then moves a tile towards the player on either the x or z axis. Afterwards the monster waits before attempting again
        if (canWalk && currentDestination == transform.position){
            Vector3 velocity = playerObject.transform.position - transform.position;
            if((velocity.x < 1 && velocity.x > -1) || (Random.Range(0, 2) == 0)){
                velocity.x = 0;
                if(velocity.z > 0){
                    velocity.z = 1;
                }
                else{
                    velocity.z = -1;
                }
            }
            else{
                velocity.z = 0;
                if(velocity.x > 0){
                    velocity.x = 1;
                }
                else{
                    velocity.x = -1;
                }
            }
            velocity.y = 0;
            currentDestination = velocity + transform.position;
            Move();
            //StartCoroutine(WalkDelay());
        }
        else{
            Move();
        }
    }


    /// <summary>
    /// Checks if the monster has begun overlapping with the player
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other){
        if(player == null){
            player = playerObject.GetComponent<Player>();
        }
        if(renderer == null){
            renderer = gameObject.GetComponent<MeshRenderer>();
        }
        if(other.gameObject.tag == "Player"){
            ThrowPlayer();
            //canWalk = false;
        }
    }


    /// <summary>
    /// Throws the player towards a random point on the board
    /// </summary>
    void ThrowPlayer(){
        if(!throwingPlayer && !releasingPlayer){
            int xDirection = 1;
            int zDirection = 1;
            if(Random.Range(0,2) == 0){
                xDirection = -1;
            }
            if(Random.Range(0,2) == 0){
                zDirection = -1;
            }
            Vector3 newDeltaPos = new Vector3(xDirection * Random.Range(0, throwRange), 0, zDirection * Random.Range(0, throwRange));
            Vector3 newPos = newDeltaPos + player.CurrentDestination;
            
            //Position correction if the destination is outside the board
            if(newPos.x > 19.5f){
                newPos.x = 19.5f;
            }
            if(newPos.z > 19.5f){
                newPos.z = 19.5f;
            }
            if(newPos.x < 10.5f){
                newPos.x = 10.5f;
            }
            if(newPos.z < 10.5f){
                newPos.z = 10.5f;
            }

            
            player.CurrentDestination = newPos;
            //throwingPlayer = true;
            //player.thrown = true;
        }
        if(throwingPlayer){
            throwAngle += Time.deltaTime * player.speed; // update angle
            Vector3 direction = Quaternion.AngleAxis(throwAngle, Vector3.forward) * Vector3.up; // calculate direction from center - rotate the up vector Angle degrees clockwise
            player.CurrentDestination = transform.position + direction * Vector3.Distance(transform.position, playerObject.transform.position); // update position based on center, the direction, and the radius (which is a constant)
        }
    }

    /// <summary>
    /// Moves the monster
    /// </summary>
    void Move(){
        if(currentDestination != transform.position){
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, step);
        }
    }
}


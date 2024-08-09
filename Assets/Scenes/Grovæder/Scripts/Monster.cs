using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class for monster chasing the player in the grovæder game
public class Monster : MonoBehaviour
{
    //Whether the monster can attempt to walk towards the player
    private bool canWalk = true;

    //The player the monster tries to catch
    [SerializeField]private GameObject playerObject;
    //The time in seconds between the monsters attempts to move toward the player
    [SerializeField]private float walkDelay = 1.5f;

    [SerializeField]private int throwRange = 5;

    //The point the monster is currently moving towards.
    [SerializeField]Vector3 currentDestination;
    private float speed = 0.5f;

    private bool overlappingWithPlayer = false;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        currentDestination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Throws the player if they have reached the center of the field.
        if(overlappingWithPlayer && playerObject.transform.position == player.CurrentDestination){
            overlappingWithPlayer = false;
            ThrowPlayer();
        }
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


    //Checks if the monster has begun overlapping with the player
    void OnTriggerEnter(Collider other){
        if(player == null){
            player = playerObject.GetComponent<Player>();
        }
        if(other.gameObject.tag == "Player"){
            overlappingWithPlayer = true;
        }
    }

    //Check if the monster has stopped overlapping with the player
    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            overlappingWithPlayer = false;
        }
    }

    //Throws the player towards a random point on the board
    void ThrowPlayer(){
        Debug.Log("Throwing player");
        int xDirection = 1;
        int zDirection = 1;
        if(Random.Range(0,2) == 0){
            xDirection = -1;
        }
        if(Random.Range(0,2) == 0){
            zDirection = -1;
        }
        Vector3 newDeltaPos = new Vector3(xDirection * Random.Range(0, throwRange), 0, zDirection * Random.Range(0, throwRange));
        Vector3 newPos = newDeltaPos + playerObject.transform.position;
        
        //Position correction if the destination is outside the board
        if(newPos.x > 19.5f){
            newDeltaPos.x = 19.5f - playerObject.transform.position.x;
        }
        if(newPos.z > 19.5f){
            newDeltaPos.z = 19.5f - playerObject.transform.position.z;
        }
        if(newPos.x < 10.5f){
            newDeltaPos.x = playerObject.transform.position.x - 10.5f;
        }
        if(newPos.z < 10.5f){
            newDeltaPos.z = playerObject.transform.position.z - 10.5f;
        }

        playerObject.transform.Translate(newDeltaPos);
        
        player.CurrentDestination = playerObject.transform.position;
    }

    //Moves the monster
    void Move(){
        if(currentDestination != transform.position){
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, step);
        }
    }
}


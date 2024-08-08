using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manager for the monster in the groværder game
public class MonsterManager : MonoBehaviour
{
    //Whether the monster can attempt to walk towards the player
    private bool canWalk = true;

    //The player the monster tries to catch
    [SerializeField]private GameObject player;
    //The time in seconds between the monsters attempts to move toward the player
    [SerializeField]private float walkDelay = 1.5f;

    [SerializeField]private int throwRange = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Finds the direction towards the player and then moves a tile towards the player on either the x or z axis. Afterwards the monster waits before attempting again
        if (canWalk){
            Vector3 velocity = player.transform.position - transform.position;
            Debug.Log(velocity);
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
            Debug.Log(velocity);
            transform.Translate(velocity);
            StartCoroutine(WalkDelay());
        }
    }

    //Handling of the delay between monster moves
    IEnumerator WalkDelay(){
        canWalk = false;
        yield return new WaitForSeconds(walkDelay);
        canWalk = true;
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player" ){
            ThrowPlayer();
        }
    }

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
        Vector3 newPos = newDeltaPos + player.transform.position;
        if(newPos.x > 19.5f){
            newDeltaPos.x = 19.5f - player.transform.position.x;
        }
        if(newPos.z > 19.5f){
            newDeltaPos.z = 19.5f - player.transform.position.z;
        }
        if(newPos.x < 10.5f){
            newDeltaPos.x = player.transform.position.x - 10.5f;
        }
        if(newPos.z < 10.5f){
            newDeltaPos.z = player.transform.position.z - 10.5f;
        }
        player.transform.Translate(newDeltaPos);
    }
}

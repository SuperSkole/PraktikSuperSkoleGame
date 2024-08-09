using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

//Player class for the grovæder game
public class Player : MonoBehaviour
{
    //How much time remains before the player is allowed to move
    [SerializeField]private float MoveDelayRemaining = 0;

    //Gameobject containing the cooldown text
    [SerializeField]private GameObject textObject;

    private TextMeshPro cooldownText;

    //The point the player currently is moving towards
    private Vector3 currentDestination;

    

    private float speed = 2;

    
    public Vector3 CurrentDestination { get => currentDestination; set => currentDestination = value; }

    // Start is called before the first frame update
    void Start()
    {
        cooldownText = textObject.GetComponent<TextMeshPro>();
        cooldownText.text = "";
        currentDestination = transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(MoveDelayRemaining == 0 && currentDestination == transform.position){
            if(Input.GetKeyDown(KeyCode.W) && transform.position.x < 19.5f){
                currentDestination = transform.position + new Vector3(1, 0, 0);
            }
            else if(Input.GetKeyDown(KeyCode.S) && transform.position.x > 10.5f){
                currentDestination = transform.position + new Vector3(-1, 0, 0);
            }
            else if(Input.GetKeyDown(KeyCode.A) && transform.position.z < 19.5f){
                currentDestination = transform.position + new Vector3(0, 0, 1);
            }
            else if(Input.GetKeyDown(KeyCode.D) && transform.position.z > 10.5f){
                currentDestination = transform.position + new Vector3(0, 0, -1);
            }
            
        }
        else if (currentDestination != transform.position){
            Move();
        }
        //Code to count down time remaining on the cooldown and to update the display
        else{
            MoveDelayRemaining -= Time.deltaTime;
            if(MoveDelayRemaining < 0){
                MoveDelayRemaining = 0;
                cooldownText.text = "";
            }
            else {
                cooldownText.text = MoveDelayRemaining + " sek. tilbage";
            }
        }
    }

    //code to start the delay in the players movement.
    public void IncorrectGuess(){
        MoveDelayRemaining = 6;
        cooldownText.text = MoveDelayRemaining + " sek. tilbage";
    }

    //Moves the player towards their destination.
    void Move(){
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentDestination, step);
    }
}

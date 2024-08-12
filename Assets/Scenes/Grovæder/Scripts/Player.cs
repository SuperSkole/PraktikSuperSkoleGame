using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Player class for the grovæder game
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// How much time remains before the player is allowed to move
    /// </summary>
    [SerializeField]private float MoveDelayRemaining = 0;

    /// <summary>
    /// Gameobject containing the cooldown text
    /// </summary>
    [SerializeField]private GameObject textObject;

    private TextMeshPro cooldownText;

    /// <summary>
    /// The point the player currently is moving towards
    /// </summary>
    private Vector3 currentDestination;

    public bool thrown = false;

    public bool hasMoved = false;    

    public float speed = 2;

    
    public Vector3 CurrentDestination { get => currentDestination; set => currentDestination = value; }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        cooldownText = textObject.GetComponent<TextMeshPro>();
        cooldownText.text = "";
        currentDestination = transform.position;
    }
    
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {

        if(MoveDelayRemaining == 0 && currentDestination == transform.position && !thrown){
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
            if(!hasMoved){
                hasMoved = true;
            }
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

    /// <summary>
    /// code to start the delay in the players movement.
    /// </summary>
    public void IncorrectGuess(){
        MoveDelayRemaining = 6;
        cooldownText.text = MoveDelayRemaining + " sek. tilbage";
    }

    /// <summary>
    /// Moves the player towards their destination.
    /// </summary>
    void Move(){
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentDestination, step);
    }
}

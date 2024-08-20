using System;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Player class for the Symbol Eater mini game
/// </summary>
public class Player : MonoBehaviour
{

    private int livesRemaining = 3;

    private int maxLivesRemaining;

    [SerializeField]private float IncorrectSymbolStepMoveDelayRemaining = 0;

    [SerializeField]private GameObject cooldownTextObject;

    [SerializeField]private GameObject healthTextObject;

    private TextMeshProUGUI healthText;

    private TextMeshPro cooldownText;

    private Vector3 currentDestination;

    public bool thrown = false;

    public bool hasMoved = false;    

    public float speed = 2;

    public bool hasMoveDelay = false;

    public bool canMove = true;

    public float maxIncorrectSymbolStepMoveDelayRemaining = 6;
    
    public Vector3 CurrentDestination { get => currentDestination; set => currentDestination = value; }


    /// <summary>
    /// Property of livesRemaining. if used to setting the value it cant be below 0 and it also updates the lives remaining text
    /// </summary>
    public int LivesRemaining 
    { 
        get => livesRemaining; 
        set 
        {
            if(value > 0)
            {
                livesRemaining = value;
            }
            healthText.text = livesRemaining + "/" + maxLivesRemaining + "liv tilbage";
        } 
    }

    public BoardController board;



    /// <summary>
    /// gets references and sets up various variables
    /// </summary>
    void Start()
    {
        cooldownText = cooldownTextObject.GetComponent<TextMeshPro>();
        cooldownText.text = "";
        currentDestination = transform.position;
        maxLivesRemaining = livesRemaining;
        healthText = healthTextObject.GetComponent<TextMeshProUGUI>();
        healthText.text = livesRemaining + "/" + maxLivesRemaining + " liv tilbage";
    }
    
    /// <summary>
    /// Handles movement and the time the player cant move when hitting an incorrect Letter
    /// </summary>
    void Update()
    {
        //Checks if the player is outside the board and ends the game if it is true
        if((transform.position.x > 20.4f || transform.position.x < 9.6f || transform.position.z > 20.4f || transform.position.z < 9.6f) && !thrown)
        {
            thrown = true;
            canMove = false;
            board.Lost();
        }
        //Checks if the player can control their movement and moves them a tile in the desired direction based on keyboard input
        if(IncorrectSymbolStepMoveDelayRemaining == 0 && currentDestination == transform.position && !thrown && canMove)
        {
            if(Input.GetKeyDown(KeyCode.W) && transform.position.x < 19.5f)
            {
                currentDestination = transform.position + new Vector3(1, 0, 0);
            }
            else if(Input.GetKeyDown(KeyCode.S) && transform.position.x > 10.5f)
            {
                currentDestination = transform.position + new Vector3(-1, 0, 0);
            }
            else if(Input.GetKeyDown(KeyCode.A) && transform.position.z < 19.5f)
            {
                currentDestination = transform.position + new Vector3(0, 0, 1);
            }
            else if(Input.GetKeyDown(KeyCode.D) && transform.position.z > 10.5f)
            {
                currentDestination = transform.position + new Vector3(0, 0, -1);
            }
            
        }
        // calls the move method if the player is moving and sets hasMoved to true.
        else if (currentDestination != transform.position && canMove)
        {
            Move();
            if(!hasMoved)
            {
                hasMoved = true;
            }
        }
        //Code to count down time remaining on the cooldown and to update the display
        else if(IncorrectSymbolStepMoveDelayRemaining > 0)
        {
            IncorrectSymbolStepMoveDelayRemaining -= Time.deltaTime;
            if(IncorrectSymbolStepMoveDelayRemaining <= 0)
            {
                IncorrectSymbolStepMoveDelayRemaining = 0;
                cooldownText.text = "";
                hasMoveDelay = false;
            }
            else 
            {
                cooldownText.text = Math.Round(IncorrectSymbolStepMoveDelayRemaining, 2) + " sek. tilbage";
            }
        }
    }

    /// <summary>
    /// code to start the delay in the players movement.
    /// </summary>
    public void IncorrectGuess()
    {
        IncorrectSymbolStepMoveDelayRemaining = maxIncorrectSymbolStepMoveDelayRemaining;
        hasMoveDelay = true;
        cooldownText.text = Math.Round(IncorrectSymbolStepMoveDelayRemaining, 2) + " sek. tilbage";
    }

    /// <summary>
    /// Moves the player towards their destination.
    /// </summary>
    void Move()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentDestination, step);
        if(transform.position == currentDestination && thrown)
        {
            thrown = false;
        }
    }

    public void StopMovement()
    {
        canMove = false;
    }

    public void StartMovement()
    {
        canMove = true;
    }
}

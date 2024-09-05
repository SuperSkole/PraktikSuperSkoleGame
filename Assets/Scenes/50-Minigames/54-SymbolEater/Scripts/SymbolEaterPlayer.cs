using System;
using System.Runtime.CompilerServices;
using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


namespace Scenes._50_Minigames._54_SymbolEater.Scripts
{
    /// <summary>
    /// Player class for the Symbol Eater mini game
    /// </summary>
    public class SymbolEaterPlayer : MonoBehaviour
    {

        private int livesRemaining = 3;

        private int maxLivesRemaining;

        [SerializeField] private float IncorrectSymbolStepMoveDelayRemaining = 0;

        [SerializeField] private GameObject cooldownTextObject;

        [SerializeField] private GameObject healthTextObject;

        [SerializeField] private GameObject placePlayerMonster;

        private GameObject playerMonster;

        private TextMeshProUGUI healthText;

        private TextMeshPro cooldownText;

        private Vector3 currentDestination;

        public bool thrown = false;

        public bool hasMoved = false;

        public float speed = 0.5f;

        public bool hasMoveDelay = false;

        public bool canMove = true;

        public float maxIncorrectSymbolStepMoveDelayRemaining = 6;

        public Vector3 CurrentDestination { get => currentDestination; set => currentDestination = value; }

        private float timerSinceMoved = 0;

        private float moveDelayTimer = 0.25f;

        private string currentState = "Walk";
        private SkeletonAnimation skeletonAnimation;
        private AnimationReferenceAsset walk;
        private AnimationReferenceAsset idle;
        private Vector3 playerOldScale;

        private readonly float blendDuration = 0.2f;
        private bool facingRight = false;
        private CapsuleCollider colider;

        /// <summary>
        /// Property of livesRemaining. if used to setting the value it cant be below 0 and it also updates the lives remaining text
        /// </summary>
        public int LivesRemaining
        {
            get => livesRemaining;
            set
            {
                if (value >= 0)
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
            colider = gameObject.GetComponent<CapsuleCollider>();

            /// <summary>
            /// Spawn Player Charector on Player prefab?
            /// </summary>
            //Instanciate playerCharactor in SymbolEater
            if (PlayerManager.Instance != null)
            {   //ask Sofie if you dont know what is happeing here!
                PlayerManager.Instance.PositionPlayerAt(placePlayerMonster);
                playerMonster = PlayerManager.Instance.SpawnedPlayer;
                playerOldScale = playerMonster.transform.localScale;
                playerMonster.transform.localScale = new(0.12f,0.12f,0.12f);
                playerMonster.transform.localScale = new(0.12f,0.12f,0.12f);
                playerMonster.transform.localPosition += Vector3.up * 0.8f;
                skeletonAnimation = playerMonster.GetComponentInChildren<SkeletonAnimation>();
                SpinePlayerMovement skeletorn = playerMonster.GetComponent<SpinePlayerMovement>();
                walk = skeletorn.walk;
                idle = skeletorn.idle;
                SetCharacterState("Idle");
            }
            else
            {
                Debug.Log("WordFactory GM.Start(): Player Manager Is null");
            }
        }


        /// <summary>
        /// Sets the player's animation state to either idle or walk, with blending between states.
        /// </summary>
        /// <param name="state">The desired animation state ("Idle" or "Walk").</param>
        public void SetCharacterState(string state)//this was stolen from SpinePlayerMovement to animate the player
        {
            if (state.Equals("Idle") && currentState != "Idle")
            {
                //Blending animations walk - idle
                skeletonAnimation.state.SetAnimation(0, idle, true).MixDuration = blendDuration;
                currentState = "Idle";
            }
            else if (state.Equals("Walk") && currentState != "Walk")
            {
                //Blending animations idle - walk
                skeletonAnimation.state.SetAnimation(0, walk, true).MixDuration = blendDuration;
                currentState = "Walk";
            }
        }

        /// <summary>
        /// Handles movement and the time the player cant move when hitting an incorrect Letter
        /// </summary>
        void Update()
        {

            timerSinceMoved += Time.deltaTime;

            //Checks if the player is outside the board and ends the game if it is true
            if ((transform.position.x > 20.4f || transform.position.x < 9.6f || transform.position.z > 20.4f || transform.position.z < 9.6f) && !thrown)
            {
                thrown = true;
                canMove = false;
                SetCharacterState("Idle");
                board.Lost();
            }
            //Check to ensure currentDestination is in sync with the center of the tiles.
            if ((currentDestination.x - 10.5f) % 1 != 0 || (currentDestination.z - 10.5f) % 1 != 0)
            {
                float fixedX = MathF.Round(currentDestination.x - 10.5f) + 10.5f;
                float fixedZ = MathF.Round(currentDestination.z - 10.5f) + 10.5f;
                currentDestination = new Vector3(fixedX, currentDestination.y, fixedZ);
            }


            

            //Checks if the player can control their movement and moves them a tile in the desired direction based on keyboard input
            if (IncorrectSymbolStepMoveDelayRemaining == 0 && currentDestination == transform.position && !thrown && canMove)
            {


                if (Input.GetKey(KeyCode.W) && transform.position.x < 19.5f && timerSinceMoved >= moveDelayTimer)
                {
                    currentDestination = transform.position + new Vector3(1, 0, 0);
                    timerSinceMoved = 0;
                }
                else if (Input.GetKey(KeyCode.S) && transform.position.x > 10.5f && timerSinceMoved >= moveDelayTimer)
                {
                    currentDestination = transform.position + new Vector3(-1, 0, 0);
                    timerSinceMoved = 0;
                }
                else if (Input.GetKey(KeyCode.A) && transform.position.z < 19.5f && timerSinceMoved >= moveDelayTimer)
                {
                    
                    currentDestination = transform.position + new Vector3(0, 0, 1);
                    if (facingRight)
                    {
                        facingRight = false;
                        Flip();
                    }
                    timerSinceMoved = 0;
                }
                else if (Input.GetKey(KeyCode.D) && transform.position.z > 10.5f && timerSinceMoved >= moveDelayTimer)
                {
                    currentDestination = transform.position + new Vector3(0, 0, -1);
                    if (!facingRight)
                    {
                        facingRight = !false;
                        Flip();
                    }
                    timerSinceMoved = 0;
                }

            }
            // calls the move method if the player is moving and sets hasMoved to true.
            else if (currentDestination != transform.position && canMove)
            {
                Move();
                if (!hasMoved)
                {
                    hasMoved = true;
                }
            }
            //Code to count down time remaining on the cooldown and to update the display
            else if (IncorrectSymbolStepMoveDelayRemaining > 0)
            {
                IncorrectSymbolStepMoveDelayRemaining -= Time.deltaTime;
                if (IncorrectSymbolStepMoveDelayRemaining <= 0)
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

            if (currentDestination == transform.position)
            {

                SetCharacterState("Idle");
            }
            else
            {

                SetCharacterState("Walk");
            }
        }

        /// <summary>
        /// used to flip the dirrection the player is facing
        /// </summary>
        void Flip()
        {
            Vector3 currentScale = playerMonster.transform.localScale;
            currentScale.x *= -1;
            playerMonster.transform.localScale = currentScale;
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


            if (transform.position == currentDestination && thrown)
            {
                thrown = false;
                colider.enabled = true;
            }
        }

        public void StopMovement()
        {
            canMove = false;
            SetCharacterState("Idle");
        }

        public void StartMovement()
        {
            canMove = true;
            SetCharacterState("Walk");
        }

        /// <summary>
        /// used to fix player after the game!!
        /// </summary>
        public void GameOver()
        {
            playerMonster.transform.parent = null;
            playerMonster.transform.localScale = playerOldScale;
            playerMonster.transform.rotation = Quaternion.Euler(0, 0, 0);
            DontDestroyOnLoad(playerMonster);
        }
    }

}
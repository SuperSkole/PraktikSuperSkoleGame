using System.Collections;
using UnityEngine;



namespace Scenes._50_Minigames._54_SymbolEater.Scripts
{


    /// <summary>
    /// Class for monster chasing the player in the Symbol Eater mini game
    /// </summary>
    public class Monster : MonoBehaviour
    {

        private bool canWalkTowardsPlayer = true;

        public GameObject playerObject;

        [SerializeField] private int throwRange = 5;

        [SerializeField] private AudioClip walkSound;

        [SerializeField] Vector3 currentDestination;
        public float speed = 0.5f;

        private bool throwingPlayer = false;

        private bool releasingPlayer = false;

        private Vector3 playerDestination;

        private SymbolEaterPlayer player;

        [SerializeField] private GameObject targetMarker;

        [SerializeField] private GameObject rangeMarker;
        [SerializeField] private AudioClip grabSound;
        [SerializeField] private AudioClip throwSound;

        GameObject spawnedRangeMarker;

        private Vector3 throwReleasePoint;

        private Vector3 throwArcPoint;

        private Vector3 playerCatchPoint;

        private float throwArcProgress;

        private float defaultSpeed = 0.5f;

        private BoardController boardController;

        /// <summary>
        /// Gets the monster ready for movement and gets reference to the player script
        /// </summary>
        void Start()
        {
            currentDestination = transform.position;
            if(player == null)
            {
                player = playerObject.GetComponent<SymbolEaterPlayer>();
            }
        }
        public void SetBord(BoardController board)
        {
            boardController = board;
        }

        /// <summary>
        /// Handles throwing the player and movement
        /// </summary>
        void Update()
        {
            //Throws the player if the monster has caught them
            if (throwingPlayer)
            {
                ThrowPlayer();
            }
            //Finds the direction towards the player and then moves a tile towards the player on either the x or z axis.
            if (canWalkTowardsPlayer && currentDestination == transform.position)
            {
                Vector3 velocity = playerObject.transform.position - transform.position;
                //Determines if the monster should walk on the x or z axis and which direction. If the monster does not share either coordinate with the player it determines axis randomly
                if ((velocity.x < 1 && velocity.x > -1) || ((velocity.z >= 1 || velocity.z <= -1) && Random.Range(0, 2) == 0))
                {
                    velocity.x = 0;
                    if (velocity.z > 0)
                    {
                        velocity.z = 1;
                    }
                    else
                    {
                        velocity.z = -1;
                    }
                }
                else
                {
                    velocity.z = 0;
                    if (velocity.x > 0)
                    {
                        velocity.x = 1;
                    }
                    else
                    {
                        velocity.x = -1;
                    }
                }
                velocity.y = 0;
                //check to ensure the monster does not move out of right side of the board
                if (velocity.x + transform.position.x > 19.5f  || velocity.x +transform.position.x < 10.5f)
                {
                    velocity.x = 0;
                }
                currentDestination = velocity + transform.position;
                Move();
                AudioManager.Instance.PlaySound(walkSound, SoundType.SFX, transform.position);
            }
            else if (canWalkTowardsPlayer)
            {
                Move();
            }
        }


        /// <summary>
        /// Checks if the monster has begun overlapping with the player
        /// </summary>
        /// <param name="other">the collider of the colliding object</param>
        void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.tag == "Player" && !player.thrown)
            {
                ThrowPlayer();
                AudioManager.Instance.PlaySound(grabSound, SoundType.SFX, transform.position);
                //canWalk = false;
            }
        }

        int safty = 0;
        /// <summary>
        /// Throws the player towards a random point on the board
        /// </summary>
        void ThrowPlayer()
        {
            //the setup code of the throw
            if (!throwingPlayer && !releasingPlayer)
            {
                player.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                //sets up the rangemarker
                Vector3 scale = new Vector3(throwRange * 0.2f, 0, throwRange * 0.2f);
                //spawnedRangeMarker = Instantiate(rangeMarker, transform.position, Quaternion.identity);
                //spawnedRangeMarker.transform.localScale = scale;
                //removes a life from the player
                if (player.LivesRemaining > 0)
                {
                    player.LivesRemaining--;
                }
                throwArcProgress = 0;

                //Calculates the direction of the throw
                int xDirection = 1;
                int zDirection = 1;
                if (Random.Range(0, 2) == 0)
                {
                    xDirection = -1;
                }
                if (Random.Range(0, 2) == 0)
                {
                    zDirection = -1;
                }

                //Calculates the throw length
                Vector3 newDeltaPos = new Vector3(xDirection * Random.Range(0, throwRange), 0, zDirection * Random.Range(0, throwRange));

                //Checks if the throw has ended up with no movement on either axis and sets it to a random tile next to it.
                if (newDeltaPos.x == 0 && newDeltaPos.z == 0)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        newDeltaPos.x = 1;
                    }
                    else
                    {
                        newDeltaPos.z = 1;
                    }
                }

                //Corrects the throw if it is outside the throw range
                if (newDeltaPos.x > throwRange)
                {
                    newDeltaPos.x = throwRange;
                }
                else if (newDeltaPos.x < -throwRange)
                {
                    newDeltaPos.x = -throwRange;
                }
                if (newDeltaPos.z > throwRange)
                {
                    newDeltaPos.z = throwRange;
                }
                else if (newDeltaPos.z < -throwRange)
                {
                    newDeltaPos.z = -throwRange;
                }
                Vector3 newPos = newDeltaPos + player.CurrentDestination;

                //Position correction if the destination is outside the board
                if (player.LivesRemaining > 0)
                {
                    if (newPos.x > 19.5f)
                    {
                        newPos.x = 19.5f;
                    }
                    if (newPos.z > 19.5f)
                    {
                        newPos.z = 19.5f;
                    }
                    if (newPos.x < 10.5f)
                    {
                        newPos.x = 10.5f;
                    }
                    if (newPos.z < 10.5f)
                    {
                        newPos.z = 10.5f;
                    }
                }
                else
                {
                    throwRange++;
                }

                //chek if the chosen point is emty
                if(!boardController.IsPosFree(newPos))
                {
                    if(safty >= 100) return;
                    safty++;
                    //Destroy(spawnedRangeMarker);
                    ThrowPlayer();
                    return;
                }


                //Sets up various variables
                playerDestination = newPos;
                throwingPlayer = true;
                canWalkTowardsPlayer = false;
                player.thrown = true;

                //Calculates the end point of the players arc around the monster. Method based on this: https://stackoverflow.com/questions/300871/best-way-to-find-a-point-on-a-circle-closest-to-a-given-point
                playerCatchPoint = playerObject.transform.position;
                float vX = playerDestination.x - transform.position.x;
                float vZ = playerDestination.z - transform.position.z;
                float magV = Mathf.Sqrt(vX * vX + vZ * vZ);
                float aX = transform.position.x + vX / magV * Vector3.Distance(playerCatchPoint, transform.position);
                float aZ = transform.position.z + vZ / magV * Vector3.Distance(playerCatchPoint, transform.position);
                throwReleasePoint = new Vector3(aX, player.transform.position.y, aZ);

                //Determine which side of the monster the arc should be on
                Vector3 arcDirection;
                if (newPos.x > newPos.z)
                {
                    if (newPos.x < transform.position.x)
                    {
                        arcDirection = Vector3.forward;
                    }
                    else
                    {
                        arcDirection = Vector3.back;
                    }
                }
                else
                {
                    if (newPos.z < transform.position.z)
                    {
                        arcDirection = Vector3.right;
                    }
                    else
                    {
                        arcDirection = Vector3.left;
                    }
                }
                throwArcPoint = playerCatchPoint + (throwReleasePoint - playerCatchPoint) + arcDirection * 1.0f;
            }
            //Calculation of the players arc movement. Method based on this: https://gamedev.stackexchange.com/questions/157642/moving-a-2d-object-along-circular-arc-between-two-points
            else if (throwArcProgress < 1)
            {
                throwArcProgress += 1.0f * Time.deltaTime;
                Vector3 m1 = Vector3.Lerp(playerCatchPoint, throwArcPoint, throwArcProgress);
                Vector3 m2 = Vector3.Lerp(throwArcPoint, throwReleasePoint, throwArcProgress);
                player.transform.position = Vector3.Lerp(m1, m2, throwArcProgress);
            }
            //Cleanup once the player has reached the endpoint of the arc
            else
            {
                throwingPlayer = false;
                canWalkTowardsPlayer = true;
                player.CurrentDestination = playerDestination;
                Instantiate(targetMarker, playerDestination, Quaternion.identity);
                AudioManager.Instance.PlaySound(throwSound, SoundType.SFX, transform.position);
                //Destroy(spawnedRangeMarker);
            }
        }

        /// <summary>
        /// Moves the monster
        /// </summary>
        void Move()
        {
            if (currentDestination != transform.position)
            {
                float step = speed * Time.deltaTime;
                //Increases the monsters speed if the player has stepped on an incorrect letter
                if (player.hasMoveDelay)
                {
                    step *= 2;
                }

               
                transform.position = Vector3.MoveTowards(transform.position, currentDestination, step);
            }
        }

        public void ResetMoveSpeed()
        {
            speed = defaultSpeed;

        }


        public void StopMovement()
        {
            canWalkTowardsPlayer = false;
        }

        public void StartMovement()
        {
            canWalkTowardsPlayer = true;
        }
    }

}
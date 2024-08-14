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
    public GameObject playerObject;
    /// <summary>
    /// The time in seconds between the monsters attempts to move toward the player
    /// </summary>
    [SerializeField]private float walkDelay = 1.5f;

    [SerializeField]private int throwRange = 5;

    /// <summary>
    /// The point the monster is currently moving towards.
    /// </summary>
    [SerializeField]Vector3 currentDestination;
    public float speed = 0.5f;

    private bool throwingPlayer = false;

    private bool releasingPlayer = false;

    private Vector3 playerDestination;

    private Player player;

    /// <summary>
    /// The point at which the monster releases the player then throwing them
    /// </summary>
    private Vector3 releasePoint;

    /// <summary>
    /// Helper point for calculating the players arc around the monster
    /// </summary>
    private Vector3 arcPoint;

    /// <summary>
    /// The point the monster catches the player at
    /// </summary>
    private Vector3 playerStartPoint;

    /// <summary>
    /// The progress of the player over their arc around the monster
    /// </summary>
    private float count;


    // Start is called before the first frame update
    void Start()
    {
        currentDestination = transform.position;
        if(player == null){
            player = playerObject.GetComponent<Player>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Throws the player if the monster has caught them
        if(throwingPlayer){
            ThrowPlayer();
        }
        //Finds the direction towards the player and then moves a tile towards the player on either the x or z axis. Afterwards the monster waits before attempting again
        if (canWalk && currentDestination == transform.position){
            Vector3 velocity = playerObject.transform.position - transform.position;
            if((velocity.x < 1 && velocity.x > -1) || ((velocity.z >= 1 || velocity.z <= -1) && Random.Range(0, 2) == 0)){
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
            if(velocity.x + transform.position.x > 19.5f){
                velocity.x = 0;
            }
            currentDestination = velocity + transform.position;
            Move();
            //StartCoroutine(WalkDelay());
        }
        else if(canWalk){
            Move();
        }
    }


    /// <summary>
    /// Checks if the monster has begun overlapping with the player
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other){
        
        if(other.gameObject.tag == "Player" && !player.thrown){
            ThrowPlayer();
            //canWalk = false;
        }
    }


    /// <summary>
    /// Throws the player towards a random point on the board
    /// </summary>
    void ThrowPlayer(){
        if(!throwingPlayer && !releasingPlayer){
            player.LivesRemaining--;
            count = 0;
            int xDirection = 1;
            int zDirection = 1;
            if(Random.Range(0,2) == 0){
                xDirection = -1;
            }
            if(Random.Range(0,2) == 0){
                zDirection = -1;
            }
            Vector3 newDeltaPos = new Vector3(xDirection * Random.Range(0, throwRange), 0, zDirection * Random.Range(0, throwRange));
            if(newDeltaPos.x == 0 && newDeltaPos.z == 0){
                if(Random.Range(0, 2) == 0){
                    newDeltaPos.x = 1;
                }
                else{
                    newDeltaPos.z = 1;
                }
            }
            Vector3 newPos = newDeltaPos + player.CurrentDestination;
            
            //Position correction if the destination is outside the board
            if(player.LivesRemaining > 0){
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
            }
            else {
                throwRange++;
            }
            

            
            playerDestination = newPos;
            throwingPlayer = true;
            canWalk = false;
            player.thrown = true;

            //Calculates the end point of the players arc around the monster. Method based on this: https://stackoverflow.com/questions/300871/best-way-to-find-a-point-on-a-circle-closest-to-a-given-point
            playerStartPoint = playerObject.transform.position;
            float vX = playerDestination.x - transform.position.x;
            float vZ = playerDestination.z - transform.position.z;
            float magV = Mathf.Sqrt(vX * vX + vZ * vZ); 
            float aX = transform.position.x + vX / magV * Vector3.Distance(playerStartPoint, transform.position);
            float aZ = transform.position.z + vZ / magV * Vector3.Distance(playerStartPoint, transform.position);
            releasePoint = new Vector3(aX, player.transform.position.y,aZ);

            //Determine which side of the monster the arc should be on
            Vector3 arcDirection;
            if(newPos.x > newPos.z){
                if(newPos.x < transform.position.x){
                    arcDirection = Vector3.forward;
                }
                else{
                    arcDirection = Vector3.back;
                }
            }
            else {
                if(newPos.z < transform.position.z){
                    arcDirection = Vector3.right;
                }
                else{
                    arcDirection = Vector3.left;
                }
            }
            arcPoint = playerStartPoint + (releasePoint - playerStartPoint) + arcDirection * 1.0f;
        }
        //Calculation of the players arc movement. Method based on this: https://gamedev.stackexchange.com/questions/157642/moving-a-2d-object-along-circular-arc-between-two-points
        else if(count < 1){
            count += 1.0f * Time.deltaTime;
            Vector3 m1 = Vector3.Lerp(playerStartPoint, arcPoint, count);
            Vector3 m2 = Vector3.Lerp(arcPoint, releasePoint, count);
            player.transform.position = Vector3.Lerp(m1, m2, count);
        }
        //Cleanup once the player has reached the endpoint of the arc
        else {
            throwingPlayer = false;
            canWalk = true;
            player.CurrentDestination = playerDestination;
        }
    }

    /// <summary>
    /// Moves the monster
    /// </summary>
    void Move(){
        if(currentDestination != transform.position){
            float step = speed * Time.deltaTime;
            if(player.hasMoveDelay){
                step *= 2;
            }
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, step);
        }
    }
}


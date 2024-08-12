using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class PlayerWorldMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Adjust the speed as needed
    public static bool allowedToMove = true;
    public static bool Talking = false;

    private Rigidbody rb;
    private Animator animator;
    public GameObject inBubble;
    public static GameObject witchObjCloseTo { get; set; }
    [SerializeField] private GameObject testingWitch;
    private GameObject SkinShopGO;
    [SerializeField] private GameObject gmSaveToJson;
    [SerializeField] private ParticleSystem lvlUpEffect;

    /*
     * 0 For Wardrope & Girl Skins & SimpleMonster Skins
     * 1 For NPC1
     * 2 For NPC1
     */
    //[SerializeField] private List<UnityEvent> whichInteraction = new List<UnityEvent>();
    private UnityEvent[] whichInteraction = new UnityEvent[3];

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        gmSaveToJson = GameObject.FindGameObjectWithTag("GM");
    }


    void Update()
    {
        testingWitch = witchObjCloseTo;
        if (allowedToMove)
        {
            HandleMovement();
        }
        HandleInteraction();

    }
    void HandleInteraction()
    {
        if (witchObjCloseTo != null && Input.GetKeyDown(KeyCode.F) && Talking == false)
        {
            switch (witchObjCloseTo.gameObject.name)
            {
                case "Wardrope":
                    whichInteraction[0].Invoke();
                    break;
                case "NPCTesterRaceOwner":
                    whichInteraction[1].Invoke();
                    Debug.Log("PlayerWorldMovement/HandleInteraction/NPC 2 is talking");
                    break;
                case "NPCTesterSkinShop":
                    whichInteraction[2].Invoke();//Husk at ændre listnerer index nede i FindNPC 
                    Debug.Log("PlayerWorldMovement/HandleInteraction/NPC 1 is talking");
                    break;
                default:
                    // Check if the player object is interacting
                    if (witchObjCloseTo.gameObject.CompareTag("Player"))
                    {
                        whichInteraction[0].Invoke(); // Assuming player interaction is at index 0
                    }
                    break;
            }
        }
    }
    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //float tmpVal = horizontalInput + verticalInput;
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        //print(horizontalInput);
        // Move the player
        transform.Translate(movement * moveSpeed * Time.deltaTime);
        // Flip the player based on the horizontal input
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Moving right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // Moving left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayLevelUpEffect();
        }
    }

    public void GenerateInteractions()
    {
        FindWardrope();
        FindPlayer();
        FindNPC();
        //FindPlayerCar();
    }

    //private void FindPlayerCar()
    //{
    //    GameObject Car = GameObject.FindGameObjectWithTag("Car");

    //    if (Car!=null)
    //    {
    //        EnterExitVehicle enterCar = Car.GetComponent<EnterExitVehicle>();
    //        // Create a new UnityEvent
    //        UnityEvent newEvent = new UnityEvent();

    //        newEvent.AddListener(enterCar.ToggleCarAndPlayer);

    //        // Add the new event to the whichInteraction list
    //        whichInteraction.Add(newEvent);
    //        whichInteraction[0].AddListener(enterCar.ToggleCarAndPlayer);
    //    }

    //}

    /// <summary>
    /// Finds the wardrope gameobject so it can 
    /// add an event listner so you can open and close it
    /// </summary>
    private void FindWardrope()
    {
        // Find the Wardrope GameObject by name
        GameObject wardrope = GameObject.Find("Wardrope");

        // Check if Wardrope GameObject is found
        if (wardrope != null)
        {
            // Get the WardropeCus script attached to the Wardrope GameObject
            WardropeCus wardropeCus = wardrope.GetComponent<WardropeCus>();

            // Check if WardropeCus script is found
            if (wardropeCus != null)
            {
                // Create a new UnityEvent
                UnityEvent newEvent = new UnityEvent();

                // Add a listener to the new event that calls HandleCustomization method
                newEvent.AddListener(wardropeCus.HandleCustomzation);
                // Add the new event to the whichInteraction list
                whichInteraction[0] = newEvent;
            }
            else
            {
                Debug.LogWarning("WardropeCus script not found on Wardrope GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Wardrope GameObject not found in the scene.");
        }
    }
    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            GameObject SeeSkinsGO = GameObject.Find("SeePlayerSkins");
            GameObject SeeSkinsShopGO = GameObject.FindGameObjectWithTag("SeeSkinShopTag");
            GameObject characterCustomize = GameObject.Find("CharCuz");
            GameObject SkinShopParent = GameObject.Find("SkinShop");
            //SkinShopGO = SeeSkinsShopGO;
            characterCustomize.SetActive(false);
            SkinShopParent.SetActive(false);
            if (SeeSkinsShopGO == null)
            {
                Debug.Log("Cant find the Shop GO DO SOMETHIGN");
            }
            if (SeeSkinsGO == null)
            {
                Debug.Log("DO SOMETHIGN");
            }
            if (SeeSkinsGO != null && SeeSkinsShopGO != null)
            {
                ShowCusBodyParts SeeSkins = SeeSkinsGO.GetComponent<ShowCusBodyParts>();
                SkinShop skinShop = SeeSkinsShopGO.GetComponent<SkinShop>();
                if (SeeSkins != null && skinShop != null)
                {
                    // Create a new UnityEvent
                    UnityEvent newEvent = new UnityEvent();
                    // Add a listener to the new event that calls GiveNameToChar method
                    newEvent.AddListener(() => SeeSkins.GiveNameToChar(player.name));
                    newEvent.AddListener(() => skinShop.GiveNameToChar(player.name));
                    // Add the new event to the whichInteraction list
                    //whichInteraction.Add(newEvent);
                    whichInteraction[0].AddListener(() => SeeSkins.GiveNameToChar(player.name));
                    whichInteraction[0].AddListener(() => skinShop.GiveNameToChar(player.name));
                }
            }
            else { Debug.Log("Cant find SeePlayerSkins or SeeShopSkins"); }

        }
        else { Debug.Log("Cant find Tag Player"); }

    }

    /// <summary>
    /// Finds all the NPCs in the world,
    /// and adds an event listner for each NPC so talk to them.
    /// </summary>
    private void FindNPC()
    {
        GameObject[] NPCs = GameObject.FindGameObjectsWithTag("NPC");
        if (NPCs != null)
        {
            for (int i = 0; i < NPCs.Length; i++)
            {
                GameObject NPC = NPCs[i];
                // Create a new UnityEvent
                UnityEvent newEvent = new UnityEvent();

                switch (NPC.gameObject.name)
                {
                    //Lav et tidspunkt sådan at man ikke behøver at åben wardrope først før skinshop virker
                    case "NPCTesterRaceOwner":
                        NPCTrackOwner npcTrackScript = NPC.GetComponent<NPCTrackOwner>();
                        //SkinShop skinShopGO = SkinShopGO.GetComponent<SkinShop>();
                        // Add a listener to the new event that calls GiveNameToChar method
                        newEvent.AddListener(() => npcTrackScript.TalkingToPlayer());
                        //whichInteraction.Add(newEvent);
                        whichInteraction[1] = newEvent;


                        //whichInteraction[1].AddListener(() => skinShopGO.GiveNameToChar(this.gameObject.name));
                        break;

                    case "NPCTesterSkinShop":
                        //SkinShop skinShop = SkinShopGO.GetComponent<SkinShop>();

                        NPCSkinShop npcSkinScript = NPC.GetComponent<NPCSkinShop>();
                        // Add a listener to the new event that calls GiveNameToChar method
                        newEvent.AddListener(() => npcSkinScript.TalkingToPlayer());
                        whichInteraction[2] = newEvent;
                        break;
                }
            }
        }
        else
        {
            Debug.Log("No NPCs have been found, maybe missing tag?");
        }

    }

    public void PlayLevelUpEffect()
    {
        StartCoroutine(lvlEffectDelay());
    }
    IEnumerator lvlEffectDelay()
    {
        yield return new WaitForSeconds(0.3f);
        gmSaveToJson.GetComponent<GernalManagement>().EnableLvlTxt(true);
        lvlUpEffect.Play();
        yield return new WaitForSeconds(1.5f);
        gmSaveToJson.GetComponent<GernalManagement>().EnableLvlTxt(false);
    }
}

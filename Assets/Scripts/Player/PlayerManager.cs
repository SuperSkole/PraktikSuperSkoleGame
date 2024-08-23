using CORE;
using LoadSave;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    //[SerializeField] private CinemachineVirtualCamera virtualCamera;
    //[SerializeField] private Camera cameraBrain;
    //    [SerializeField] private LayerMask layerMask;

    // Fields required for setting up a new game
    private PlayerData playerData;
    private GameObject spawnedPlayer;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TextMeshProUGUI playerName;


    public GameObject SpawnedPlayer { get { return spawnedPlayer; } }
    public PlayerData PlayerData { get { return playerData; } }
    //public string ChosenMonsterColor;


    private static PlayerManager instance;
    public static PlayerManager Instance { get { return instance; }}

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // SceneManager.sceneLoaded += OnSceneLoaded;
        }
        Debug.Log("PlayerManager/Setting up Player instance");
        SetupPlayer();
    }
    private void Start()
    {
    }
    public void SetupPlayer()
    {
        // instantiate temp object in scene

        spawnedPlayer = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);

        PlayerData player = spawnedPlayer.GetComponent<PlayerData>();

        // Init player data
        player.Initialize(
            GameManager.Instance.CurrentUser,
            GameManager.Instance.PlayerData.MonsterName,
            GameManager.Instance.CurrentMonsterColor,
            0,
            0,
            0,
            spawnedPlayer.transform.position
        );

        // Log for debugging
        Debug.Log("Player setup complete with username: " + player.Username +
                  " Player Name: " + player.MonsterName +
                  " Monster Color: " + player.MonsterColor +
                  " XP: " + player.CurrentXPAmount.ToString() +
                  " Gold: " + player.CurrentGoldAmount.ToString());

        // TODO: delete at later date when PlayerManger works
        GameManager.Instance.PlayerData = player;

    }
    //public void InitializePlayerData(PlayerData data)
    //{
    //    playerData = data;

    //    // Init other player settings or components
    //}

    //public void InstantiatePlayer()
    //{
    //    var tmp = 0;
    //    try
    //    {
    //        tmp = GameManager.Instance.PlayerData.MonsterTypeID;
    //    }
    //    catch (System.Exception)
    //    {
    //        Debug.Log("SceneStartBehavior/Start/No Game Instance so no monster ID can be found, using ID: 0 ");
    //        tmp = 0;
    //    }
    //    switch (tmp)
    //    {
    //        case 0:
    //            spawnedPlayer = Instantiate(playerPrefab, playerSpawnPoint);
    //            playerData = spawnedPlayer.GetComponent<PlayerData>();
    //            virtualCamera.Follow = spawnedPlayer.transform;
    //            virtualCamera.LookAt = spawnedPlayer.transform;
    //            spawnedPlayer.GetComponent<SpinePlayerMovement>().sceneCamera = cameraBrain;
    //            spawnedPlayer.GetComponent<SpinePlayerMovement>().placementLayermask = layerMask;

    //            try
    //            {
    //                PopulatePlayerInfo();
    //            }
    //            catch (System.Exception) { Debug.Log("SceneStartBehavior/Start/Error when trying to populate player info "); }
    //            break;
    //    }
    //}

    //private void PopulatePlayerInfo()
    //{
    //    var gm = GameManager.Instance.PlayerData;
    //    playerData.Username = gm.Username;
    //    playerData.MonsterName = gm.MonsterName;
    //    playerData.MonsterTypeID = gm.MonsterTypeID;
    //    playerData.MonsterColor = gm.MonsterColor;
    //    playerData.CurrentGoldAmount = gm.CurrentGoldAmount;
    //    playerData.CurrentXPAmount = gm.CurrentXPAmount;
    //    playerData.CurrentLevel = gm.CurrentLevel;

    //    spawnedPlayer.GetComponent<PlayerColorManager>().ColorChange(playerData.MonsterColor);
    //}

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if (!scene.name.StartsWith("00") && !scene.name.StartsWith("01"))
    //    {
    //        spawnedPlayer = GameObject.Find("PlayerPrefab(Clone)");
    //        //spawnedPlayer.GetComponent<PlayerColorManager>().ColorChange(playerData.MonsterColor);
    //    }
    //}
}
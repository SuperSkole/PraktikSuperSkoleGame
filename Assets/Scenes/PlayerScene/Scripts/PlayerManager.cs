using Cinemachine;
using CORE;
using LoadSave;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.PlayerScene.Scripts
{
    public class PlayerManager : MonoBehaviour
    {
        // Fields required for setting up a new game
        [SerializeField] private GameObject playerPrefab;

        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TextMeshProUGUI playerName;

        private PlayerData playerData;
        private GameObject spawnedPlayer;
        private PlayerColorChanger playerColorChanger;
        
        // public GameObject SpawnedPlayer => spawnedPlayer;
        // public PlayerData PlayerData => playerData;
        
        //temp for testing
        public GameObject SpawnedPlayer 
        {
            get 
            {
                if (spawnedPlayer == null) 
                {
                    Debug.LogError("SpawnedPlayer accessed before being initialized.");
                }
                return spawnedPlayer;
            }
        }

        public PlayerData PlayerData 
        {
            get 
            {
                if (playerData == null) 
                {
                    Debug.LogError("PlayerData accessed before being initialized.");
                }
                return playerData;
            }
        }



        // Player Manager Singleton
        private static PlayerManager instance;
        public static PlayerManager Instance => instance;

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
                SceneManager.sceneLoaded += OnSceneLoaded;
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
            spawnedPlayer.name = "PlayerMonster";
            
            playerColorChanger = spawnedPlayer.GetComponentInChildren<PlayerColorChanger>();
            if (playerColorChanger == null) 
            {
                Debug.LogError("PlayerColorChanger component not found on spawned player.");
                return;
            }
            
            playerData = spawnedPlayer.GetComponent<PlayerData>();
            if (playerData == null) 
            {
                Debug.LogError("PlayerData component not found on spawned player.");
                return;
            }

            // Init player data
            playerData.Initialize(
                GameManager.Instance.CurrentUser,
                GameManager.Instance.PlayerData.MonsterName,
                GameManager.Instance.CurrentMonsterColor,
                0,
                0,
                0,
                spawnedPlayer.transform.position
            );
            
            // Call the ColorChange method to recolor the player
            playerColorChanger.ColorChange(GameManager.Instance.CurrentMonsterColor);
            
            // TODO CHANGE DISCUSTING MAGIC NUMBER FIX THE FUXKING MAIN WORLD
            playerData.SetLastInteractionPoint(new Vector3(-191, 40, -168));

            // Log for debugging
            Debug.Log("Player setup complete with username: " + playerData.Username +
                      " Player Name: " + playerData.MonsterName +
                      " Monster Color: " + playerData.MonsterColor +
                      " XP: " + playerData.CurrentXPAmount.ToString() +
                      " Gold: " + playerData.CurrentGoldAmount.ToString());

            // TODO: delete at later date when PlayerManger works
            GameManager.Instance.PlayerData = playerData;
            DontDestroyOnLoad(spawnedPlayer);
        }

        // TODO maybe refactor onSceneLoaded into new script 
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // if login or start screen we have no player yet, but we set camera
            SetCinemachineCameraTarget(scene);
            
            // Color change if scene is house or main
            UpdatePlayerColorOnSceneChange(scene);
            
            // if we are loading into main world, look for last interaction point and set as spawn point
            SetPlayerPositionOnSceneChange(scene);
        }

        private void SetCinemachineCameraTarget(Scene scene)
        {
            if (!scene.name.StartsWith("00") && !scene.name.StartsWith("01"))
            {
                var cinemachineCam = GameObject.FindGameObjectWithTag("Camera");
                var virtualCamera = cinemachineCam.GetComponent<CinemachineVirtualCamera>();

                virtualCamera.Follow = spawnedPlayer.transform;
                virtualCamera.LookAt = spawnedPlayer.transform;
            }
        }

        private void UpdatePlayerColorOnSceneChange(Scene scene)
        {
            if (scene.name.StartsWith("02") ||
                scene.name.StartsWith("03") ||
                scene.name.StartsWith("05"))
            {
                if (playerColorChanger != null)
                {
                    // Call the ColorChange method to recolor the player
                    playerColorChanger.ColorChange(GameManager.Instance.CurrentMonsterColor);
                }    
            }
        }
        
        private void SetPlayerPositionOnSceneChange(Scene scene)
        {
            Debug.Log($"Loaded Scene: {scene.name}, " +
                      $"Last Interaction Point: {PlayerData.lastInteractionPoint}");

            if (scene.name.StartsWith("03"))
            {
                // Ensure PlayerData and the lastInteractionPoint have been properly initialized
                if (playerData != null && playerData.lastInteractionPoint != Vector3.zero)
                {
                    // Set the player's position to the last interaction point stored in PlayerData
                    spawnedPlayer.transform.position = playerData.lastInteractionPoint;

                    Debug.Log("Player spawned at last interaction point: " + playerData.lastInteractionPoint.ToString());
                }
                else
                {
                    Debug.LogError("PlayerData is null or last interaction point is not set.");
                }
            }
        }

    }
}
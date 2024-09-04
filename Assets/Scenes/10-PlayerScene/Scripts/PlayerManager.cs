using System.Collections;
using Cinemachine;
using CORE;
using LoadSave;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._10_PlayerScene.Scripts
{
    /// <summary>
    /// Manages the player's data, interactions, and scene changes within the game.
    /// Ensures there is only one instance of this manager through the Singleton pattern.
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        // Fields required for setting up a new game
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private int moveSpeed = 5;
        [SerializeField] private Vector3 dropOffPoint; 

        private PlayerData playerData;
        private GameObject spawnedPlayer;
        private ColorChanging colorChanging;
        private ISkeletonComponent skeleton;

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

        /// <summary>
        /// Initializes the singleton instance and sets up the player.
        /// </summary>
        private void Awake()
        {
            //Debug.Log("PlayerManager.Awake: Setting up Player instance");
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

            if (GameManager.Instance.IsNewGame)
            {
                SetupNewPlayer();
            }
        }
        
        /// <summary>
        /// Positions the player at a specified spawn point in a scene.
        /// </summary>
        /// <param name="spawnPoint">The spawn point GameObject.</param>
        public void PositionPlayerAt(GameObject spawnPoint)
        {
            if (spawnPoint != null)
            {
                spawnedPlayer.transform.position = spawnPoint.transform.position;
                spawnedPlayer.transform.rotation = spawnPoint.transform.rotation;
            }
            else
            {
                Debug.LogError($"Spawn point {spawnPoint} not found for positioning player.");
            }
        }
        
        /// <summary>
        /// Sets up the player object in the game scene with necessary components.
        /// </summary>
        private void SetupNewPlayer()
        {
            spawnedPlayer = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            spawnedPlayer.name = "PlayerMonster";
            
            colorChanging = spawnedPlayer.GetComponentInChildren<ColorChanging>();
            if (colorChanging == null) 
            {
                Debug.LogError("PlayerManager.SetupPlayer(): " +
                               "ColorChanging component not found on spawned player.");
                return;
            }
            
            playerData = spawnedPlayer.GetComponent<PlayerData>();
            if (playerData == null) 
            {
                Debug.LogError("PlayerManager.SetupPlayer(): " +
                               "PlayerData component not found on spawned player.");
                return;
            }

            skeleton = spawnedPlayer.GetComponentInChildren<ISkeletonComponent>();
            if (skeleton == null)
            {
                Debug.LogError("PlayerManager.SetupPlayer(): " +
                               "ISkeleton component not found on spawned player.");
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
            colorChanging.SetSkeleton(skeleton);
            colorChanging.ColorChange(GameManager.Instance.CurrentMonsterColor);
            
            // TODO CHANGE DISCUSTING MAGIC NUMBER FIX THE FUXKING MAIN WORLD
            playerData.SetLastInteractionPoint(new Vector3(-184, 39, -144));

            // Log for debugging
            // Debug.Log(
            //     $"PlayerManager.SetupPlayer(): " +
            //     $"username: {playerData.Username} " +
            //     $"Player Name: {playerData.MonsterName} " +
            //     $"Monster Color: {playerData.MonsterColor} " +
            //     $"XP: {playerData.CurrentXPAmount} " +
            //     $"Gold: {playerData.CurrentGoldAmount}");

            // TODO: delete at later date when PlayerManger works
            GameManager.Instance.PlayerData = playerData;
            DontDestroyOnLoad(spawnedPlayer);
        }
        
        public void SetupPlayerFromSave(SaveDataDTO saveData)
        {
            // instantiate player object in scene
            spawnedPlayer = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            spawnedPlayer.name = "PlayerMonster";

            colorChanging = spawnedPlayer.GetComponentInChildren<ColorChanging>();
            if (colorChanging == null) 
            {
                Debug.LogError("PlayerManager.SetupPlayerFromSave(): " +
                               "ColorChanging component not found on spawned player.");
                return;
            }

            playerData = spawnedPlayer.GetComponent<PlayerData>();
            if (playerData == null) 
            {
                Debug.LogError("PlayerManager.SetupPlayerFromSave(): " +
                               "PlayerData component not found on spawned player.");
                return;
            }

            skeleton = spawnedPlayer.GetComponent<ISkeletonComponent>();
            if (skeleton == null)
            {
                Debug.LogError("PlayerManager.SetupPlayerFromSave(): " +
                               "ISkeletonComponent component not found on spawned player.");
                return;
            }


            // Init player data with saved data
            playerData.Initialize(
                saveData.Username,
                saveData.MonsterName,
                saveData.MonsterColor,
                saveData.GoldAmount,
                saveData.XPAmount,
                saveData.PlayerLevel,
                saveData.SavedPlayerStartPostion.GetVector3()
            );

            // Call the ColorChange method to recolor the player
            colorChanging.SetSkeleton(skeleton);
            colorChanging.ColorChange(playerData.MonsterColor);

            playerData.SetLastInteractionPoint(
                playerData.LastInteractionPoint == Vector3.zero
                    ? new Vector3(-184, 39, -144)
                    : playerData.LastInteractionPoint);

            // Log for debugging
            Debug.Log($"Player loaded from save: " +
                      $"username: {playerData.Username} " +
                      $"Player Name: {playerData.MonsterName} " +
                      $"Monster Color: {playerData.MonsterColor} " +
                      $"XP: {playerData.CurrentXPAmount} " +
                      $"Gold: {playerData.CurrentGoldAmount}");

            // Assign to GameManager for global access
            GameManager.Instance.PlayerData = playerData;

            DontDestroyOnLoad(spawnedPlayer);
        }

        // TODO maybe refactor onSceneLoaded into new script 
        /// <summary>
        /// Called every time a scene is loaded. Adjusts camera targets and updates player data.
        /// </summary>
        /// <param name="scene">The loaded scene.</param>
        /// <param name="mode">The loading mode of the scene.</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // if login or start screen we have no player yet, but we set camera
            SetCinemachineCameraTarget(scene);
            
            // Color change if scene is house or main
            UpdatePlayerColorOnSceneChange(scene);
            
            // if we are loading into main world, look for last interaction point and set as spawn point
            SetPlayerPositionOnSceneChange(scene);

            instance.spawnedPlayer.GetComponent<SpinePlayerMovement>().SceneStart();
            // TODO : Find a more permnat solution
            if (SceneManager.GetActiveScene().name.StartsWith("11") || SceneManager.GetActiveScene().name.StartsWith("20"))
            {
                instance.spawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = true;
                instance.spawnedPlayer.GetComponent<Rigidbody>().useGravity = true;
                instance.spawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;

            }
            else
            {
                instance.spawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = false;
                instance.spawnedPlayer.GetComponent<Rigidbody>().useGravity = false;
                instance.spawnedPlayer.GetComponent<CapsuleCollider>().enabled = false;
            }
        }

        /// <summary>
        /// Configures the Cinemachine camera to follow the player when necessary based on the scene.
        /// </summary>
        /// <param name="scene">The loaded scene.</param>
        private void SetCinemachineCameraTarget(Scene scene)
        {
            if (!scene.name.StartsWith("0"))
            {
                try
                {
                    var cinemachineCam
                        = GameObject.FindGameObjectWithTag("Camera");
                    var virtualCamera = cinemachineCam
                        .GetComponent<CinemachineVirtualCamera>();

                    virtualCamera.Follow = spawnedPlayer.transform;
                    virtualCamera.LookAt = spawnedPlayer.transform;

                    var cameraBrain = GameObject.Find("CameraBrain");
                    spawnedPlayer.GetComponent<SpinePlayerMovement>()
                        .sceneCamera = cameraBrain.GetComponent<Camera>();
                }
                catch
                {
                    
                }
            }
        }

        /// <summary>
        /// Updates the player's color when entering specific scenes.
        /// </summary>
        /// <param name="scene">The loaded scene.</param>
        private void UpdatePlayerColorOnSceneChange(Scene scene)
        {
            if (scene.name == SceneNames.House ||
                scene.name == SceneNames.Main ||
                scene.name.StartsWith("5") ||
                scene.name.StartsWith("6") ||
                scene.name.StartsWith("7"))
            {
                if (colorChanging != null)
                {
                    // Call the ColorChange method to recolor the player
                    colorChanging.SetSkeleton(skeleton);
                    colorChanging.ColorChange(playerData.MonsterColor);
                }    
            }
        }
        
        /// <summary>
        /// Sets the player's position based on the last interaction point when the scene changes.
        /// </summary>
        /// <param name="scene">The loaded scene.</param>
        private void SetPlayerPositionOnSceneChange(Scene scene)
        {
            // Debug.Log($"PlayerManager.SetPlayerPositionOnSceneChange():" +
            //           $"Loaded Scene: {scene.name}, " +
            //           $"Last Interaction Point: {PlayerData.LastInteractionPoint}");

            // Player House sat spawn to 0,2,0
            if (scene.name == SceneNames.House)
            {
                // Ensure PlayerData have been properly initialized
                if (playerData != null)
                {
                    // Set the player's position to player house magic number
                    spawnedPlayer.transform.position = new Vector3(0, 2, 0);
                    Debug.Log("Player spawned in house at 0,2,0");
                }
                else
                {
                    Debug.LogError("PlayerData is null");
                }
            }
            
            // if going to main world spawn at last known interaction point
            if (scene.name == SceneNames.Main)
            {
                // Ensure PlayerData and the lastInteractionPoint have been properly initialized
                if (playerData != null && playerData.LastInteractionPoint != Vector3.zero)
                {
                    // Set the player's position to the last interaction point stored in PlayerData
                    spawnedPlayer.transform.position = playerData.LastInteractionPoint;

                    //Debug.Log("Player spawned at last interaction point: " + playerData.LastInteractionPoint.ToString());
                }
                else
                {
                    Debug.LogError("PlayerData is null or last interaction point is not set.");
                }
            }
        }
    }
}
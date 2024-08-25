using Cinemachine;
using CORE;
using LoadSave;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.PlayerScene.Scripts
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

        /// <summary>
        /// Initializes the singleton instance and sets up the player.
        /// </summary>
        private void Awake()
        {
            Debug.Log("PlayerManager.Awake: Setting up Player instance");
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
            
            SetupPlayer();
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
        private void SetupPlayer()
        {
            // instantiate temp object in scene

            spawnedPlayer = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            spawnedPlayer.name = "PlayerMonster";
            
            playerColorChanger = spawnedPlayer.GetComponentInChildren<PlayerColorChanger>();
            if (playerColorChanger == null) 
            {
                Debug.LogError("PlayerManager.SetupPlayer(): " +
                               "PlayerColorChanger component not found on spawned player.");
                return;
            }
            
            playerData = spawnedPlayer.GetComponent<PlayerData>();
            if (playerData == null) 
            {
                Debug.LogError("PlayerManager.SetupPlayer(): " +
                               "PlayerData component not found on spawned player.");
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
            Debug.Log(
                $"PlayerManager.SetupPlayer(): " +
                $"username: {playerData.Username} " +
                $"Player Name: {playerData.MonsterName} " +
                $"Monster Color: {playerData.MonsterColor} " +
                $"XP: {playerData.CurrentXPAmount} " +
                $"Gold: {playerData.CurrentGoldAmount}");

            // TODO: delete at later date when PlayerManger works
            GameManager.Instance.PlayerData = playerData;
            DontDestroyOnLoad(spawnedPlayer);
        }
        
        public void SetupPlayerFromSave(SaveDataDTO saveData)
        {
            // instantiate player object in scene
            spawnedPlayer = Instantiate(playerPrefab, saveData.SavedPlayerStartPostion.GetVector3(), Quaternion.identity);
            spawnedPlayer.name = "PlayerMonster";

            playerColorChanger = spawnedPlayer.GetComponentInChildren<PlayerColorChanger>();
            if (playerColorChanger == null) 
            {
                Debug.LogError("PlayerManager.SetupPlayerFromSave(): " +
                               "PlayerColorChanger component not found on spawned player.");
                return;
            }

            playerData = spawnedPlayer.GetComponent<PlayerData>();
            if (playerData == null) 
            {
                Debug.LogError("PlayerManager.SetupPlayerFromSave(): " +
                               "PlayerData component not found on spawned player.");
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

            // Apply the saved color
            playerColorChanger.ColorChange(saveData.MonsterColor);

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
        }

        /// <summary>
        /// Configures the Cinemachine camera to follow the player when necessary based on the scene.
        /// </summary>
        /// <param name="scene">The loaded scene.</param>
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

        /// <summary>
        /// Updates the player's color when entering specific scenes.
        /// </summary>
        /// <param name="scene">The loaded scene.</param>
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
        
        /// <summary>
        /// Sets the player's position based on the last interaction point when the scene changes.
        /// </summary>
        /// <param name="scene">The loaded scene.</param>
        private void SetPlayerPositionOnSceneChange(Scene scene)
        {
            Debug.Log($"PlayerManager.SetPlayerPositionOnSceneChange():" +
                      $"Loaded Scene: {scene.name}, " +
                      $"Last Interaction Point: {PlayerData.LastInteractionPoint}");

            // Player House sat spawn to 0,2,0
            if (scene.name.StartsWith("02"))
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
            if (scene.name.StartsWith("03"))
            {
                // Ensure PlayerData and the lastInteractionPoint have been properly initialized
                if (playerData != null && playerData.LastInteractionPoint != Vector3.zero)
                {
                    // Set the player's position to the last interaction point stored in PlayerData
                    spawnedPlayer.transform.position = playerData.LastInteractionPoint;

                    Debug.Log("Player spawned at last interaction point: " + playerData.LastInteractionPoint.ToString());
                }
                else
                {
                    Debug.LogError("PlayerData is null or last interaction point is not set.");
                }
            }
        }
    }
}
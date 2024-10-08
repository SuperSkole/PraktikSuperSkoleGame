using Cinemachine;
using CORE;
using CORE.Scripts;
using LoadSave;
using Scenes._24_HighScoreScene.Scripts;
using Scenes._88_LeaderBoard.Scripts;
using Spine.Unity;
using System.Collections.Generic;
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
        [SerializeField] private HighScore highScore;

        // Fields required for setting up a new game
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Vector3 dropOffPoint;
        public GameObject coinPrefab;
        private PlayerData playerData;
        private GameObject spawnedPlayer;
        private ColorChanging colorChanging;
        private ClothChanging clothChanging;
        private ISkeletonComponent skeleton;

        private Vector3 tmpPlayerSpawnPoint = new Vector3(0f, 3f, 28f);

        private ILeaderboardSubmissionService leaderboardSubmissionService;

        // public GameObject SpawnedPlayer => spawnedPlayer;
        // public PlayerData PlayerData => playerData;

        //temp for testing
        public GameObject SpawnedPlayer
        {
            get
            {
                if (spawnedPlayer == null)
                {
                    Debug.Log("SpawnedPlayer accessed before being initialized.");
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
                    Debug.Log("PlayerData accessed before being initialized.");
                }

                return playerData;
            }
        }

        public HighScore HighScore
        {
            get
            {
                if (highScore == null)
                {
                    Debug.Log("highScore accessed before being initialized.");
                }

                return highScore;
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

            GameManager.Instance.PlayerManager = this;
            leaderboardSubmissionService = new LeaderboardSubmissionService();
        }

        private void OnDestroy()
        {
            PlayerEvents.OnAddWord -= OnAddWordHandler;
            PlayerEvents.OnAddLetter -= OnAddLetterHandler;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            instance = null;
        }

        private void OnEnable()
        {
            PlayerEvents.OnAddWord += OnAddWordHandler;
            PlayerEvents.OnAddLetter += OnAddLetterHandler;
        }

        private void OnDisable()
        {
            PlayerEvents.OnAddWord -= OnAddWordHandler;
            PlayerEvents.OnAddLetter -= OnAddLetterHandler;
        }

        private void OnAddWordHandler(string word)
        {
            SubmitWordCountToLeaderboard();
        }

        private void OnAddLetterHandler(char letter)
        {
            SubmitLetterCountToLeaderboard();
        }


        public async void SubmitWordCountToLeaderboard()
        {
            await leaderboardSubmissionService.EnsureSignedIn();
            int totalWords = PlayerData.LifetimeTotalWords;
            await leaderboardSubmissionService.SubmitMostWords(totalWords, PlayerData.MonsterName);
        }

        public async void SubmitLetterCountToLeaderboard()
        {
            await leaderboardSubmissionService.EnsureSignedIn();
            int totalLetters = PlayerData.LifetimeTotalLetters;
            await leaderboardSubmissionService.SubmitMostLetters(totalLetters, PlayerData.MonsterName);
        }

        /// <summary>
        /// Positions the player at a specified spawn point in a scene.
        /// </summary>
        /// <param name="spawnPoint">The spawn point GameObject.</param>
        public void PositionPlayerAt(GameObject spawnPoint)
        {
            if (spawnPoint != null)
            {
                spawnedPlayer.GetComponent<Rigidbody>().position = spawnPoint.transform.position;
                spawnedPlayer.GetComponent<Rigidbody>().rotation = spawnPoint.transform.rotation;
                spawnedPlayer.transform.position = spawnPoint.transform.position;
                spawnedPlayer.transform.rotation = spawnPoint.transform.rotation;
                spawnedPlayer.transform.position = spawnPoint.transform.position;

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

            clothChanging = spawnedPlayer.GetComponentInChildren<ClothChanging>();
            if (clothChanging == null)
            {
                Debug.LogError("PlayerManager.SetupPlayer(): " +
                               "ClothChanging component not found on spawned player.");
            }

            // Init player data
            playerData.Initialize(
                GameManager.Instance.CurrentUser,
                GameManager.Instance.PlayerData.MonsterName,
                GameManager.Instance.CurrentMonsterColor,
                0,
                0,
                1,
                spawnedPlayer.transform.position,
                new List<string>(),
                new List<char>(),
                0,
                0,
                "",
                "",
                new List<int>(),
                new List<CarInfo>()//Start with the Van so there is a purpose for switching cars
                {
                    new CarInfo("Van",
                        "Gray",
                        true,
                        new List<MaterialInfo>
                        {
                            new MaterialInfo(true,
                                "Gray")
                        })
                },
                new List<int>()
                {
                    0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 7, 7, 7, 7, 8, 8, 8, 8, 9, 11, 10, 12
                }
            );

            if (GameManager.Instance.IsPlayerBootstrapped)
            {
                playerData.CollectedWords.AddRange(WordsManager.GetRandomWordsByLengthAndCount(2, 3));
                playerData.CollectedLetters.AddRange(LetterManager.GetRandomLetters(3));
            }

            // Call the ColorChange method to recolor the player
            colorChanging.SetSkeleton(skeleton);
            colorChanging.ColorChange(GameManager.Instance.CurrentMonsterColor);

            clothChanging.ChangeClothes(GameManager.Instance.CurrentClothMid, skeleton);
            clothChanging.ChangeClothes(GameManager.Instance.CurrentClothTop, skeleton);

            // TODO CHANGE DISCUSTING MAGIC NUMBER FIX THE FUXKING MAIN WORLD
            playerData.SetLastInteractionPoint(tmpPlayerSpawnPoint);

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

            GameManager.Instance.IsNewGame = false;
        }

        public void SetupPlayerFromSave(PlayerData saveData)
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

            skeleton = spawnedPlayer.GetComponentInChildren<ISkeletonComponent>();
            if (skeleton == null)
            {
                Debug.LogError("PlayerManager.SetupPlayerFromSave(): " +
                               "ISkeletonComponent component not found on spawned player.");
                //return;
            }

            clothChanging = spawnedPlayer.GetComponentInChildren<ClothChanging>();
            if (clothChanging == null)
            {
                Debug.LogError("PlayerManager.SetupPlayer(): " +
                               "ClothChanging component not found on spawned player.");
            }

            // Init player data with saved data
            playerData.Initialize(
                saveData.Username,
                saveData.MonsterName,
                saveData.MonsterColor,
                saveData.CurrentGoldAmount,
                saveData.CurrentXPAmount,
                saveData.CurrentLevel,
                saveData.CurrentPosition,
                saveData.CollectedWords,
                saveData.CollectedLetters,
                saveData.LifetimeTotalWords,
                saveData.LifetimeTotalLetters,
                saveData.ClothMid,
                saveData.ClothTop,
                saveData.BoughtClothes,
                saveData.ListOfCars,
                saveData.ListOfFurniture
            );

            // Call the ColorChange method to recolor the player
            colorChanging.SetSkeleton(skeleton);
            colorChanging.ColorChange(playerData.MonsterColor);

            // Call the ColorChange method to recolor the player
            clothChanging.ChangeClothes(playerData.ClothMid, skeleton);
            clothChanging.ChangeClothes(playerData.ClothTop, skeleton);

            playerData.SetLastInteractionPoint(
                playerData.LastInteractionPoint == Vector3.zero
                    ? tmpPlayerSpawnPoint
                    : playerData.LastInteractionPoint);

            // // Log for debugging
            // Debug.Log($"Player loaded from save: " +
            //           $"username: {playerData.Username} " +
            //           $"Player Name: {playerData.MonsterName} " +
            //           $"Monster Color: {playerData.MonsterColor} " +
            //           $"XP: {playerData.CurrentXPAmount} " +
            //           $"Gold: {playerData.CurrentGoldAmount}");

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
            if (instance.spawnedPlayer == null) return;

            instance.spawnedPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;

            // if login or start screen we have no player yet, but we set camera
            SetCinemachineCameraTarget(scene);

            // Color change if scene is house or main
            UpdatePlayerColorOnSceneChange(scene);

            UpdatePlayerClothOnSceneChange(scene);

            // if we are loading into main world, look for last interaction point and set as spawn point
            SetPlayerPositionOnSceneChange(scene);

            // TODO : Find a more permnat solution
            if (SceneManager.GetActiveScene().name.StartsWith("11") ||
                SceneManager.GetActiveScene().name.StartsWith("20") ||
                SceneManager.GetActiveScene().name.StartsWith("70"))
            {
                instance.spawnedPlayer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                instance.spawnedPlayer.transform.GetChild(1).transform.localPosition = new Vector3(-2.24f, 3f, -2f);

                instance.spawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = true;
                instance.spawnedPlayer.GetComponent<Rigidbody>().useGravity = true;
                instance.spawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;
                instance.spawnedPlayer.GetComponent<PlayerFloating>().enabled = true;
                instance.spawnedPlayer.GetComponent<PlayerAnimatior>().StartUp();
            }
            else
            {
                instance.spawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = false;
                instance.spawnedPlayer.GetComponent<Rigidbody>().useGravity = false;
                instance.spawnedPlayer.GetComponent<CapsuleCollider>().enabled = false;
            }
            if (SceneManager.GetActiveScene().name.StartsWith("11"))
            {
                instance.spawnedPlayer.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                instance.spawnedPlayer.transform.GetChild(1).transform.localPosition = new Vector3(-2.24f, 0.9f, -2f);
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
        public void UpdatePlayerColorOnSceneChange(Scene scene)
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

        public void UpdatePlayerClothOnSceneChange(Scene scene)
        {
            if (scene.name == SceneNames.House ||
                scene.name == SceneNames.Main ||
                scene.name.StartsWith("5") ||
                scene.name.StartsWith("6") ||
                scene.name.StartsWith("7"))
            {
                if (clothChanging != null)
                {
                    // Call the ColorChange method to recolor the player
                    clothChanging.ChangeClothes(playerData.ClothMid, skeleton);
                    clothChanging.ChangeClothes(playerData.ClothTop, skeleton);
                }
            }
        }

        /// <summary>
        /// Sets the player's position based on the last interaction point when the scene changes.
        /// </summary>
        /// <param name="scene">The loaded scene.</param>
        private void SetPlayerPositionOnSceneChange(Scene scene)
        {
            // Player House sat spawn to 0,2,0
            if (scene.name == SceneNames.House)
            {
                // Ensure PlayerData have been properly initialized
                if (playerData != null)
                {
                    // Set the player's position to player house magic number
                    spawnedPlayer.GetComponent<Rigidbody>().position = new Vector3(0, 2, 0);
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
                    spawnedPlayer.GetComponent<Rigidbody>().position = playerData.LastInteractionPoint;
                    spawnedPlayer.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, 0);
                    spawnedPlayer.transform.position = playerData.LastInteractionPoint;
                }
                else
                {
                    Debug.LogError("PlayerData is null or last interaction point is not set.");
                }
            }
        }
    }
}
using LoadSave;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes._10_PlayerScene.Scripts
{
    /// <summary>
    /// Manages player-related events and data interactions within the game,
    /// utilizing design patterns to enhance modularity and reduce component dependencies.
    /// 
    /// <para><b>Event Aggregator:</b> Centralizes player events
    /// to ensure consistent handling and dispatch across various game components.</para>
    /// 
    /// <para><b>Observer:</b> Listens and reacts to player events
    /// to keep player data synchronized with game activities.</para>
    /// 
    /// <para><b>Mediator:</b> Facilitates communication between game components
    /// concerning player data and events,
    /// acting as a central coordinator to simplify interactions and dependencies.</para>
    /// </summary>
    public class PlayerEventManager : MonoBehaviour

    {
        [SerializeField] private PlayerData playerData;
        public bool IsInCar { get; set; }

        // Event to trigger visual effects or other responses to leveling up
        public event Action OnLevelUp;
        public UnityEvent PlayerInteraction { get; set; } = new UnityEvent();

        void Start()
        {
            if (playerData == null)
            {
                Debug.LogError("PlayerData reference not set on PlayerEventManager.");
            }
        }


        private void OnEnable()
        {
            PlayerEvents.OnWordValidated += AddWordToPlayerData;
            PlayerEvents.OnWordRemovedValidated += RemoveWordFromPlayerData;
            PlayerEvents.OnPlayerDataWordsExtracted += HandlePlayerDataWordsExtracted;

        }

        private void OnDisable()
        {
            PlayerEvents.OnWordValidated -= AddWordToPlayerData;
            PlayerEvents.OnWordRemovedValidated -= RemoveWordFromPlayerData;
            PlayerEvents.OnPlayerDataWordsExtracted -= HandlePlayerDataWordsExtracted;
        }

        private void OnDestroy()
        {
            PlayerEvents.OnWordValidated -= AddWordToPlayerData;
            PlayerEvents.OnWordRemovedValidated -= RemoveWordFromPlayerData;
            PlayerEvents.OnPlayerDataWordsExtracted -= HandlePlayerDataWordsExtracted;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                try
                {
                    PlayerInteraction.Invoke();
                    if (!IsInCar)
                    {
                        PlayerInteraction = new UnityEvent();
                    }

                }
                catch { print("PlayerEventManager/Update/No playeraction"); }
            }
        }

        // /// <summary>
        // /// Initializes the PlayerEventManager with references to player data.
        // /// </summary>
        // /// <param name="data">Reference to the player data object.</param>
        // public void InitializePlayerEventManager()
        // {
        //     print("InitializePlayerEventManager");
        //     playerData = PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>();
        // }

        /// <summary>
        /// Adds a word to the player's collected words list,
        /// if it is not null or empty.
        /// </summary>
        /// <param name="word">The word to add to the collected words list.</param>
        /// <exception cref="ArgumentException">Thrown when the provided word is null or empty.</exception>
        private void AddWordToPlayerData(string word)
        {
            try
            {
                if (string.IsNullOrEmpty(word))
                {
                    throw new ArgumentException("Word cannot be null or empty.", nameof(word));
                }

                playerData.CollectedWords.Add(word);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to add word to player data: {ex.Message}");
            }
        }




        /// <summary>
        /// Removes a word from the player's collected words list,
        /// if it is not null or empty.
        /// </summary>
        /// <param name="word">The word to add to the collected words list.</param>
        /// <exception cref="ArgumentException">Thrown when the provided word is null or empty.</exception>
        private void RemoveWordFromPlayerData(string word)
        {
            try
            {
                if (string.IsNullOrEmpty(word))
                {
                    throw new ArgumentException("Word cannot be null or empty.", nameof(word));
                }

                playerData.CollectedWords.Remove(word);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to remove word from player data: {ex.Message}");
            }
        }


        private List<string> HandlePlayerDataWordsExtracted(List<string> words)
        {
            Debug.Log($"Extracted {words.Count} words from player data.");

            // Liste hvor de fundne ord skal tilf�jes
            List<string> updatedWordList = new List<string>();

            // Tilf�j de fundne ord til updatedWordList
            updatedWordList.AddRange(words);

            // Returner den opdaterede liste
            return updatedWordList;
        }









        //---------- templates and ideas for later------------

        #region ideas

        /// <summary>
        /// Adds or removes gold from the player's current total.
        /// </summary>
        /// <param name="amount">The amount of gold to add (positive) or remove (negative).</param>
        public void ModifyGold(int amount)
        {
            playerData.CurrentGoldAmount += amount;
        }

        /// <summary>
        /// Adds or removes XP from the player's current total.
        /// </summary>
        /// <param name="amount">The amount of XP to add (positive) or remove (negative).</param>
        public void ModifyXP(int amount)
        {
            playerData.CurrentXPAmount += amount;
            CheckForLevelUp();
        }

        /// <summary>
        /// Checks if the player has reached the XP threshold for leveling up.
        /// </summary>
        private void CheckForLevelUp()
        {
            // Check xp for level
            // if level up confitte around player
        }

        /// <summary>
        /// Updates the player's current position.
        /// </summary>
        /// <param name="newPosition">The new position to set.</param>
        public void UpdatePosition(Vector3 newPosition)
        {
            playerData.CurrentPosition = newPosition;
        }

        /// <summary>
        /// Sets or updates the player's username.
        /// </summary>
        /// <param name="newUsername">The new username.</param>
        public void SetUsername(string newUsername)
        {
            playerData.Username = newUsername;
        }

        /// <summary>
        /// Sets or updates the monster's name associated with the player.
        /// </summary>
        /// <param name="newMonsterName">The new monster name.</param>
        public void SetMonsterName(string newMonsterName)
        {
            playerData.MonsterName = newMonsterName;
        }

        /// <summary>
        /// Sets or updates the monster's color associated with the player.
        /// </summary>
        /// <param name="newColor">The new color.</param>
        public void SetMonsterColor(string newColor)
        {
            playerData.MonsterColor = newColor;
        }

        #endregion
    }
}

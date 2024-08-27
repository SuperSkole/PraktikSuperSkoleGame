using System;
using LoadSave;
using UnityEngine;

namespace Scenes.PlayerScene.Scripts
{
    /// <summary>
    /// Manages all player-related events, interactions, and data modifications.
    /// </summary>
    public class PlayerEventManager : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;

        // Event to trigger visual effects or other responses to leveling up
        public event Action OnLevelUp;

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
        }

        private void OnDisable()
        {
            PlayerEvents.OnWordValidated -= AddWordToPlayerData;
        }

        private void OnDestroy()
        {
            PlayerEvents.OnWordValidated -= AddWordToPlayerData;
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

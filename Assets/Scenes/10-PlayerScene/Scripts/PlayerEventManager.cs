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
        //public bool IsInCar { get; set; }

        // Event to trigger visual effects or other responses to leveling up
        public event Action OnLevelUp;
        public UnityEvent PlayerInteraction { get; set; } = new UnityEvent();
        public GameObject interactionIcon;

        void Start()
        {
            if (playerData == null)
            {
                Debug.LogError("PlayerData reference not set on PlayerEventManager.");
            }
        }


        private void OnEnable()
        {
            PlayerEvents.OnAddLetter += AddLetterToPlayerData;
            PlayerEvents.OnAddWord += AddWordToPlayerData;
            PlayerEvents.OnWordRemovedValidated += RemoveWordFromPlayerData;
            PlayerEvents.OnPlayerDataWordsExtracted += HandlePlayerDataWordsExtracted;
            PlayerEvents.OnPlayerDataLettersExtracted += HandlePlayerDataLettersExtracted;
            PlayerEvents.OnGoldChanged += ModifyGold;
            PlayerEvents.OnXPChanged += ModifyXP;
        }

        private void OnDisable()
        {
            PlayerEvents.OnAddLetter -= AddLetterToPlayerData;
            PlayerEvents.OnAddWord -= AddWordToPlayerData;
            PlayerEvents.OnWordRemovedValidated -= RemoveWordFromPlayerData;
            PlayerEvents.OnPlayerDataWordsExtracted -= HandlePlayerDataWordsExtracted;
            PlayerEvents.OnPlayerDataLettersExtracted -= HandlePlayerDataLettersExtracted;
            PlayerEvents.OnGoldChanged -= ModifyGold;
            PlayerEvents.OnXPChanged -= ModifyXP;
        }

        private void OnDestroy()
        {
            PlayerEvents.OnAddLetter -= AddLetterToPlayerData;
            PlayerEvents.OnAddWord -= AddWordToPlayerData;
            PlayerEvents.OnWordRemovedValidated -= RemoveWordFromPlayerData;
            PlayerEvents.OnPlayerDataWordsExtracted -= HandlePlayerDataWordsExtracted;
            PlayerEvents.OnPlayerDataLettersExtracted -= HandlePlayerDataLettersExtracted;
            PlayerEvents.OnGoldChanged -= ModifyGold;
            PlayerEvents.OnXPChanged -= ModifyXP;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                InvokeAction();
            }
        }
        public void InvokeAction()
        {
            try
            {
                interactionIcon.SetActive(false);
                GetComponent<SpinePlayerMovement>().StopPointAndClickMovement();
                PlayerInteraction.Invoke();
                PlayerInteraction = new UnityEvent();
            }
            catch { print("PlayerEventManager/InvokeAction/No playeraction"); }
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
        /// Adds a word to the player's collected wordsOrLetters list,
        /// if it is not null or empty.
        /// </summary>
        /// <param name="word">The word to add to the collected wordsOrLetters list.</param>
        /// <exception cref="ArgumentException">Thrown when the provided word is null or empty.</exception>
        private void AddWordToPlayerData(string word)
        {
            playerData.LifetimeTotalWords++;
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

        private void AddLetterToPlayerData(char Letter)
        {
            playerData.LifetimeTotalLetters++;
            try
            {
                playerData.CollectedLetters.Add(Letter);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to add word to player data: {ex.Message}");
            }
        }


        /// <summary>
        /// Removes a word from the player's collected wordsOrLetters list,
        /// if it is not null or empty.
        /// </summary>
        /// <param name="word">The word to add to the collected wordsOrLetters list.</param>
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

        /// <summary>
        /// Handles the wordsOrLetters extracted from playerdata
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        private List<string> HandlePlayerDataWordsExtracted(List<string> words)
        {

            // Liste hvor de fundne ord skal tilf�jes
            List<string> updatedWordList = new List<string>();

            // Tilf�j de fundne ord til updatedWordList
            updatedWordList.AddRange(words);

            // Returner den opdaterede liste
            return updatedWordList;
        }


        /// <summary>
        /// Handles the letters extracted from playerdata
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        private List<char> HandlePlayerDataLettersExtracted(List<char> letters)
        {
            Debug.Log($"Extracted {letters.Count} letters from player data.");

            // Liste hvor de fundne bogstaver skal tilf�jes
            List<char> updatedlettersList = new List<char>();

            // Tilf�j de fundne bogstaver til updatedlettersList
            updatedlettersList.AddRange(letters);

            // Returner den opdaterede liste
            return updatedlettersList;
        }

        /// <summary>
        /// Adds or removes gold from the player's current total.
        /// </summary>
        /// <param name="amount">The amount of gold to add (positive) or remove (negative).</param>
        public void ModifyGold(int amount)
        {
            playerData.PendingGoldAmount += amount;
        }

        /// <summary>
        /// Adds or removes XP from the player's current total.
        /// </summary>
        /// <param name="amount">The amount of XP to add (positive) or remove (negative).</param>
        public void ModifyXP(int amount)
        {
            playerData.PendingXPAmount += amount;
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









        //---------- templates and ideas for later------------

        #region ideas







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

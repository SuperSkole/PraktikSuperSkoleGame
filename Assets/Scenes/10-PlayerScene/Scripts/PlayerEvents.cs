using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes._10_PlayerScene.Scripts
{
    /// <summary>
    /// Manages player-related events through static delegates and methods.
    /// we use this class to <b>centralize</b> the logic for triggering events,
    /// that affect the player's state within the game.
    /// </summary>
    public static class PlayerEvents
    {
        // Define delegates for each type of event.
        // Delegates define the method signature for different event types, for consistent event handling.
        
        /// <summary>
        /// Delegate for events that pass a word.
        /// </summary>
        /// <param name="word">The word involved in the event.</param>
        /// <param name="dateTime">When did this happen.</param>
        public delegate void PlayerWordEventWithDateTime(string word, DateTime dateTime);

        /// <summary>
        /// Delegate for events that pass a position vector.
        /// </summary>
        /// <param name="position">The Vector3 position involved in the event.</param>
        public delegate void PlayerPositionEvent(Vector3 position);

        // Actions for sending word, letter and number to playerData
        public static event Action<string> OnAddWord;
        public static event Action<char> OnAddLetter;
        //public static event Action<char> OnAddNumber;

        public static event Func<List<string>,List<string>> OnPlayerDataWordsExtracted;

        public static event Func<List<char>, List<char>> OnPlayerDataLettersExtracted;
        
        //public static event Action<List<char>> OnPlayerDataNumbersExtracted;



        // Actions for removing word, letter or number from playerData
        public static event Action<string> OnWordRemovedValidated;

 

        // Events for adding or removing wordsOrLetters, letters, and numbers
        public static event PlayerWordEventWithDateTime OnWordAdded;
        public static event PlayerWordEventWithDateTime OnWordRemoved;
        public static event PlayerWordEventWithDateTime OnLetterAdded;
        public static event PlayerWordEventWithDateTime OnLetterRemoved;
        public static event PlayerWordEventWithDateTime OnNumberAdded;
        public static event PlayerWordEventWithDateTime OnNumberRemoved;

        // Events for modifying XP and gold
        public static event Action<int> OnXPChanged;
        public static event Action<int> OnGoldChanged;

        // Event for leveling up
        public static event Action OnLevelUp;

        // Event for updating position
        public static event PlayerPositionEvent OnPositionUpdated;

        // Events for setting or reading username, monster name, and monster color
        public static event Action<string> OnUsernameChanged;
        public static event Action<string> OnMonsterNameChanged;
        public static event Action<string> OnMonsterColorChanged;
        
        // Event for moving player
        public static event Action<GameObject> OnMovePlayerToBlock;
        public static event Action<GameObject> OnMovePlayerToPosition;

        // Methods to trigger each event and actions.
        // Utilizing the null-conditional operator to prevent invoking events with no subscribers.

       
/// <summary>
/// Raises an event after extracting wordsOrLetters from PlayerData, if any wordsOrLetters are found.
/// </summary>
       public static List<string> RaisePlayerDataWordsExtracted()
        {
            if (PlayerManager.Instance.PlayerData.CollectedWords.Count > 0)
            {
                return PlayerEvents.OnPlayerDataWordsExtracted?.Invoke(PlayerManager.Instance.PlayerData.CollectedWords);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Raises an event after extracting letters from PlayerData, if any letters are found.
        /// </summary>
        public static List<char> RaisePlayerDataLettersExtracted()
        {
            if (PlayerManager.Instance.PlayerData.CollectedLetters.Count > 0)
            {
                return PlayerEvents.OnPlayerDataLettersExtracted?.Invoke(PlayerManager.Instance.PlayerData.CollectedLetters);
            }
            else
            {
                return null;
            }
        }

        public static void RaiseAddWord(string word) => OnAddWord?.Invoke(word);
        public static void RaiseAddLetter(char letter) => OnAddLetter?.Invoke(letter);
        public static void RaiseWordRemovedValidated(string word) => OnWordRemovedValidated?.Invoke(word);

        public static void RaiseWordAdded(string word)
        {
            DateTime eventTime = DateTime.Now;
            OnWordAdded?.Invoke(word, eventTime); // Fire event with current DateTime
        }

        public static void RaiseLetterAdded(char letter)
        {
            DateTime eventTime = DateTime.Now;
            OnLetterAdded?.Invoke(letter.ToString(), eventTime); // Fire event with current DateTime
        }

        public static void RaiseWordRemoved(string word, DateTime dateTime) => OnWordRemoved?.Invoke(word, dateTime);
        public static void RaiseLetterRemoved(string letter, DateTime dateTime) => OnLetterRemoved?.Invoke(letter, dateTime);
        public static void RaiseNumberAdded(string number, DateTime dateTime) => OnNumberAdded?.Invoke(number, dateTime);
        public static void RaiseNumberRemoved(string number, DateTime dateTime) => OnNumberRemoved?.Invoke(number, dateTime);
        public static void RaiseXPChanged(int xp) => OnXPChanged?.Invoke(xp);
        public static void RaiseGoldChanged(int gold) => OnGoldChanged?.Invoke(gold);
        public static void RaiseLevelUp() => OnLevelUp?.Invoke();
        public static void RaisePositionUpdated(Vector3 position) => OnPositionUpdated?.Invoke(position);
        public static void RaiseUsernameChanged(string username) => OnUsernameChanged?.Invoke(username);
        public static void RaiseMonsterNameChanged(string name) => OnMonsterNameChanged?.Invoke(name);
        public static void RaiseMonsterColorChanged(string color) => OnMonsterColorChanged?.Invoke(color);
        
        public static void RaiseMovePlayerToBlock(GameObject position)
        {
            OnMovePlayerToBlock?.Invoke(position);
        }
        
        public static void RaiseMovePlayerToPosition(GameObject position)
        {
            OnMovePlayerToPosition?.Invoke(position);
        }
    }
}

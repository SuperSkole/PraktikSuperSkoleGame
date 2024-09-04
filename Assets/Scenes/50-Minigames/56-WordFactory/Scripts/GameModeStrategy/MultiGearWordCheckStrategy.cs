using System.Collections.Generic;
using CORE.Scripts;
using Scenes._50_Minigames._56_WordFactory.Scripts.Managers;
using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.GameModeStrategy
{
    public class MultiGearWordCheckStrategy : IWordCheckStrategy
    {
        private WordValidator validator;
        private HashSet<string> wordsCreated;
        private ScoreManager scoreManager; 
        private INotificationDisplay notificationDisplay;
    
        public void Initialize(WordValidator wordValidator, HashSet<string> createdWords, ScoreManager score, INotificationDisplay notification)
        {
            validator = wordValidator;
            wordsCreated = createdWords;
            scoreManager = score;
            notificationDisplay = notification;
        }

        public bool ValidateWord(string word, List<Transform> teeth)
        {
            return true;
        }
    }
}

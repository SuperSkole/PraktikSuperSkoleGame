using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Minigames.WordFactory.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private List<GameObject> gears = new List<GameObject>();

        public int numberOfGears = 2; // Default to 2 gears
        public int numberOfTeeth = 8; // Default to 8 teeth per gear
        public int difficultyLevel = 1; // Default difficulty level

        public event Action<GameObject> OnGearAdded;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        
        

        public void AddGear(GameObject gear)
        {
            gears.Add(gear);
            OnGearAdded?.Invoke(gear);
            //Debug.Log($"Added gear: {gear.name}");
        }

        public List<GameObject> GetGears()
        {
            return gears;
        }

        public int GetNumberOfGears()
        {
            return numberOfGears;
        }

        public int GetNumberOfTeeth()
        {
            return numberOfTeeth;
        }

        public int GetDifficultyLevel()
        {
            return difficultyLevel;
        }

        public void SetDifficultyLevel(int level)
        {
            difficultyLevel = level;
        }
    }
}
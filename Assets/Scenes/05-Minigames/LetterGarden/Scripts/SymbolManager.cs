using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

namespace Minigames.LetterGarden.Scripts
{

    /// <summary>
    /// Manages splined symbols for the lettergarden game
    /// </summary>
    public class SymbolManager : MonoBehaviour
    {
        /// <summary>
        /// a gameobject gets added to this list if it has a name with the format Capital<Letter>
        /// </summary>
        public Dictionary<char, GameObject> capitalLettersObjects;
        /// <summary>
        /// a gameobject gets added to this list if it has a name with the format Capital<Letter>
        /// </summary>
        public Dictionary<char, SplineContainer> capitalLetters;
        /// <summary>
        /// a gameobject gets added to this list if it has a name with the format Lowercase<Letter>
        /// </summary>
        public Dictionary<char, GameObject> lowercaseLettersObjects;
        /// <summary>
        /// a gameobject gets added to this list if it has a name with the format Lowercase<Letter>
        /// </summary>
        public Dictionary<char, SplineContainer> lowercaseLetters;
        /// <summary>
        /// a gameobject gets added to this list if it has a name with the format Lowercase<Letter>
        /// </summary>
        public Dictionary<int, GameObject> numbersObjects;
        /// <summary>
        /// a gameobject gets added to this list if it can be converted to an int
        /// </summary>
        public Dictionary<int, SplineContainer> numbers;

        [SerializeField] private List<GameObject>symbolPrefabs;
        /// <summary>
        /// Takes the gameobjects from the symbolprefabs list and sorts them into the various dictionaries
        /// </summary>
        void Start()
        {
            capitalLettersObjects = new Dictionary<char, GameObject>();
            lowercaseLettersObjects = new Dictionary<char, GameObject>();
            numbersObjects = new Dictionary<int, GameObject>();
            capitalLetters = new Dictionary<char, SplineContainer>();
            lowercaseLetters = new Dictionary<char, SplineContainer>();
            numbers = new Dictionary<int, SplineContainer>();
            //Takes each gameobject and sorts it based on its name
            foreach(GameObject gameObject in symbolPrefabs){
                
                int numName;
                //Adds the game object and splinecontainer of a capital letter to their dictionaries using the letter at the end as key
                if(gameObject.name.Contains("Capital")){
                    capitalLettersObjects.Add(gameObject.name[7], gameObject);
                    capitalLetters.Add(gameObject.name[7], gameObject.GetComponent<SplineContainer>());
                }
                //Adds the game object and splinecontainer of a lowercase letter to their dictionaries using the letter at the end as key
                else if(gameObject.name.Contains("Lowercase")){
                    lowercaseLettersObjects.Add(gameObject.name[9], gameObject);
                    lowercaseLetters.Add(gameObject.name[9], gameObject.GetComponent<SplineContainer>());
                }
                //Adds the game object and splinecontainer of a number to their dictionaries using the number as the key
                else if(Int32.TryParse(gameObject.name, out numName)){
                    numbersObjects.Add(numName, gameObject);
                    numbers.Add(numName, gameObject.GetComponent<SplineContainer>());
                }
                //Error in case a gameobject gets added to the list on accident or the name is wrongly formated
                else {
                    Debug.LogError("Could not find the type of symbol in " + gameObject.name);
                }
            }
        }
    }
}
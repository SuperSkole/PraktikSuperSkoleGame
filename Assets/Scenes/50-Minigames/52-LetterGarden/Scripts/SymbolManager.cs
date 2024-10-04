using System;
using System.Collections;
using System.Collections.Generic;
using CORE.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

namespace Scenes.Minigames.LetterGarden.Scripts
{

    /// <summary>
    /// Manages splined symbols for the lettergarden game
    /// </summary>
    public class SymbolManager : MonoBehaviour
    {
        /// <summary>
        /// a gameobject gets added to this list if it has a name with the format Capital<Letter>
        /// </summary>
        public static Dictionary<char, GameObject> capitalLettersObjects;

        /// <summary>
        /// a spline gets added to this list if it has a name with the format Capital<Letter>
        /// </summary>
        public static Dictionary<char, SplineContainer> capitalLetters;

        /// <summary>
        /// a gameobject gets added to this list if it has a name with the format Lowercase<Letter>
        /// </summary>
        public static Dictionary<char, GameObject> lowercaseLettersObjects;
        /// <summary>
        /// a spline gets added to this list if it has a name with the format Lowercase<Letter>
        /// </summary>
        public static Dictionary<char, SplineContainer> lowercaseLetters;
        /// <summary>
        /// a gameobject gets added to this list if it has a name with the format Lowercase<Letter>
        /// </summary>
        public static Dictionary<int, GameObject> numbersObjects;
        /// <summary>
        /// a spline gets added to this list if it can be converted to an int
        /// </summary>
        public static Dictionary<int, SplineContainer> numbers;

        /// <summary>
        /// a gameobject gets added to this list if it has a name used as a key in symbolnames
        /// </summary>
        public static Dictionary<char, GameObject> symbolObjects;

        /// <summary>
        /// a spline gets added to this list if it has a name used as a key in symbolnames
        /// </summary>
        public static Dictionary<char, SplineContainer> symbols;

        [SerializeField] private List<GameObject>symbolPrefabs;
        
        private Dictionary<string, char> symbolNames = new Dictionary<string, char>();
        /// <summary>
        /// Takes the gameobjects from the symbolprefabs list and sorts them into the various dictionaries
        /// </summary>
        public void StartLoad()
        {
            AddSymbolNames();
            capitalLettersObjects = new Dictionary<char, GameObject>();
            lowercaseLettersObjects = new Dictionary<char, GameObject>();
            numbersObjects = new Dictionary<int, GameObject>();
            capitalLetters = new Dictionary<char, SplineContainer>();
            lowercaseLetters = new Dictionary<char, SplineContainer>();
            numbers = new Dictionary<int, SplineContainer>();
            //Takes each gameobject and sorts it based on its name
            foreach(GameObject gameObject in symbolPrefabs)
            {
                //Adds the game object and splinecontainer of a capital letter to their dictionaries using the letter at the end as key
                if(gameObject.name.Contains("Capital"))
                {
                    if(gameObject.name[7] == '(')
                    {
                        DanishLetterHandler(gameObject, true, gameObject.name.Substring(8,2));
                    }
                    else
                    {
                        capitalLettersObjects.Add(gameObject.name[7], gameObject);
                        capitalLetters.Add(gameObject.name[7], gameObject.GetComponent<SplineContainer>());
                    }
                    
                }
                //Adds the game object and splinecontainer of a lowercase letter to their dictionaries using the letter at the end as key
                else if (gameObject.name.Contains("Lowercase") || gameObject.name.Contains("LowerCase"))
                {
                    if (gameObject.name[9] == '(')
                    {
                        DanishLetterHandler(gameObject, false, gameObject.name.Substring(10,2));
                    }
                    else
                    {
                        lowercaseLettersObjects.Add(gameObject.name.ToLower()[9], gameObject);
                        lowercaseLetters.Add(gameObject.name.ToLower()[9], gameObject.GetComponent<SplineContainer>());
                    }
                    
                }
                //Adds the game object and splinecontainer of a number to their dictionaries using the number as the key
                else if (Int32.TryParse(gameObject.name, out int numName))
                {
                    numbersObjects.Add(numName, gameObject);
                    numbers.Add(numName, gameObject.GetComponent<SplineContainer>());
                }
                else if(symbolNames.ContainsKey(gameObject.name))
                {
                    symbolObjects.Add(symbolNames[gameObject.name], gameObject);
                    symbols.Add(symbolNames[gameObject.name], gameObject.GetComponent<SplineContainer>());
                }
                //Error in case a gameobject gets added to the list on accident or the name is wrongly formated
                else
                {
                    Debug.LogError("Could not find the type of symbol in " + gameObject.name);
                }
            }
        }

        private void AddSymbolNames()
        {
            symbolNames.Add("Ampersand", '&');
        }


        /// <summary>
        /// Translates the letter replacement from files t the equivelent danish letter
        /// </summary>
        /// <param name="gameObject">The gameobject which should be inserted on listst</param>
        /// <param name="isCapitalLetter"></param>
        /// <param name="character">The letter replacement from the file name</param>
        private void DanishLetterHandler(GameObject gameObject, bool isCapitalLetter, string character)
        {

            char letter = ' ';
            //Finds out which letter it is
            switch (character.ToUpper())
            {
                case "AA":
                    letter = 'Å';
                    break;
                case "AE":
                    letter = 'Æ';
                    break;
                case "OE":
                    letter = 'Ø';
                    break;
                default:
                    Debug.LogError("No character could be found for " + character);
                    break;
            }
            //Inserts the letter into the correct dictionary
            if (isCapitalLetter)
            {
                capitalLettersObjects.Add(letter, gameObject);
                capitalLetters.Add(letter, gameObject.GetComponent<SplineContainer>());
            }
            else 
            {
                lowercaseLettersObjects.Add(char.ToLower(letter), gameObject);
                lowercaseLetters.Add(char.ToLower(letter), gameObject.GetComponent<SplineContainer>());
            }
        }
    }
}
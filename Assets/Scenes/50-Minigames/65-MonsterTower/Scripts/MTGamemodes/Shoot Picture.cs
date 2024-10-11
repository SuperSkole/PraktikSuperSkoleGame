using Analytics;
using CORE;
using CORE.Scripts;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Scenes._50_Minigames._65_MonsterTower.Scrips.MTGameModes
{
    public class ShootPicture: IMTGameMode
    {
        private string previousRetrievedAnswer;

        /// <summary>
        /// Will be called by the TowerManager to create a brick with the correct answer
        /// </summary>
        /// <param name="str">the correct answer will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void SetCorrectAnswer(string str, TowerManager manager)
        {
            manager.soloImage.texture = ImageManager.GetImageFromLetter(str);
        }

        /// <summary>
        /// Will be called by the TowerManager to create a brick with an (usually random) incorrect answer
        /// </summary>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void SetWrongAnswer(TowerManager manager,string correctAnswer)
        {
            var rndImageWithKey = ImageManager.GetRandomImageWithKey();

            while(rndImageWithKey.Item2==correctAnswer)
            {
                rndImageWithKey = ImageManager.GetRandomImageWithKey();
            }

            manager.soloImage.texture = rndImageWithKey.Item1;
            manager.imageKey = rndImageWithKey.Item2;

            
        }

        /// <summary>
        /// Sets the answer key, which will tell the player which brick is correct. Uses the opposite medium of SetCorrectAnswer
        /// </summary>
        /// <param name="str">The answer key will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void GetDisplayAnswer(string str, TowerManager manager)
        {
            manager.displayBox.text = str;
        }

        /// <summary>
        /// create a series of answers
        /// </summary>
        /// <param name="count">number of answers to create</param>
        /// <returns>Returns a set of answers strings to be used by the towerManager</returns>
        public string[] GenerateAnswers(int count)
        {
            string[] returnedString = new string[count];

            List<ILanguageUnit> languageUnits = GameManager.Instance.DynamicDifficultyAdjustmentManager.GetNextLanguageUnitsBasedOnLevel(80);

            List<ILanguageUnit> letters = new List<ILanguageUnit>();

            foreach (var item in languageUnits)
            {
                if (item.LanguageUnitType == LanguageUnit.Letter)
                {
                    letters.Add(item);
                }
            }

            for (int i = 0; i < count; i++)
            {
                returnedString[i] = letters[Random.Range(0, 15)].Identifier;


                bool checkIfAvailable = true;

                while (checkIfAvailable)
                {
                    switch (returnedString[i].ToLower())
                    {
                        case "y":
                            returnedString[i] = letters[Random.Range(0, 15)].Identifier;
                            break;

                        case "z":
                            returnedString[i] = letters[Random.Range(0, 15)].Identifier;
                            break;

                        case "w":
                            returnedString[i] = letters[Random.Range(0, 15)].Identifier;
                            break;

                        case "c":
                            returnedString[i] = letters[Random.Range(0, 15)].Identifier;
                            break;

                        case "q":
                            returnedString[i] = letters[Random.Range(0, 15)].Identifier;
                            break;

                        case "x":
                            returnedString[i] = letters[Random.Range(0, 15)].Identifier;
                            break;

                        default:
                            checkIfAvailable = false;
                            break;
                    }
                }

                while (returnedString[i] == previousRetrievedAnswer)
                {
                    returnedString[i] = letters[Random.Range(0, 15)].Identifier;


                    checkIfAvailable = true;

                    while (checkIfAvailable)
                    {
                        switch (returnedString[i].ToLower())
                        {
                            case "y":
                                returnedString[i] = letters[Random.Range(0, 15)].Identifier;
                                break;

                            case "z":
                                returnedString[i] = letters[Random.Range(0, 15)].Identifier;
                                break;

                            case "w":
                                returnedString[i] = letters[Random.Range(0, 15)].Identifier;
                                break;

                            case "c":
                                returnedString[i] = letters[Random.Range(0, 15)].Identifier;
                                break;

                            case "q":
                                returnedString[i] = letters[Random.Range(0, 15)].Identifier;
                                break;

                            case "x":
                                returnedString[i] = letters[Random.Range(0, 15)].Identifier;
                                break;

                            default:
                                checkIfAvailable = false;
                                break;
                        }
                    }
                }
                previousRetrievedAnswer = returnedString[i];
            }

            return returnedString;
        }
        /// <summary>
        /// changes the prefab of the TowerManager so we only apply 1 image to the bricks
        /// </summary>
        /// <param name="manager">a reference back to the towermanager</param>
        public void SetAnswerPrefab(TowerManager manager)
        {
            manager.answerHolderPrefab = manager.singleImageHolderPrefab;
            manager.soloImage = manager.singleImageHolderPrefab.transform.GetChild(0).GetComponent<RawImage>();
            manager.descriptionText.text = "Tryk på ammunition for at lade. \nSkyd det billede der passer med ordet";
        }
    }

}
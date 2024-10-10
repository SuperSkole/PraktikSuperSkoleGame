using Analytics;
using CORE;
using CORE.Scripts;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes._50_Minigames._65_MonsterTower.Scrips.MTGameModes
{
    public class Level5 : IMTGameMode
    {
        private string previousRetrievedAnswer;


        /// <summary>
        /// Will be called by the TowerManager to create a brick with the correct answer
        /// </summary>
        /// <param name="str">the correct answer will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void SetCorrectAnswer(string str, TowerManager manager)
        {

            Debug.Log("setCorrect="+str);
            manager.soloImage.texture = ImageManager.GetImageFromLetter(str);


        }

        /// <summary>
        /// Will be called by the TowerManager to create a brick with an (usually random) incorrect answer
        /// </summary>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void SetWrongAnswer(TowerManager manager,string correctAnswer)
        {
            var rndImageWithKey = ImageManager.GetRandomImageWithKey();

            while (rndImageWithKey.Item2 == correctAnswer)
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

            

            AudioClip clip= LetterAudioManager.GetAudioClipFromLetter(str+"1");

            manager.VoiceClip = clip;

        }

        /// <summary>
        /// create a series of answers
        /// </summary>
        /// <param name="count">number of answers to create</param>
        /// <returns>Returns a set of answers strings to be used by the towerManager</returns>
        public string[] GenerateAnswers(int count)
        {


            string[] returnedString = new string[count];

            List<ILanguageUnit> words = GameManager.Instance.DynamicDifficultyAdjustmentManager.GetNextLanguageUnitsBasedOnLevel(15);

            for (int i = 0; i < count; i++)
            {
                returnedString[i] = words[Random.Range(0, 15)].Identifier;


                bool checkIfAvailable = true;

                while (checkIfAvailable)
                {
                    switch (returnedString[i].ToLower())
                    {
                        case "y":
                            returnedString[i] = words[Random.Range(0, 15)].Identifier;
                            break;

                        case "z":
                            returnedString[i] = words[Random.Range(0, 15)].Identifier;
                            break;

                        case "w":
                            returnedString[i] = words[Random.Range(0, 15)].Identifier;
                            break;

                        case "c":
                            returnedString[i] = words[Random.Range(0, 15)].Identifier;
                            break;

                        case "q":
                            returnedString[i] = words[Random.Range(0, 15)].Identifier;
                            break;

                        case "x":
                            returnedString[i] = words[Random.Range(0, 15)].Identifier;
                            break;

                        default:
                            checkIfAvailable = false;
                            break;
                    }
                }

                while (returnedString[i] == previousRetrievedAnswer)
                {
                    returnedString[i] = words[Random.Range(0, 15)].Identifier;


                    checkIfAvailable = true;

                    while (checkIfAvailable)
                    {
                        switch (returnedString[i].ToLower())
                        {
                            case "y":
                                returnedString[i] = words[Random.Range(0, 15)].Identifier;
                                break;

                            case "z":
                                returnedString[i] = words[Random.Range(0, 15)].Identifier;
                                break;

                            case "w":
                                returnedString[i] = words[Random.Range(0, 15)].Identifier;
                                break;

                            case "c":
                                returnedString[i] = words[Random.Range(0, 15)].Identifier;
                                break;

                            case "q":
                                returnedString[i] = words[Random.Range(0, 15)].Identifier;
                                break;

                            case "x":
                                returnedString[i] = words[Random.Range(0, 15)].Identifier;
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
            manager.hearLetterButton.SetActive(true);
            manager.answerHolderPrefab = manager.singleImageHolderPrefab;
            manager.soloImage = manager.singleImageHolderPrefab.transform.GetChild(0).GetComponent<RawImage>();

            manager.descriptionText.text = "Tryk på ammunition for at lade. \nTryk På den grønne knap og skyd billedet der har ens forlyd";
        }
    }

}
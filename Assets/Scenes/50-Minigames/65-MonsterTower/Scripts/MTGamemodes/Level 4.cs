using Analytics;
using CORE;
using CORE.Scripts;
using Letters;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes._50_Minigames._65_MonsterTower.Scrips.MTGameModes
{
    public class Level4 : IMTGameMode
    {
        private string previousRetrievedAnswer;




        /// <summary>
        /// Will be called by the TowerManager to create a brick with the correct answer
        /// </summary>
        /// <param name="str">the correct answer will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void SetCorrectAnswer(string str, TowerManager manager)
        {


            manager.textOnBrick.text = str.ToUpper();


        }

        /// <summary>
        /// Will be called by the TowerManager to create a brick with an (usually random) incorrect answer
        /// </summary>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void SetWrongAnswer(TowerManager manager,string correctAnswer)
        {
            var rndLetterWithKey = LetterManager.GetRandomLetter();

            while (rndLetterWithKey == correctAnswer.ToCharArray()[0])
            {
                rndLetterWithKey = LetterManager.GetRandomLetter();
            }

            manager.textOnBrick.text = rndLetterWithKey.ToString();



            manager.imageKey = rndLetterWithKey.ToString();


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

            List<ILanguageUnit> languageUnits = GameManager.Instance.DynamicDifficultyAdjustmentManager.GetNextLanguageUnitsBasedOnLevel(80);

            List<ILanguageUnit> letters = new List<ILanguageUnit>();

            LetterData modeLetterType = (LetterData)languageUnits[0];

            foreach (var item in languageUnits)
            {
                if (item.LanguageUnitType == LanguageUnit.Letter)
                {
                    LetterData letterData = (LetterData)item;
                    if ( GameManager.Instance.PlayerData.PlayerLanguageLevel>=2)
                    { 
                        letters.Add(item);
                    }
                  else if (letterData.Category == modeLetterType.Category)
                  {
                        letters.Add(item);
                  }
                }
            }

            for (int i = 0; i < count; i++)
            {

                //Code to make sure that the previous answer is not getting repeated imediatly after. 

                returnedString[i] = letters[Random.Range(0, 10)].Identifier;

                while (returnedString[i] == previousRetrievedAnswer)
                {

                    returnedString[i] = letters[Random.Range(0, 10)].Identifier;
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
            manager.answerHolderPrefab = manager.textHolderPrefab;

            manager.textOnBrick = manager.textHolderPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            manager.descriptionText.text = "Tryk på ammunition for at lade. \nTryk På den grønne knap og skyd det bogstav du hørte";
        }
    }

}
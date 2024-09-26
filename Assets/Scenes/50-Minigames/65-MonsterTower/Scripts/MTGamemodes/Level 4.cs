using CORE.Scripts;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes._50_Minigames._65_MonsterTower.Scrips.MTGameModes
{
    public class Level4 : MonoBehaviour, IMTGameMode
    {

        List<char> FMNSConsonants = LetterManager.GetFMNSConsonants();



        /// <summary>
        /// Will be called by the TowerManager to create a brick with the correct answer
        /// </summary>
        /// <param name="str">the correct answer will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void SetCorrectAnswer(string str, TowerManager manager)
        {


            foreach (var item in FMNSConsonants)
            {
                if (item == str.ToCharArray()[0])
                {
                    manager.textOnBrick.text = item.ToString();
                }
            }


        }

        /// <summary>
        /// Will be called by the TowerManager to create a brick with an (usually random) incorrect answer
        /// </summary>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void SetWrongAnswer(TowerManager manager,string correctAnswer)
        {
            var rndVowelWithKey = LetterManager.GetRandomFMNSConsonant();

            while (rndVowelWithKey == correctAnswer.ToCharArray()[0])
            {
                rndVowelWithKey = LetterManager.GetRandomFMNSConsonant();
            }

            manager.textOnBrick.text = rndVowelWithKey.ToString();



            manager.imageKey = rndVowelWithKey.ToString();


        }

        /// <summary>
        /// Sets the answer key, which will tell the player which brick is correct. Uses the opposite medium of SetCorrectAnswer
        /// </summary>
        /// <param name="str">The answer key will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void GetDisplayAnswer(string str, TowerManager manager)
        {

            

            AudioClip clip= LetterAudioManager.GetAudioClipFromLetter(str+"1");

            manager.hearLetterButton.GetComponent<AudioSource>().clip = clip;

        }

        /// <summary>
        /// create a series of answers
        /// </summary>
        /// <param name="count">number of answers to create</param>
        /// <returns>Returns a set of answers strings to be used by the towerManager</returns>
        public string[] GenerateAnswers(int count)
        {


            string[] returnedString = new string[count];
            for (int i = 0; i < count; i++)
            {
                returnedString[i] = LetterManager.GetRandomFMNSConsonant().ToString();
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

            manager.descriptionText.text = "Tryk p� ammunition for at lade. \nTryk P� den gr�nne knap og skyd det bogstav du h�rte";
        }
    }

}
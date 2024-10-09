using Analytics;
using CORE;
using CORE.Scripts;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes._50_Minigames._65_MonsterTower.Scrips.MTGameModes
{
    public class SentenceToPictures : IMTGameMode
    {
        /// <summary>
        /// sets the Tower Manager's images based on the sentence given
        /// </summary>
        /// <param name="str">sentence required to find the correct images</param>
        /// <param name="manager">a reference back to the tower manager calling the function, so we can find what to change</param>
        public void SetCorrectAnswer(string str, TowerManager manager)
        {
            List<string> words = new();
            StringBuilder currentWord = new();
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                if (ch == ' ')
                {
                    words.Add(currentWord.ToString());
                    currentWord = new StringBuilder();
                    continue;
                }

                currentWord.Append(ch);
            }
            words.Add(currentWord.ToString());
            if (words.Count < 3)
            {
                SetWrongAnswer(manager,str);
                return;
            }

            switch (words[1])
            {
                case "på":
                    manager.bottomImage.texture = ImageManager.GetImageFromWord(words[2]);
                    manager.topImage.texture = ImageManager.GetImageFromWord(words[0]);
                    break;
                case "under":
                    manager.topImage.texture = ImageManager.GetImageFromWord(words[2]);
                    manager.bottomImage.texture = ImageManager.GetImageFromWord(words[0]);
                    break;
                default:
                    Debug.Log("word is not in switch case please add it.");
                    break;
            }
        }

        /// <summary>
        /// sets random images from the image manager
        /// </summary>
        /// <param name="manager">a reference back to the manager calling this function so we know where to set the images</param>
        public void SetWrongAnswer(TowerManager manager,string correctAnswer)
        {

            
            var rndImageWithKey1 = ImageManager.GetRandomImageWithKey();
            var rndImageWithKey2 = ImageManager.GetRandomImageWithKey();

            while(rndImageWithKey2.Item2 + " p\u00e5 " + rndImageWithKey1.Item2==correctAnswer)
            {
                rndImageWithKey1 = ImageManager.GetRandomImageWithKey();
                rndImageWithKey2 = ImageManager.GetRandomImageWithKey();
            }

            manager.bottomImage.texture = rndImageWithKey1.Item1;
            manager.topImage.texture = rndImageWithKey2.Item1;

            manager.imageKey = rndImageWithKey2.Item2 + " \u00e5 " + rndImageWithKey1.Item2;



        }

        /// <summary>
        /// sets the displaybox text
        /// </summary>
        /// <param name="str">the sentence we are setting as the question displayed</param>
        /// <param name="manager">a reference back to the manager calling the function so that we can modify displaybox</param>
        public void GetDisplayAnswer(string str, TowerManager manager)
        {
            manager.displayBox.text = str;
        }


        /// <summary>
        /// makes a set of sentences
        /// </summary>
        /// <param name="count">amount of sentences to generate</param>
        /// <returns>an array of sentences</returns>
        public string[] GenerateAnswers(int count)
        {
            string sentence;
            string[] answers = new string[count];
            for (int i = 0; i < count; i++)
            {
                //update the range if more options are needed for the binding word of the sentence generator, the range should be (0, x) where x is the number of cases
                int rnd = Random.Range(0, 2);
                

                List<ILanguageUnit> words = GameManager.Instance.DynamicDifficultyAdjustmentManager.GetNextLanguageUnitsBasedOnLevel(10);
                
                switch (rnd)
                {
                    case 0:
                        sentence = words[Random.Range(0,10)].Identifier + " p\u00e5 " + words[Random.Range(0, 10)].Identifier;
                        break;

                    case 1:
                        sentence = words[Random.Range(0, 10)].Identifier + " under " + words[Random.Range(0, 10)].Identifier;
                        break;

                    default:
                        sentence = "ko på is";
                        Debug.Log("the number given was out of the range of expected results, defaulting to ko på is");
                        break;
                }
                answers[i] = sentence;
            }
            return answers;
        }

        /// <summary>
        /// changes the prefab of the TowerManager so we apply 2 images to the bricks
        /// </summary>
        /// <param name="manager">a reference back to the manager that called the function</param>
        public void SetAnswerPrefab(TowerManager manager)
        {
            manager.answerHolderPrefab = manager.imageHolderPrefab;
            manager.topImage = manager.answerHolderPrefab.transform.GetChild(0).GetComponent<RawImage>();
            manager.bottomImage = manager.answerHolderPrefab.transform.GetChild(1).GetComponent<RawImage>();

            manager.descriptionText.text = "Tryk på ammunition for at lade. \nSkyd det billede der passer til sætningen";
        }
    }
}
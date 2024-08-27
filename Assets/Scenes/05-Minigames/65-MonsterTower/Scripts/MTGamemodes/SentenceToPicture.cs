using CORE.Scripts;
using CORE.Scripts.GameRules;
using Scenes.Minigames.MonsterTower;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scenes.Minigames.MonsterTower.Scrips;

namespace Scenes.Minigames.MonsterTower.Scrips.MTGameModes
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
                Debug.Log("Tower expected 3 words sentences but got less. setting random image as correct image");
                SetWrongAnswer(manager);
                return;
            }

            switch (words[1])
            {
                case "p�":
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
        public void SetWrongAnswer(TowerManager manager)
        {
            var rndImageWithKey1 = ImageManager.GetRandomImageWithKey();
            var rndImageWithKey2 = ImageManager.GetRandomImageWithKey();

            manager.bottomImage.texture = rndImageWithKey1.Item1;
            manager.topImage.texture = rndImageWithKey2.Item1;



            manager.bottomImageKey = rndImageWithKey1.Item2;
            manager.topImageKey = rndImageWithKey2.Item2;
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
                int rnd = Random.Range(0, 2);
                string[] words = WordsForImagesManager.GetRandomWordForImage(2);
                switch (rnd)
                {
                    case 0:
                        sentence = words[0] + " p� " + words[1];
                        break;

                    case 1:
                        sentence = words[0] + " under " + words[1];
                        break;

                    default:
                        sentence = "ko p� is";
                        Debug.Log("the number given was out of the range of expected results, defaulting to ko p� is");
                        break;
                }
                answers[i] = sentence;
            }
            return answers;
        }
    }
}
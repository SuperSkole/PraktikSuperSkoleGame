using System.Collections.Generic;
using _99_Legacy;
using CORE.Scripts.Game_Rules;
using UnityEngine;

namespace Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes 
{
    /// <summary>
    /// Static class containing various helper methods for the gamemodes in the symbol eater minigame
    /// </summary>
    public static class GameModeHelper
    {

        public delegate void GetLetterCubeValue(LetterCube letterCube, bool correct);
        public delegate bool IsGameCompleted(); 

        /// <summary>
        /// activates a given amount of lettercubes with values given by the gamemode
        /// </summary>
        /// <param name="amount">how many lettercubes which should be activated</param>
        /// <param name="letterCubes">all lettercubes which could be activated</param>
        /// <param name="activeLetterCubes">all currently active lettercubes</param>
        /// <param name="getLetterCubeValue">method which activates the lettercube</param>
        /// <param name="correct">whether the value on the cube should be the correct one</param>
        public static void ActivateLetterCubes(int amount, List<LetterCube> letterCubes, List<LetterCube>activeLetterCubes, GetLetterCubeValue getLetterCubeValue, bool correct, IGameRules gameRules, Vector3 playerPos)
        {
            //Activates the given amount of lettercubes
            for(int i = 0; i < amount; i++)
            {
                LetterCube potientialCube = letterCubes[Random.Range(0, letterCubes.Count)];
                Vector3 pos = new Vector3(playerPos.x, potientialCube.transform.position.y, playerPos.z);

                //Check to ensure it does not try to activate an already active lettercube
                while(pos == potientialCube.transform.position || potientialCube.active)
                {
                    potientialCube = letterCubes[Random.Range(0, letterCubes.Count)];
                }
                
                getLetterCubeValue(potientialCube, correct);
            }
            int foundLetterCubes = 0;
            foreach(LetterCube letterCube in letterCubes)
            {
                if (letterCube.active)
                {
                    if (correct && gameRules.IsCorrectSymbol(letterCube.GetLetter()))
                    {
                        foundLetterCubes++;
                        activeLetterCubes.Add(letterCube);
                    }
                    else if (!correct && !gameRules.IsCorrectSymbol(letterCube.GetLetter()))
                    {
                        foundLetterCubes++;
                        activeLetterCubes.Add(letterCube);
                    }
                    if (foundLetterCubes == amount)
                    {
                        break;
                    }
                }
            }
            if(foundLetterCubes < amount)
            {
                //ActivateLetterCubes(amount - foundLetterCubes, letterCubes, activeLetterCubes, getLetterCubeValue, correct, gameRules, playerPos);
            }
        }

        /// <summary>
        /// Activates a random lettercube
        /// </summary>
        /// <param name="letterCubes">all lettercubes which could be activated</param>
        /// <param name="activeLetterCubes">all currently active lettercubes</param>
        /// <param name="getLetterCubeValue">method which activates the lettercube</param>
        /// <param name="correct">whether the value on the cube should be the correct one</param>
        public static void ActivateLetterCube(List<LetterCube> letterCubes, List<LetterCube>activeLetterCubes, GetLetterCubeValue getLetterCubeValue, bool correct)
        {
            LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            while(activeLetterCubes.Contains(potentialCube)){
                potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            }
            activeLetterCubes.Add(potentialCube);
            getLetterCubeValue(potentialCube, correct);
        }

        /// <summary>
        /// Handles deactivation of lettercubes replacing it and tells whether the current game is over
        /// </summary>
        /// <param name="amount">how many lettercubes which should be activated</param>
        /// <param name="letterCubes">all lettercubes which should be activated</param>
        /// <param name="activeLetterCubes">all currently active lettercubes</param>
        /// <param name="repeatLetter">Whether the new lettercube should have the same letter as the old one</param>
        /// <param name="getLetterCubeValue">method which activates the lettercube</param>
        /// <param name="isGameCompleted">method which tells whether the current game is over</param>
        /// <returns>whether the cube got replaced(true) or the current game should end(false)</returns>
        public static bool ReplaceOrVictory(LetterCube letterCube, List<LetterCube> letterCubes, List<LetterCube>activeLetterCubes, bool repeatLetter, GetLetterCubeValue getLetterCubeValue, IsGameCompleted isGameCompleted)
        {
            string letterCubeValue = letterCube.GetLetter();
            letterCube.Deactivate();
            
            
            LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            //Ensures that the new cube is not one which is already activated
            while(activeLetterCubes.Contains(potentialCube) && potentialCube.active)
            {
                potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            }
            activeLetterCubes.Remove(letterCube);
            //Checks whether the game is over
            if(isGameCompleted())
            {
                return false;
            }
            //activates the new cube with either the old cubes value or a new one
            else
            {
                if(repeatLetter)
                {
                    potentialCube.Activate(letterCubeValue, true);
                }
                else
                {
                    getLetterCubeValue(potentialCube, false);
                }
                return true;
            }
        }
    }
}
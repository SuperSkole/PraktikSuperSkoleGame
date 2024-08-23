
using System.Collections;
using System.Collections.Generic;
using Scenes.Minigames.SymbolEater.Scripts;
using UnityEngine;

namespace Scenes.Minigames.SymbolEater.Scripts.Gamemodes 
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
        /// <param name="letterCubes">all lettercubes which should be activated</param>
        /// <param name="activeLetterCubes">all currently active lettercubes</param>
        /// <param name="getLetterCubeValue">method which activates the lettercube</param>
        /// <param name="correct">whether the value on the cube should be the correct one</param>
        public static void ActivateLetterCubes(int amount, List<LetterCube> letterCubes, List<LetterCube>activeLetterCubes, GetLetterCubeValue getLetterCubeValue, bool correct){
            //Activates the given amount of lettercubes
            for(int i = 0; i < amount; i++)
            {
                LetterCube potientialCube = letterCubes[Random.Range(0, letterCubes.Count)];
                //Check to ensure it does not try to activate an already active lettercube
                while(activeLetterCubes.Contains(potientialCube))
                {
                    potientialCube = letterCubes[Random.Range(0, letterCubes.Count)];
                }
                activeLetterCubes.Add(potientialCube);
                getLetterCubeValue(potientialCube, correct);
            }
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
        public static bool ReplaceOrVictory(LetterCube letterCube, List<LetterCube> letterCubes, List<LetterCube>activeLetterCubes, bool repeatLetter, GetLetterCubeValue getLetterCubeValue, IsGameCompleted isGameCompleted){
            string letterCubeValue = letterCube.GetLetter();
            letterCube.Deactivate();
            
            
            LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            //Ensures that the new cube is not one which is already activated
            while(activeLetterCubes.Contains(potentialCube))
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
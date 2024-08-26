using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CORE.Scripts.GameRules
{

    /// <summary>
    /// Implementation of IGameRules for games where the player should complete a 
    /// </summary>
    public class FindNumberSeries : IGameRules
    {

            private int mminNumber;
            private int currentNumber;
            private int maxNumber;

            /// <summary>
            /// Returns the current number the player should look for
            /// </summary>
            /// <returns>Current number</returns>
            public string GetCorrectAnswer()
            {
                return currentNumber.ToString();
            }

            /// <summary>
            /// Returns a display string for helping the player
            /// </summary>
            /// <returns>information for the player</returns>
            public string GetDisplayAnswer()
            {
                if(currentNumber == mminNumber)
                {
                    return "Find tallene fra " + mminNumber + " til " + maxNumber + " i rækkefølge. Du har foreløbigt ikke fundet nogen";
                }
                else
                {
                    return "Find tallene fra " + mminNumber + " til " + maxNumber + " i rækkefølge. Du har foreløbigt fundet tallene op til " + currentNumber;
                }
            }

            /// <summary>
            /// Returns a random wrong answer
            /// </summary>
            /// <returns>a wrong answer</returns>
            public string GetWrongAnswer()
            {
                int wrongAnswer = Random.Range(mminNumber - 5, maxNumber + 5);
                while(wrongAnswer == currentNumber)
                {
                    wrongAnswer = Random.Range(mminNumber - 5, maxNumber + 5);
                }
                return wrongAnswer.ToString();
            }

            /// <summary>
            /// Checks whether the given symbol is correct
            /// </summary>
            /// <param name="symbol">The symbol to be checked</param>
            /// <returns>Whether the symbol is the same as the correct one</returns>
            public bool IsCorrectSymbol(string symbol)
            {
                return symbol == currentNumber.ToString();
            }

            /// <summary>
            /// Checks whether the sequence is complete
            /// </summary>
            /// <returns>Whether the sequence is complete</returns>
            public bool SequenceComplete()
            {
                return currentNumber == maxNumber;
            }

            /// <summary>
            /// Updates the min and max numbers if it is the first time it is run. Otherwise just updates currentNumber
            /// </summary>
            public void SetCorrectAnswer()
            {
                if(maxNumber == 0 || currentNumber == maxNumber)
                {
                    mminNumber = Random.Range(6, 85);
                    currentNumber = mminNumber;
                    maxNumber = Random.Range(mminNumber + 5, mminNumber + 15);
                }
                else {
                    currentNumber++;
                }
            }


    }
}
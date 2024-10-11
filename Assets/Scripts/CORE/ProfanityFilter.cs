using System;
using System.Collections.Generic;
using Spine;

using UnityEditor.Experimental.GraphView;

namespace CORE
{
    public static class ProfanityFilter
    {
        private static readonly HashSet<string> bannedWords = new HashSet<string>
        {
            "ass", "pussy", "fuck", "�ndsforsnottet", "skidespr�ller","skvadderhoved", "pestspreder", "skvatpisser", 
            "spytslikker", "torskepande", "underm�ler", "�ndsam�be", "dangleb�r", "dingleb�r", "lortefj�s", "lusepuster",
            "patteb�rn", "pladderabe", "ringleb�r", "satanedeme", "sjatpisser", "t�sedreng", "�ndsbolle", "�rkefjols",
            "�gleyngel", "agurketud", "forpulede", "kvabodder", "kvajhoved", "kvajpande", "lorte�re", "pattebarn",
            "slapsvans", "forpulet", "nakkeost", "narrehat", "pikfj�s", "pikhoved", "skiderik", "abelort", "fandeme",
            "f�king", "f�kker", "m�gdyr", "narr�v", "urinere", "bovlam", "fanden", "fandme", "focker", "narhat", "satan",
            "satme", "skide", "skvat", "fock", "f�k", "lort", "pjok", "skid", "svin", "urin", "sgu"         
        };

        /// <summary>
        /// Checks if the given input contains any banned words.
        /// </summary>
        /// <param name="input">The input string to check for profanity.</param>
        /// <returns>True if profanity is found, false otherwise.</returns>
        public static bool ContainsProfanity(string input)
        {
            string loweredInput = input.ToLower();
            foreach (string bannedWord in bannedWords)
            {
                if (loweredInput.Contains(bannedWord))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
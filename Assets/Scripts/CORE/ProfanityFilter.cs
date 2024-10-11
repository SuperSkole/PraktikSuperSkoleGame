using System;
using System.Collections.Generic;
using Spine;
namespace CORE
{
    public static class ProfanityFilter
    {
        private static readonly HashSet<string> bannedWords = new HashSet<string>
        {
            "ass", "pussy", "fuck", "åndsforsnottet", "skidespræller","skvadderhoved", "pestspreder", "skvatpisser", 
            "spytslikker", "torskepande", "undermåler", "åndsamøbe", "danglebær", "dinglebær", "lortefjæs", "lusepuster",
            "pattebørn", "pladderabe", "ringlebær", "satanedeme", "sjatpisser", "tøsedreng", "åndsbolle", "ærkefjols",
            "øgleyngel", "agurketud", "forpulede", "kvabodder", "kvajhoved", "kvajpande", "lorteøre", "pattebarn",
            "slapsvans", "forpulet", "nakkeost", "narrehat", "pikfjæs", "pikhoved", "skiderik", "abelort", "fandeme",
            "fåking", "fåkker", "møgdyr", "narrøv", "urinere", "bovlam", "fanden", "fandme", "focker", "narhat", "satan",
            "satme", "skide", "skvat", "fock", "fåk", "lort", "pjok", "skid", "svin", "urin", "sgu", "nigger","negger","neger"         
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
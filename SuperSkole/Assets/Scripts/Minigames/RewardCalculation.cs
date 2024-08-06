using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RewardCalculation : MonoBehaviour
{

    public (int XP, int Gold) CalculateRewards(float completionTime, float maxTime, float minTime, float maxXP, float minXP, int difficulty, float goldPerXP)
    {
        // Validate inputs to prevent logical errors or unexpected behavior
        if (minTime >= maxTime)
        {
            throw new ArgumentException("minTime must be less than maxTime");
        }
        if (difficulty < 0)
        {
            throw new ArgumentException("difficulty must be a non-negative integer");
        }
        if (goldPerXP < 0)
        {
            throw new ArgumentException("goldPerXP must be a non-negative value");
        }
        if (maxXP < minXP)
        {
            throw new ArgumentException("maxXP must be greater than or equal to minXP");
        }

        // Calculate base XP based on completion time
        float baseXP;
        if (completionTime <= minTime)
        {
            baseXP = maxXP;
        }
        else if (completionTime >= maxTime)
        {
            baseXP = minXP;
        }
        else
        {
            float timeRatio = (completionTime - minTime) / (maxTime - minTime);
            timeRatio = Mathf.Clamp(timeRatio, 0, 1); // Ensure timeRatio is between 0 and 1
            baseXP = maxXP - (timeRatio * (maxXP - minXP));
        }

        // Apply difficulty multiplier to base XP
        float difficultyMultiplier = 1 + difficulty * 0.1f;
        float finalXP = baseXP * difficultyMultiplier;

        // Calculate gold earned based on final XP
        float finalGold = finalXP * goldPerXP;

        int finalGoldAsInt = (int)Math.Ceiling(finalGold);
        int finalXpAsInt = (int)Math.Ceiling(finalXP);


        return (finalXpAsInt, finalGoldAsInt);
    }


}

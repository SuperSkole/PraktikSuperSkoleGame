using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class RacingGameManager : MonoBehaviour
{
    //public StateNameController stateNameController;
    int gold;
    int xp;

    public float completionTime = 0;
    public float maxTime => 15;
    public float minTime => 5;
    public float maxXP => 15;
    public float minXP => 10;
    public int difficulty => 2;
    public float goldPerXP => 3;

    RacingGameCore racingGameCore;

    private void Start()
    {
        racingGameCore = GetComponent<RacingGameCore>();
        //stateNameController = GetComponent<StateNameController>();
    }

    private void Update()
    {
      
        completionTime = racingGameCore.Timer;
    }

    public void EndGame()
    {
        RewardCalculation rewardCalculator = new RewardCalculation();

        var (XP, Gold) = rewardCalculator.CalculateRewards(completionTime, maxTime, minTime, maxXP, minXP, difficulty, goldPerXP);

        EndGameUI.Instance.DisplayRewards(XP, Gold, completionTime);
        
        StateNameController.SetXPandGoldandCheck(XP, Gold);

    }
}
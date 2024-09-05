using _99_Legacy;
using Minigames;
using Unity.VisualScripting;
using UnityEngine;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    public class RacingGameManager : MonoBehaviour
    {
        //public StateNameController stateNameController;
        private readonly int gold;
        private readonly int xp;

        public float completionTime = 0;
        public float maxTime => 15;
        public float minTime => 5;
        public float maxXP => 15;
        public float minXP => 10;
        public int difficulty => 2;
        public float goldPerXP => 3;

        private RacingGameCore racingGameCore;

        /// <summary>
        /// Gets the racing game core.
        /// </summary>
        private void Start()
        {
            racingGameCore = GetComponent<RacingGameCore>();
            //stateNameController = GetComponent<StateNameController>();
        }

        /// <summary>
        /// Updates the completion timer.
        /// </summary>
        private void Update()
        {
            completionTime = racingGameCore.Timer;
        }

        /// <summary>
        /// Game is over, hand out rewards.
        /// </summary>
        public void EndGame()
        {
            RewardCalculation rewardCalculator = this.AddComponent<RewardCalculation>();

            var (XP, Gold) = rewardCalculator.CalculateRewards(completionTime, maxTime, minTime, maxXP, minXP, difficulty, goldPerXP);

            EndGameUI.Instance.DisplayRewards(XP, Gold, completionTime);

            StateNameController.SetXPandGoldandCheck(XP, Gold);
        }
    }
}
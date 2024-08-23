using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CORE.Scripts.GameRules;

namespace CORE.Scripts
{
    public interface IMinigameSetup
    {
        /// <summary>
        /// Use this for whatever function sets up the main game mode and rules of your minigame, usually replaces a start function
        /// </summary>
        /// <param name="gameMode">takes a gamemode that your manager should use</param>
        /// <param name="gameRules">takes a game rule set that your manager should use</param>
        public void SetupGame(IGenericGameMode gameMode, IGameRules gameRules);
    }
}
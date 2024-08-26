using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.GameMode
{
    public interface IGameModeSetter
    {
        /// <summary>
        /// Each script using this should have a switch case that turns a string into a game mode
        /// </summary>
        /// <param name="mode">string to represent what gamemode needs to be created and set</param>
        /// <param name="rules">string to represent what gamerules need to be created and set</param>
        public void SetModeAndRules(string mode, string rules);
    }
}
using CORE.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    public interface IRacingGameMode : IGenericGameMode
    {
        public string returnMode(); 
        public string displayObjective();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.GameMode
{
    public interface IGameModeSetter
    {
        public void SetModeAndRules(string mode, string rules);
    }
}
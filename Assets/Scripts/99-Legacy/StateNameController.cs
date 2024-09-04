using UnityEngine;
using UnityEngine.SceneManagement;

namespace _99_Legacy
{
    /// <summary>
    /// This class handles the different varaibles that gets transfered between the scenes
    /// Every variable has to be static, so ONLY 1 can exist
    /// https://www.youtube.com/watch?v=WchH-JCwVI8 Video for Preserving Data between Scene Loading/Switching
    /// 
    /// </summary>
    public class StateNameController : MonoBehaviour
    {
        private static int xpToGive;
        private static int goldToGive;
        private static bool checkXPandGold = false;
        //[SerializeField] private SaveGameToJson SaveGameToJson;

        /// <summary>
        /// Call this func from a missionscreen when its complete to set the values so the mainGameLoop can use them later
        /// </summary>
        /// <param name="xp"></param>
        /// <param name="gold"></param>
        public static void SetXPandGoldandCheck(int xp, int gold) { xpToGive = xp; goldToGive = gold; checkXPandGold = true;
            Debug.Log("StateNameController/SetXPandGoldandCheck/XP, Gold and check has been set");
        }
        /// <summary>
        /// Returns xp and gold values
        /// </summary>
        /// <returns></returns>
        public static (int, int) GetXPandGold() { return (xpToGive, goldToGive); }
        /// <summary>
        /// Call this func when the mainGameLoop has used the values
        /// </summary>
        public static void ResetXPandGoldandCheck() { xpToGive = 0; goldToGive = 0; checkXPandGold = false;
            Debug.Log("StateNameController/ResetXPandGoldandCheck/Gold, XP and check has been reset");
        }
        /// <summary>
        /// Returns "true" or "false" depending on a mission has been completed.
        /// </summary>
        /// <returns></returns>
        public static bool CheckIfXPHasGained() { return checkXPandGold; }

        //Needs to be able to change the mission
        public void ChangeToGameScene()
        {
            //SaveGameToJson.SaveToJson();
            SceneManager.LoadScene("MiniRacingGame");
        }
        public void ChangeToTownScene()
        {
            //SaveGameToJson.LoadFromJson();
            SceneManager.LoadScene("Town");
        }
    }
}

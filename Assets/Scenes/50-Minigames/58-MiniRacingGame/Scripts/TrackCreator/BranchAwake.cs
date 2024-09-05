using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    public class BranchAwake : MonoBehaviour
    {
        public List<TextMeshProUGUI> leftText;
        public List<TextMeshProUGUI> rightText;
        public List<TextMeshProUGUI> timerText;
        public List<Image> billBoard;
        public delegate void SendText(string branchName, TextMeshProUGUI newText);
        public static event SendText OnBranchTextAwaken;
        public delegate void SendImage(Image newBillboard);
        public static event SendImage OnBranchImageAwaken;

        /// <summary>
        /// Checks all parts of a checkpoint when waking up and sends it to racing core
        /// </summary>
        private void Start()
        {
            foreach (TextMeshProUGUI text in leftText)
            {
                OnBranchTextAwaken("left", text);
            }
            foreach (TextMeshProUGUI text in rightText)
            {
                OnBranchTextAwaken("right", text);
            }
            foreach (TextMeshProUGUI text in timerText)
            {
                OnBranchTextAwaken("timer", text);
            }
            foreach (Image image in billBoard)
            {
                OnBranchImageAwaken(image);
            }
        }
    }
}

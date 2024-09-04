using UnityEngine;

namespace UI
{
    public class ButtonTest : MonoBehaviour
    {
        public void OptionClick()
        {
            Debug.Log("Click!");
        }

        public void OptionHoverEnter()
        {
            Debug.Log("Hover Enter!");
        }

        public void OptionHoverExit()
        {
            Debug.Log("Hover Exit!");
        }
    }
}

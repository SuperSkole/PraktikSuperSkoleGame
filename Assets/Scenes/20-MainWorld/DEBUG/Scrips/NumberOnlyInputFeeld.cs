using TMPro;
using UnityEngine;

namespace Scenes._20_MainWorld.DEBUG.Scrips
{
    public class NumberOnlyInputFeeld : MonoBehaviour
    {
        [SerializeField] TMP_InputField textMeshPro;
        string oldValue = "";

        /// <summary>
        /// used to make a input field numbers only
        /// </summary>
        public void OnChangeValue()
        {
            string value = textMeshPro.text;
            if (int.TryParse(value,out _) || value == "")
            {
                oldValue = value;
                return;
            }
            textMeshPro.text = oldValue;
        }
    }
}
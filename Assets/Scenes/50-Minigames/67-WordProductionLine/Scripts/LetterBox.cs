using Assets.Scenes._50_Minigames._67_WordProductionLine.Scripts;
using TMPro;
using UnityEngine;


namespace Scenes._50_Minigames._67_WordProductionLine.Scripts
{

    public class LetterBox : MonoBehaviour, IBox
    {

        [SerializeField]
        private GameObject letterBoxText;

        public TextMeshProUGUI letterText;

        public bool isSelected = false;

        void Start()
        {
            letterText = letterBoxText.GetComponent<TextMeshProUGUI>();
        }

        /// <summary>
        /// gets letter and displays on box
        /// </summary>
        /// <param name="letter"></param>
        public void GetLetter(string letter)
        {
            letterText.text = letter;
        }



        /// <summary>
        /// resets the Cube, so the momentum dosnt stay.
        /// </summary>
        /// <param name="cube"></param>
        public void ResetCube()
        {
            gameObject.transform.parent.gameObject.SetActive(false);
            Rigidbody rb = gameObject.GetComponentInParent<Rigidbody>(true);

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;


            rb.rotation = Quaternion.Euler(0, 0, 0);
            gameObject.transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        }


    }
}
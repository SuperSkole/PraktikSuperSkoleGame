using CORE.Scripts;
using Scenes._10_PlayerScene.Scripts;
using System.Linq;
using TMPro;
using UI.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scenes._20_MainWorld.DEBUG.Scrips
{


    public class DebugManager : MonoBehaviour
    {
        [SerializeField] private Animator menuAnimator;

        [SerializeField] private TMP_InputField xPField;
        [SerializeField] private TMP_InputField goldField;

        [SerializeField] private TMP_Dropdown tpList;

        [SerializeField] private GameObject debugDoorsParrentObject;

        [SerializeField] private InputAction openMenu;

        private float time = 0;
        private bool isShown = false;
        private GameObject player;
        private Rigidbody playerRigidbody;
        private XPBar xpbar;
        private BarMeter barMeter;

       /// <summary>
       /// The OnEnable function enables the input system, gets the player, and player's Rigidbody
       /// component.
       /// </summary>
        private void OnEnable()
        {
            openMenu.Enable();
            xpbar = FindObjectOfType<XPBar>();
            barMeter = FindObjectOfType<BarMeter>();
            player = PlayerManager.Instance.SpawnedPlayer;
            playerRigidbody = player.GetComponent<Rigidbody>();
        }

        /// <summary>
        /// The OnDisable function disables the openMenu.
        /// </summary>
        private void OnDisable()
        {
            openMenu.Disable();
        }

        /// <summary>
        /// used to toggle the menu
        /// </summary>
        private void Update()
        {
            time += Time.deltaTime;
            if(openMenu.ReadValue<float>() >= 0.1f && time >= 1)
            {
                menuAnimator.SetBool("ShowDebug", !isShown);
                isShown = !isShown;
                time = 0;
            }
        }

        /// <summary>
        /// stops the player from moving if you click on the menu
        /// </summary>
        public void StopPlayerMovement()
        {
            player.GetComponent<SpinePlayerMovement>().StopPointAndClickMovement();
        }

        /// <summary>
        /// adds a new random word to the player
        /// </summary>
        public void NewWord()
        {
            PlayerEvents.RaiseAddWord(WordsManager.GetRandomWordsFromCombinationByCount(1).First());
        }

        /// <summary>
        /// adds a new random letter to the player
        /// </summary>
        public void NewLetter()
        {
            PlayerEvents.RaiseAddLetter(LetterManager.GetRandomLetter());
        }

        /// <summary>
        /// adds an amount of xp to the player, whitch is spesified in the xPField
        /// </summary>
        public void AddXP()
        {
            PlayerEvents.RaiseXPChanged(int.Parse(xPField.text));
            xpbar.AddXP(int.Parse(xPField.text));
            xPField.text = "";
        }

        /// <summary>
        /// adds an amount of gold to the player, whitch is spesified in the goldField
        /// </summary>
        public void AddGold()
        {
            barMeter.ChangeValue(int.Parse(goldField.text));
            goldField.text = "";
        }

        /// <summary>
        /// teleports the player to the given location thru the dropdown
        /// </summary>
        public void TPToLocation()
        {
            string location = tpList.options[tpList.value].text;

            switch (location) //TODO add tp functunalety
            {
                case "EXAMPLE": //this is an example of how to tp the player to a location with the new Rigidbody movement system
                    playerRigidbody.position  = new Vector3(0,0,0);
                    player.transform.position = new Vector3(0, 0, 0);
                    break;
                default:
                    Debug.Log($"DEBUG MENU: cant find {location} in switch for locations. Script: DebugManager");
                    break;
            }
        }

        /// <summary>
        /// activats and deactivats the debug doors for faster testing
        /// </summary>
        public void ToggleDebugDoors()
        {
            debugDoorsParrentObject.SetActive(!debugDoorsParrentObject.activeInHierarchy);
        }
    }

}
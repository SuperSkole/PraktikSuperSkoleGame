using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using Scenes._20_MainWorld.Scripts.Car;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes._20_MainWorld.Scripts
{
    public class InColliderZone : MonoBehaviour
    {
        [SerializeField] Interactions parent;
        [SerializeField] private UnityEvent action;
        [SerializeField] private GameObject door;
        [SerializeField] private bool isDoor;
        [SerializeField] private bool isNPC;
        [SerializeField] private bool isCar;
        [SerializeField] private bool isGasSTT;
        //[SerializeField] private NPCInteractions interactions;
        [SerializeField] private int neededLvlToEnter;
        private PlayerEventManager playerEventManager;
        private CarEventsManager carEventsMa;
        private CarEvents carEvents;
        
        private OpenCloseDoor doorMechanism;

        private int playerLvl;

        private void Start()
        {
            // Cache the door component
            if (isDoor && door != null)
            {
                doorMechanism = door.GetComponent<OpenCloseDoor>();
            }
            playerEventManager = PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>();
            playerLvl = PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>().PlayerLanguageLevel;
            //playerLvl = PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>().CurrentLevel;
            // TODO : Remove Try catch at later date
            try
            {
                carEventsMa = GameObject.FindGameObjectWithTag("Car").GetComponent<CarEventsManager>();
                carEvents = GameObject.FindGameObjectWithTag("Car").GetComponent<CarEvents>();
            }
            catch {  }
        }

        /// <summary>
        /// Enter zone for interaction
        /// </summary>
        /// <param name="collision"></param>
        public void OnTriggerEnter(Collider collision)
        {
            playerLvl = PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>().PlayerLanguageLevel;
            if (collision.gameObject.CompareTag("Player") && playerLvl >= neededLvlToEnter)
            {
                //Some Obj dont need a parent to work, a quick failsafe
                try
                {
                    playerEventManager.PlayerInteraction = action;
                    playerEventManager.interactionIcon.SetActive(true);
                    //parent.action = action;
                    parent.inZone = true;
                }
                catch { }

                if (isDoor && doorMechanism != null)
                {
                    try
                    {
                        doorMechanism.OpenDoor();
                        SetLastInteractionPoint(transform);
                    }
                    catch { }
                }
                if (isNPC)
                {
                    //interactions.StartScaling();
                }
                //If the player walksinto a collider with this name active the event
                switch (gameObject.name)
                {
                    case "WalkInto":
                        playerEventManager.InvokeAction();
                        break;
                    case "PlayerCar"://Doesnt Work, Rename to new car if we want it to work
                        playerEventManager.InvokeAction();
                        break;
                    default:
                        //print("InColliderZone/OnTriggerEnter/No name matches");
                        break;
                }
            }
            if (collision.gameObject.CompareTag("Car") && isGasSTT)
            {
                try
                {
                    carEventsMa.CarInteraction = action;
                    carEventsMa.interactionIcon.SetActive(true);
                }
                catch { }
            }
        }

        /// <summary>
        /// Left the zone for interactions disables the interaction bubble.
        /// </summary>
        /// <param name="collision"></param>
        public void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                try
                {
                    playerEventManager.PlayerInteraction = new UnityEvent();
                    playerEventManager.interactionIcon.SetActive(false);

                    //parent.action = null;
                    parent.inZone = false;
                }
                catch { }

                if (isDoor && doorMechanism != null)
                {
                    doorMechanism.CloseDoor();
                }
            }
            if (collision.gameObject.CompareTag("Car"))
            {
                if (isGasSTT)
                {
                    try
                    {
                        carEventsMa.CarInteraction = new UnityEvent();
                        carEventsMa.CarInteraction.AddListener(carEvents.TurnOffCar);
                        carEventsMa.interactionIcon.SetActive(false);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Saves the last interaction point.
        /// </summary>
        /// <param name="interactionPoint">The transform of the interaction point.</param>
        public void SetLastInteractionPoint(Transform interactionPoint)
        {
            //print($"Here is the saved position: {interactionPoint.position}");
            // set next spawn point, add 1 in height so we dont spawn in ground
            PlayerManager.Instance.PlayerData.SetLastInteractionPoint(interactionPoint.position + new Vector3(0, 1, 0));
            var car = GameObject.Find(PlayerManager.Instance.PlayerData.ReturnActiveCarName());
            PlayerManager.Instance.PlayerData.CarPos = car.transform.transform.position;
            PlayerManager.Instance.PlayerData.CarRo = car.transform.transform.rotation;
            PlayerManager.Instance.PlayerData.FuelAmount = car.GetComponent<CarFuelMangent>().FuelAmount;
        }
    }
}

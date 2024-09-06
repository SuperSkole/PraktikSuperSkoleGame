using _99_Legacy.Interaction;
using Scenes._10_PlayerScene.Scripts;
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
        [SerializeField] private NPCInteractions interactions;
        private PlayerEventManager playerEventManager;
        private OpenCloseDoor doorMechanism;

        private void Start()
        {
            // Cache the door component
            if (isDoor && door != null)
            {
                doorMechanism = door.GetComponent<OpenCloseDoor>();
            }
            playerEventManager = PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>();
        }

        /// <summary>
        /// Enter zone for interaction
        /// </summary>
        /// <param name="collision"></param>
        public void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("Player"))
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
                else if (isNPC)
                {
                    interactions.StartScaling();
                }

                switch (gameObject.name)
                {
                    case "WalkInto":
                        playerEventManager.InvokeAction();
                        break;
                    case "PlayerCar":
                        playerEventManager.InvokeAction();
                        break;
                    default:
                        //print("InColliderZone/OnTriggerEnter/No name matches");
                        break;
                }
            }
            if (collision.gameObject.CompareTag("Car"))
            {

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
                    playerEventManager.PlayerInteraction = null;
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
        }
    }
}

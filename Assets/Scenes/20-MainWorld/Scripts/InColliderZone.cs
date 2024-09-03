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

        private OpenCloseDoor doorMechanism;

        private void Start()
        {
            // Cache the door component
            if (isDoor && door != null)
            {
                doorMechanism = door.GetComponent<OpenCloseDoor>();
            }
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
                    PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction = action;
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
                        action.Invoke();
                        break;
                    case "PlayerCar":
                        action.Invoke();
                        break;
                    default:
                        //print("InColliderZone/OnTriggerEnter/No name matches");
                        break;
                }
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
                    PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction = null;

                    //parent.action = null;
                    parent.inZone = false;
                }
                catch { }

                if (isDoor && doorMechanism != null)
                {
                    doorMechanism.CloseDoor();
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
        }
    }
}

using CORE;
using UnityEngine;
using UnityEngine.Events;

public class InColliderZone : MonoBehaviour
{
    [SerializeField] Interactions parent;
    [SerializeField] private UnityEvent action;
    [SerializeField] private GameObject door;
    [SerializeField] private bool isDoor;
    [SerializeField] private bool isNPC;
    [SerializeField] private NPCInteractions interactions;

    /// <summary>
    /// Enter zone for interaction enables the interaction bubble
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Some Obj dont need a parent to work, a quick failsafe
            try { 
                parent.action = action;
                parent.inZone = true;
            }
            catch { }          
            
            if (isDoor)
            {
                try { door.GetComponent<OpenCloseDoor>().OpenDoor(); }
                catch { }
            }
            else if (isNPC)
            {
                interactions.StartScaling();
            }
            if (gameObject.name == "WalkInto")
            {
                action.Invoke();
                //  Debug.Log($"Interactions/OnTriggerEnter/WalkInto/ ChangedScene");
            }
            //Debug.Log($"Interactions/OnTriggerEnter/Obj: {gameObject.name}");

            //collision.gameObject.GetComponent<PlayerWorldMovement>().inBubble.SetActive(true);
            //PlayerWorldMovement.witchObjCloseTo = interactionZoneObj;

        }
    }
    /// <summary>
    /// Left the zone for interactions disables the interaction bubble.
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            try {
                parent.action = null;
                parent.inZone = false;
            }
            catch { }
            
            if (isDoor) { door.GetComponent<OpenCloseDoor>().CloseDoor(); }
            //Debug.Log($"Interactions/OnTriggerExit/Obj: {gameObject.name}");

            //collision.gameObject.GetComponent<PlayerWorldMovement>().inBubble.SetActive(false);
            //PlayerWorldMovement.witchObjCloseTo = null;
        }
    }

    public void SetSaveSpawnPos(Transform transform)
    {
        print($"Here is the saved position: {transform.position}");
        //GameManager.Instance.pla
    }
}

using UnityEngine;

public class Interactions : MonoBehaviour
{
    [SerializeField] private GameObject whichObj;
  
    /// <summary>
    /// Enter zone for interaction enables the interaction bubble
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("EnterInteraction");
            collision.gameObject.GetComponent<PlayerWorldMovement>().inBubble.SetActive(true);
            PlayerWorldMovement.witchObjCloseTo = whichObj;

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
            Debug.Log("LeftInteraction");
            collision.gameObject.GetComponent<PlayerWorldMovement>().inBubble.SetActive(false);
            PlayerWorldMovement.witchObjCloseTo = null;

        }
    }
}

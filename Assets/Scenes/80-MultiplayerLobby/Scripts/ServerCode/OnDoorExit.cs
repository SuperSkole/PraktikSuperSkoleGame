using UnityEngine;

public class OnDoorExit : MonoBehaviour
{
    /// <summary>
    /// Checks if the one colliding is the player and if so, call disconnection for them.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<DisconnectHandler>().PlayerDoorExit();
        }
    }
}

using Scenes._10_PlayerScene.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class IsCarFlipped : MonoBehaviour
{
    [SerializeField] private List<Transform> rayPos = new List<Transform>();
    [SerializeField] private List<bool> wheelTouchingGround = new List<bool>();
    [SerializeField] UnityEvent action;
    CarEvents carEvents;
    private void Awake()
    {
        carEvents = gameObject.GetComponent<CarEvents>();

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < rayPos.Count; i++)
        {
            wheelTouchingGround[i] = IsWheelTouchingGround(rayPos[i]);
            if (wheelTouchingGround.All(b => b == false))
            {
                PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction = null;
                PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction = action;
            }
        }
    }
    public void FlipCarEvent()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        
        PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction = null;
        PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction.AddListener(carEvents.TurnOffCar);
    }
    private bool IsWheelTouchingGround(Transform rayPos)
    {
        Vector3 fwd = rayPos.TransformDirection(Vector3.forward);
        if (Physics.Raycast(rayPos.position, fwd, 1))
        {
            return true;
        }
        else
            return false;
    }
}

using Scenes._10_PlayerScene.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IsCarFlipped : MonoBehaviour
{
    [SerializeField] private List<Transform> rayPos = new List<Transform>();
    [SerializeField] private List<bool> wheelTouchingGround = new List<bool>();
    CarEvents carEvents;
    bool eventSet;
    private void Awake()
    {
        carEvents = gameObject.GetComponent<CarEvents>();

    }
    // Update is called once per frame
    // TODO : Fix So this works with flipping the car
    void FixedUpdate()
    {
        for (int i = 0; i < rayPos.Count; i++)
        {
            wheelTouchingGround[i] = IsWheelTouchingGround(rayPos[i]);
        }
        if (!eventSet && wheelTouchingGround.All(b => b == false))
        {
            try
            {
                PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction.RemoveAllListeners();
            }
            catch { }
            PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction.AddListener(FlipCarEvent);
            eventSet = true;

        }
        if (eventSet && wheelTouchingGround.All(b => b == true))
        {
            PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction.RemoveAllListeners();
            PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction.AddListener(carEvents.TurnOffCar);
        }
    }
    public void FlipCarEvent()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction.RemoveAllListeners();
        PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction.AddListener(carEvents.TurnOffCar);
        eventSet = false;
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

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
    float timer;
    private void Awake()
    {
        carEvents = gameObject.GetComponent<CarEvents>();

    }
    // Update is called once per frame
    // TODO : Fix So this works with flipping the car
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        for (int i = 0; i < rayPos.Count; i++)
        {
            wheelTouchingGround[i] = IsWheelTouchingGround(rayPos[i]);
        }
        if (timer > 2f && !eventSet && wheelTouchingGround.All(b => b == false))
        {
            if (PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction != null)
            {
                PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction.RemoveAllListeners();
            }

            PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction.AddListener(FlipCarEvent);
            eventSet = true;

        }
        if (timer > 2f && eventSet && wheelTouchingGround.All(b => b == true))
        {
            if (PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction != null)
            {
                PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction.RemoveAllListeners();
            }
            PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction.AddListener(carEvents.TurnOffCar);
        }
    }
    public void FlipCarEvent()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        if (PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction != null)
        {
            PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction.RemoveAllListeners();
        }
        PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction.AddListener(carEvents.TurnOffCar);
        eventSet = false;
        timer = 0;
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

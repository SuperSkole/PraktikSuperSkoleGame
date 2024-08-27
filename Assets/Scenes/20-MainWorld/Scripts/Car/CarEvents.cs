using Cinemachine;
using Scenes.PlayerScene.Scripts;
using UnityEngine;

public class CarEvents : MonoBehaviour
{
    [SerializeField] CarMainWorldMovement car;
    private GameObject spawnedPlayer;
    private CinemachineVirtualCamera cam;
    [SerializeField] CarSetPlayerPos carSetPlayerPos;

    private void Start()
    {
        gameObject.GetComponent<CarMainWorldMovement>().enabled = false;
        spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
    }
    public void TurnOnCar()
    {
        gameObject.GetComponent<CarMainWorldMovement>().enabled = true;
        car.CarActive = true;

        cam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        cam.Follow = gameObject.transform;
        cam.LookAt = gameObject.transform;

        spawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction.AddListener(TurnOffCar);
        DisablePlayer();
        carSetPlayerPos.isDriving = true;

    }
    public void TurnOffCar()
    {
        if (carSetPlayerPos.ReturningPlayerPlacement())
        {
            car.CarActive = false;
            gameObject.GetComponent<CarMainWorldMovement>().enabled = false;
            cam.Follow = spawnedPlayer.transform;
            cam.LookAt = spawnedPlayer.transform;
            spawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction.RemoveAllListeners();
            
            var pos = carSetPlayerPos.SetTransformOfPlayer().position;
            pos.y += 1;
            spawnedPlayer.transform.position = pos;
            
            EnablePlayer();
            carSetPlayerPos.isDriving = false;

        }

    }
    /// <summary>
    /// Enables certain componets on the player
    /// </summary>
    private void EnablePlayer()
    {
        spawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = true;
        spawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;
        spawnedPlayer.GetComponentInChildren<MeshRenderer>().enabled = true;
        spawnedPlayer.GetComponent<Rigidbody>().useGravity = true;
    }
    /// <summary>
    /// Disables certain componets on the player
    /// If entier player is disabled we will not have any way for an input
    /// </summary>
    private void DisablePlayer()
    {
        spawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = false;
        spawnedPlayer.GetComponent<CapsuleCollider>().enabled = false;
        spawnedPlayer.GetComponentInChildren<MeshRenderer>().enabled = false;
        spawnedPlayer.GetComponent<Rigidbody>().useGravity = false;
    }
}

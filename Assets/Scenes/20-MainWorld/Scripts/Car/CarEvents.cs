using Cinemachine;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes._20_MainWorld.Scripts.Car
{
    public class CarEvents : MonoBehaviour
    {
        [SerializeField] CarMainWorldMovement car;
        [SerializeField] PrometeoCarController prometeoCarController;
        private GameObject spawnedPlayer;
        private PlayerEventManager playerEvent;
        private CinemachineVirtualCamera cam;
        [SerializeField] CarSetPlayerPos carSetPlayerPos;

        public GameObject CarSmoke1;
        public GameObject CarSmoke2;
        private void Start()
        {
            if (car != null)
            {
                gameObject.GetComponent<CarMainWorldMovement>().enabled = false;
            }
            if (prometeoCarController != null)
            {
                gameObject.GetComponent<PrometeoCarController>().enabled = false;
                gameObject.GetComponent<CarFuelMangent>().enabled = false;
            }
            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
            playerEvent = spawnedPlayer.GetComponent<PlayerEventManager>();
            CarSmoke1.SetActive(false);
            CarSmoke2.SetActive(false);
        }
        public void TurnOnCar()
        {
            if (car != null)
            {
                gameObject.GetComponent<CarMainWorldMovement>().enabled = true;
                car.CarActive = true;
            }
            if (prometeoCarController != null)
            {
                prometeoCarController.enabled = true;
                gameObject.GetComponent<CarFuelMangent>().enabled = true;

            }

            CarSmoke1.SetActive(true);
            CarSmoke2.SetActive(true);
            cam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();

            cam.Follow = gameObject.transform;
            cam.LookAt = gameObject.transform;
            //GetComponent<CarFuel>().gaugeImg.enabled = true;

            playerEvent.IsInCar = true;
            playerEvent.PlayerInteraction.AddListener(TurnOffCar);
            playerEvent.interactionIcon.SetActive(false);

            DisablePlayer();
            carSetPlayerPos.isDriving = true;


        }
        public void TurnOffCar()
        {
            if (carSetPlayerPos.ReturningPlayerPlacement())
            {
                if (car != null)
                {
                    car.CarActive = false;
                    car.ApplyBrakingToStop();
                    gameObject.GetComponent<CarMainWorldMovement>().enabled = false;
                }
                if (prometeoCarController != null)
                {
                    prometeoCarController.enabled = false;
                    gameObject.GetComponent<CarFuelMangent>().enabled = false;
                }
                
                CarSmoke1.SetActive(false);
                CarSmoke2.SetActive(false);

                cam.Follow = spawnedPlayer.transform;
                cam.LookAt = spawnedPlayer.transform;
                //GetComponent<CarFuel>().gaugeImg.enabled = false;

                playerEvent.IsInCar = false;
                playerEvent.PlayerInteraction.RemoveAllListeners();

                var pos = carSetPlayerPos.SetTransformOfPlayer().position;
                pos.y += 1;
                spawnedPlayer.transform.position = pos;

                EnablePlayer();
                carSetPlayerPos.isDriving = false;
                PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerEventManager>().PlayerInteraction = new UnityEvent();

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
}

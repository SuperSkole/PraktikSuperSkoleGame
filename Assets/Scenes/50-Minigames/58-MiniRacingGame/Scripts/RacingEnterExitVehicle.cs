using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    public class EnterExitVehicle : MonoBehaviour
    {
        public GameObject car;    // Assign your car GameObject in the Inspector
        private CarController carController;
        public UnityEvent enterCar;

        [SerializeField] private CinemachineVirtualCamera mainCamera;

        /// <summary>
        /// Checks the scene id to know if the car should be activated.
        /// </summary>
        private void Start()
        {
            carController = car.GetComponent<CarController>();
            CarActive();
        }

        // private void ToggleCarAndPlayer()
        // {
        //
        //     carController.CarActive = !carController.CarActive;
        //     player.SetActive(!carController.CarActive); // Disable player if car is active, and vice versa
        //
        //     // Position the player next to the car when exiting
        //     if (!carController.CarActive)
        //     {
        //         CamChange obj = camera.GetComponent<CamChange>();
        //         UnityEvent newEvent = new UnityEvent();
        //         player.transform.position = car.transform.position - car.transform.right + 0.5f * Vector3.up; //0.5 for at kompensere for spillerens h�jde
        //         exitCar.AddListener(() => obj.ChangeCamToPlayer(player.transform));
        //         exitCar.Invoke();
        //     }
        //
        //     if (carController.CarActive)
        //     {
        //         enterCar.Invoke();
        //     }
        // }
        //
        // private void PlayerActive()
        // {
        //     carController.CarActive = false;
        //     player.SetActive(true); // Enable player and disable car
        //
        //     // Position the player next to the car when exiting
        //     CamChange obj = camera.GetComponent<CamChange>();
        //     UnityEvent newEvent = new UnityEvent();
        //     player.transform.position = car.transform.position - car.transform.right + 0.5f * Vector3.up; //0.5 to compensate for the player's height
        //     exitCar.AddListener(() => obj.ChangeCamToPlayer(player.transform));
        //     exitCar.Invoke();
        // }

        /// <summary>
        /// Activate the car.
        /// </summary>
        private void CarActive()
        {
            carController.CarActive = true;

            enterCar.Invoke();  
        }
    }
}

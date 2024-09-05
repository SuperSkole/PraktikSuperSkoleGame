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
            enterCar.Invoke();
        }
    }
}

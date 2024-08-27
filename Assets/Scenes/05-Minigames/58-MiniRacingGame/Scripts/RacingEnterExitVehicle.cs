using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes.Minigames.MiniRacingGame.Scripts
{
    public class EnterExitVehicle : MonoBehaviour
    {
        public GameObject player; // Assign your player GameObject in the Inspector
        public GameObject car;    // Assign your car GameObject in the Inspector
        //public GameObject racingGameManager;
        private bool isPlayerNearby = false;
        private CarController carController;
        //private RacingGameCore racingGameCore;
        //[SerializeField] private GameObject camera;
        public UnityEvent enterCar;
        public UnityEvent exitCar;
        int sceneID = 0;

        [SerializeField] private CinemachineVirtualCamera mainCamera;

        /// <summary>
        /// Checks the scene id to know if the car should be activated.
        /// </summary>
        private void Start()
        {
            sceneID = SceneManagerScript.Instance.SceneID;

            carController = car.GetComponent<CarController>();
            if (sceneID == 0) //Town scene
            {
                //PlayerActive();
                //CarActive();
            }

            if (sceneID == 13) //Racing scene
            {
                CarActive();
                //PlayerActive();
            }
        }

        /// <summary>
        /// Check if the player is trying to enter or exit the car.
        /// </summary>
        private void Update()
        {
            if (isPlayerNearby && Input.GetKeyDown(KeyCode.E)) //Town scene
            {
                //ToggleCarAndPlayer();
            }

            //if (sceneID == 0 && isPlayerNearby && Input.GetKeyDown(KeyCode.E)) //Town scene
            //{
            //    ToggleCarAndPlayer();
            //}
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
        //         player.transform.position = car.transform.position - car.transform.right + 0.5f * Vector3.up; //0.5 for at kompensere for spillerens højde
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
            player.SetActive(false); // Disable player and enable car

            enterCar.Invoke();  
        }

        /// <summary>
        /// Fetch the player.
        /// </summary>
        public void FindPlayer()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        /// <summary>
        /// Check if the player is colliding with it.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player)
            {
                isPlayerNearby = true;
            }
        }

        /// <summary>
        /// Checks if the player is leaving the car.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == player)
            {
                isPlayerNearby = false;
            }
        }
    }
}

using Cinemachine;
using Scenes._10_PlayerScene.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scenes._20_MainWorld.Scripts.Car
{
    public class CarEvents : MonoBehaviour
    {
        [SerializeField] CarMainWorldMovement car;
        [SerializeField] PrometeoCarController prometeoCarController;
        private GameObject spawnedPlayer;
        private CarEventsManager carEventsMa;
        private CinemachineVirtualCamera cam;
        [SerializeField] CarSetPlayerPos carSetPlayerPos;
        [SerializeField] GameObject carSpeedGo;
        [SerializeField] GameObject carFuelBarGo;
        [SerializeField] GameObject SoundParent;
        [SerializeField] GameObject callCarButton;

        public List<GameObject> Exhaustsmoke;
        private void Start()
        {
            if (car != null)
            {
                gameObject.GetComponent<CarMainWorldMovement>().enabled = false;
            }
            if (prometeoCarController != null)
            {
                GetComponent<PrometeoCarController>().carSpeedText = GameObject.Find("CarSpeedTxt").GetComponent<Text>();
                GetComponent<PrometeoCarController>().enabled = false;
                GetComponent<CarFuelMangent>().fuelGauge = GameObject.Find("FuelBar").transform.Find("background").GetComponentInChildren<Image>();
                //GetComponent<CarFuelMangent>().enabled = false;
                GetComponent<CarSaveTPPoint>().enabled = false;
                GetComponent<IsCarFlipped>().enabled = false;
                carSpeedGo = GameObject.Find("CarSpeedTxt");
                carFuelBarGo = GameObject.Find("FuelBar");
                callCarButton = GameObject.Find("CallCarButton");

                carSpeedGo.SetActive(false);
                carFuelBarGo.SetActive(false);

                SoundParent.SetActive(false);
            }
            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
            carEventsMa = GetComponent<CarEventsManager>();
            foreach (GameObject item in Exhaustsmoke)
            {
                item.SetActive(false);
            }
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
               // prometeoCarController.SetEnabledValue(true);
                gameObject.GetComponent<CarFuelMangent>().enabled = true;
                GetComponent<IsCarFlipped>().enabled = true;
                GetComponent<CarSaveTPPoint>().enabled = true;
                carSpeedGo.SetActive(true);
                carFuelBarGo.SetActive(true);
                SoundParent.SetActive(true);
                callCarButton.SetActive(false);
                carEventsMa.enabled = true;

            }
            foreach (GameObject item in Exhaustsmoke)
            {
                item.SetActive(true);
            }
            cam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();

            cam.Follow = gameObject.transform;
            cam.LookAt = gameObject.transform;
            //GetComponent<CarFuel>().gaugeImg.enabled = true;

            carEventsMa.IsInCar = true;
            carEventsMa.CarInteraction.AddListener(TurnOffCar);
            carEventsMa.interactionIcon.SetActive(false);


            //playerEvent.IsInCar = true;
            //playerEvent.PlayerInteraction.AddListener(TurnOffCar);
            //playerEvent.interactionIcon.SetActive(false);

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
                    //GetComponent<Rigidbody>().velocity = Vector3.zero;
                    //GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    //prometeoCarController.SetEnabledValue(false);
                    prometeoCarController.Brakes();
                    prometeoCarController.enabled = false;
                    //GetComponent<CarFuelMangent>().enabled = false;
                    GetComponent<IsCarFlipped>().enabled = false;
                    GetComponent<CarSaveTPPoint>().enabled = false;
                    carSpeedGo.SetActive(false);
                    carFuelBarGo.SetActive(false);
                    SoundParent.SetActive(false);
                    callCarButton.SetActive(true);
                    carEventsMa.enabled = false;
                }
                foreach (GameObject item in Exhaustsmoke)
                {
                    item.SetActive(false);
                }

                cam.Follow = spawnedPlayer.transform;
                cam.LookAt = spawnedPlayer.transform;
                //GetComponent<CarFuel>().gaugeImg.enabled = false;

                carEventsMa.IsInCar = false;
                carEventsMa.CarInteraction.RemoveAllListeners();


                var pos = carSetPlayerPos.SetTransformOfPlayer().position;
                pos.y += 1;
                spawnedPlayer.GetComponent<Rigidbody>().position = pos;
                spawnedPlayer.transform.position = pos;

                EnablePlayer();
                carSetPlayerPos.isDriving = false;

                carEventsMa.CarInteraction = new UnityEvent();

            }

        }
        /// <summary>
        /// Enables certain componets on the player
        /// </summary>
        private void EnablePlayer()
        {
            spawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = true;
            spawnedPlayer.GetComponent<PlayerEventManager>().enabled = true;
            spawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;
            foreach (var playerMeshRenderer in spawnedPlayer.GetComponentsInChildren<MeshRenderer>())
            {
                playerMeshRenderer.enabled = true;
            }
            spawnedPlayer.GetComponent<Rigidbody>().useGravity = true;
        }
        /// <summary>
        /// Disables certain componets on the player
        /// If entire player is disabled we will not have any way for an input
        /// </summary>
        private void DisablePlayer()
        {
            spawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = false;
            spawnedPlayer.GetComponent<PlayerEventManager>().enabled = false;
            spawnedPlayer.GetComponent<CapsuleCollider>().enabled = false;
            foreach (var playerMeshRenderer in spawnedPlayer.GetComponentsInChildren<MeshRenderer>())
            {
                playerMeshRenderer.enabled = false;
            }
            spawnedPlayer.GetComponent<Rigidbody>().useGravity = false;
        }
    }
}

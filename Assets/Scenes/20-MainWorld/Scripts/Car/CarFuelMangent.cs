using UnityEngine;
using UnityEngine.UI;

namespace Scenes._20_MainWorld.Scripts.Car
{
    public class CarFuelMangent : MonoBehaviour
    {
        [SerializeField] private PrometeoCarController controller;
        private float speed;
        private float fuelConsumptionRate = 0.001f;
        [SerializeField] private float fuelAmount;
        public float FuelAmount { get { return fuelAmount; } 
                                  set { fuelAmount = value; } }
        public Image fuelGauge;

        private void Awake()
        {
            controller = GetComponent<PrometeoCarController>();
            FuelAmount = 1.0f;
        }
        private float time;

        private void Update()
        {
            time += Time.deltaTime;
            // Reduce fuel based on the car's speed
            if (time > 0.1f && IsThereFuelLeft())
            {
                UpdateFuel();
                time = 0.0f;
            }
            else if(!IsThereFuelLeft())
            {
                controller.ThrottleOff();
                controller.Brakes();
                controller.enabled = false;
            }
        }

        private void UpdateFuel()
        {
            speed = controller.carSpeed;
            // Calculate fuel consumption based on speed
            FuelAmount -= speed * fuelConsumptionRate * Time.deltaTime;

            // Clamp the fuel to ensure it doesn't go below 0
            FuelAmount = Mathf.Clamp(FuelAmount, 0, 1.0f);

            fuelGauge.fillAmount = fuelAmount;

        }

        private bool IsThereFuelLeft()
        {
            if (FuelAmount > 0)
            {
                return true;
            }
            return false;
        }
    }
}

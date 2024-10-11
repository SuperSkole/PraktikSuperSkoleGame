using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes._20_MainWorld.Scripts.Car
{
    public class CarFuelMangent : MonoBehaviour
    {
        [SerializeField] private PrometeoCarController carController;
        private float speed;
        private float fuelConsumptionRate = 0.001f;
        [SerializeField] private float fuelAmount;
        [Range(1, 3)]
        public float FuelUsageMultiplier;
        public float FuelAmount
        {
            get { return fuelAmount; }
            set { fuelAmount = value; }
        }
        public Image fuelGauge;
        private void OnEnable()
        {
            FuelAmount = PlayerManager.Instance.PlayerData.FuelAmount;
        }
        private void Awake()
        {
            carController = GetComponent<PrometeoCarController>();
            FuelAmount = PlayerManager.Instance.PlayerData.FuelAmount;
        }
        private float time;

        private void Update()
        {
            if (carController.enabled)
            {
                time += Time.deltaTime;
                // Reduce fuel based on the car's speed
                if (time > 0.1f && IsThereFuelLeft())
                {
                    UpdateFuel();
                    carController.SetEnabledValue(true);
                    time = 0.0f;
                }
                else if (!IsThereFuelLeft())
                {
                    carController.NoFuelLeftDisableCar();
                }
            }
        }

        private void UpdateFuel()
        {
            // Use the absolute value of speed to ensure fuel is consumed when reversing
            float absoluteSpeed = Mathf.Abs(carController.carSpeed);

            // Calculate fuel consumption based on the absolute speed
            FuelAmount -= absoluteSpeed * (fuelConsumptionRate * FuelUsageMultiplier) * Time.deltaTime;

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

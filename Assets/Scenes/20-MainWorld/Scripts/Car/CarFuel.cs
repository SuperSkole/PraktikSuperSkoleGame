using UnityEngine;
using UnityEngine.UI;

namespace Scenes._20_MainWorld.Scripts.Car
{
    public class CarFuel : MonoBehaviour
    {
        [SerializeField] GameObject FuelGaugeParent;
        public Image gaugeImg;
        public float fuelAmount { get; set; }

        private void Awake()
        {
            fuelAmount = 1.0f;
            FuelGaugeParent.SetActive(true);
        }
        private void Update()
        { 
            gaugeImg.fillAmount = fuelAmount;
        }



    


    }
}

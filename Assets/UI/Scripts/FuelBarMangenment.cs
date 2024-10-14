using Scenes._20_MainWorld.Scripts.Car;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Only purpose is so that if the player opens the fuel station the fuel bar doesnt show if the player is not in the car
/// </summary>
public class FuelBarMangenment : MonoBehaviour
{
    [SerializeField] private Image fillImg;
    private void OnEnable()
    {
        try//try so we dont get error
        {
            var spawnedCarController = GameObject.FindGameObjectWithTag("Car").GetComponent<PrometeoCarController>();
            var spawnedCarFuel = GameObject.FindGameObjectWithTag("Car").GetComponent<CarFuelMangent>();
            if (spawnedCarController.enabled)
            {
                fillImg.fillAmount = spawnedCarFuel.FuelAmount;
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        catch { }
    }
}

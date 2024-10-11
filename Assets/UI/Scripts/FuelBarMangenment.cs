using UnityEngine;

/// <summary>
/// Only purpose is so that if the player opens the fuel station the fuel bar doesnt show if the player is not in the car
/// </summary>
public class FuelBarMangenment : MonoBehaviour
{
    private void OnEnable()
    {
        try//try so we dont get error
        {
            var spawnedCar = GameObject.FindGameObjectWithTag("Car").GetComponent<PrometeoCarController>();
            if (spawnedCar.enabled)
            {
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

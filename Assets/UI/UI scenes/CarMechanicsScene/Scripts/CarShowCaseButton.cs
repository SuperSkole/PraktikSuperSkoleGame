using UnityEngine;

public class CarShowCaseButton : MonoBehaviour
{
    public GameObject car;
    public string nameOfCar;
    public int price;
    public bool Bought;

    public CarShowCaseButton(GameObject car, string nameOfCar)
    {
        this.car = car;
        this.nameOfCar = nameOfCar;
    }

}

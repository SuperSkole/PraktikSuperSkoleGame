using UnityEngine;

public class CarShowCaseButton : MonoBehaviour
{
    public GameObject car;
    public string nameOfCar;
    public int price;
    public bool Bought;

    // Stats for InfoPanel
    [Range(20, 130)]
    public int MaxSpeed;
    [Range(1, 10)]
    public int Acacceleration;
    [Range(1, 10)]
    public int DriftStats;
    [Range(1, 3)]
    public float FuelUsageMultiplier;




}

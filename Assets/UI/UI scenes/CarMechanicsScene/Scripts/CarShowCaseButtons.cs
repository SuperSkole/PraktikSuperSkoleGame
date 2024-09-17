using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarShowCaseButtons : MonoBehaviour
{
    public Material material;
    public string nameOfMaterial;
    public int price;
    public bool Bought;

    public CarShowCaseButtons(Material material, string nameOfMaterial)
    {
        this.material = material;
        this.nameOfMaterial = nameOfMaterial;
    }
}

using System.Collections.Generic;

[System.Serializable]
public class CarInfo
{
    /// <summary>
    /// Name of the car
    /// </summary>
    public string Name;
    public string MaterialName;//Name of the material
    public bool IsActive;
    /// <summary>
    /// List of Materials that have been bought
    /// </summary>
    public List<MaterialInfo> materialList;

    public CarInfo(string name, string materialName, bool isActive, List<MaterialInfo> materialList)
    {
        Name = name;
        MaterialName = materialName;
        IsActive = isActive;
        this.materialList = materialList;
    }
}

[System.Serializable]
public class MaterialInfo   
{
    public bool Bought;
    public string nameOfMaterial;

    public MaterialInfo(bool bought, string nameOfMaterial)
    {
        Bought = bought;
        this.nameOfMaterial = nameOfMaterial;
    }
}

using System.Collections.Generic;

[System.Serializable]
public class CarInfo
{
    public string Name;
    public string MaterialName;
    public bool IsActive;
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

using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using Scenes._50_Minigames._56_WordFactory.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class SetUpPlayerCar : MonoBehaviour
{
    public Dictionary<string, List<CarMaterialnfo>> MaterialDic = new();
    public Dictionary<string, GameObject> CarGODic = new();

    public List<SpeceficCarMaterialnfo> CarListMaterials;
    public List<CarGameObjectInfo> CarListGameObjects;

    private Vector3 CarSpawnPos;
    PlayerData playerData;
    public GameObject spawnedCar;
    private string nameOfSpawnedCar;

    [SerializeField] private GameObject carFuelMeterUI;
    [SerializeField] private GameObject carSpeedTextUI;


    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>();
        populateDics();
        CarSpawnPos = playerData.CarPos;
        foreach (var item in playerData.ListOfCars)
        {
            if (item.IsActive)
            {
                // TODO : Change "Quaternion.identity" to the actual saved rotation
                nameOfSpawnedCar = item.Name;
                spawnedCar = Instantiate(ReturnThePlayerCar(item.Name), CarSpawnPos, Quaternion.identity);
                PreviewColorOfCar(item.Name);
                break;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">Name of the Car</param>
    public void PreviewColorOfCar(string name)
    {
        var previewMaterial = ReturnTheRightMaterial(name);
        Transform carBodyTransform = spawnedCar.transform.Find("Body");

        if (carBodyTransform != null)
        {
            Renderer carRenderer = carBodyTransform.GetComponent<Renderer>();

            if (carRenderer != null)
            {
                // Set the new material
                carRenderer.material = previewMaterial.CarMaterial;
                if (spawnedCar.name == "VanCar(Clone)")
                {
                    spawnedCar.transform.Find("BackDoor (0)").GetComponent<Renderer>().material = previewMaterial.CarMaterial;
                    spawnedCar.transform.Find("BackDoor (1)").GetComponent<Renderer>().material = previewMaterial.CarMaterial;
                }
            }
            else
            {
                Debug.LogWarning("Renderer component not found on the car's body.");
            }
        }
        else
        {
            Debug.LogWarning("Car body child object not found.");
        }
    }
    private void populateDics()
    {
        foreach (var car in CarListMaterials)
        {
            MaterialDic.Add(car.CarName, car.materialInfo);
        }
        foreach (var car in CarListGameObjects)
        {

            CarGODic.Add(car.CarName, car.CarGameObject);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">Name of the car</param>
    /// <returns></returns>
    private CarMaterialnfo ReturnTheRightMaterial(string value)
    {
        if (MaterialDic.TryGetValue(value, out List<CarMaterialnfo> materialInfo))
        {
            foreach (var item in materialInfo)
            {
                if (playerData.ListOfCars[ReturnsIndexOfCar(value)].MaterialName == item.MaterialName)
                {
                    return item;
                }
            }
        }
        return null;
    }
    private GameObject ReturnThePlayerCar(string value)
    {
        return CarGODic[value];
    }
    private int ReturnsIndexOfCar(string name)
    {
        for (int i = 0; i < playerData.ListOfCars.Count; i++)
        {
            if (playerData.ListOfCars[i].Name == name)
            {
                return i;
            }
        }
        //No car found
        return 0;
    }
}
[System.Serializable]
public class SpeceficCarMaterialnfo
{
    public string CarName;
    public List<CarMaterialnfo> materialInfo;
}
[System.Serializable]
public class CarMaterialnfo
{
    public string MaterialName;
    public Material CarMaterial;
}

[System.Serializable]
public class CarGameObjectInfo
{
    [SerializeField] public string CarName;
    [SerializeField] public GameObject CarGameObject;
}

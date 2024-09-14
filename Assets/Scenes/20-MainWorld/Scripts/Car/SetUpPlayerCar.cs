using Cinemachine.Utility;
using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SetUpPlayerCar : MonoBehaviour
{
    public Dictionary<string, Material> MaterialDic = new Dictionary<string, Material>();
    public Dictionary<string, GameObject> CarGODic = new Dictionary<string, GameObject>();

    public List<CarMaterialInfo> CarListMaterials;
    public List<CarGameObjectInfo> CarListGameObjects;

    private Vector3 CarSpawnPos;
    PlayerData playerData;
    GameObject spawnedCar;

    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>();
        populateDics();
        CarSpawnPos = playerData.CarPos;
        foreach (var item in playerData.listOfCars)
        {
            if (item.IsActive)
            {
                // TODO : Change "Quaternion.identity" to the actual saved rotation
                spawnedCar = Instantiate(ReturnThePlayerCar(item.Name), CarSpawnPos, Quaternion.identity);
                PreviewColorOfCar(item.MaterialName);
                break;
            }
        }
    }
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
                carRenderer.material = previewMaterial;
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
            MaterialDic.Add(car.CarName, car.CarMaterial);
        }
        foreach (var car in CarListGameObjects)
        {

            CarGODic.Add(car.CarName, car.CarGameObject);
        }
    }

    private Material ReturnTheRightMaterial(string value)
    {
        return MaterialDic[value];
    }
    private GameObject ReturnThePlayerCar(string value)
    {
        return CarGODic[value];
    }
}
[System.Serializable]
public class CarMaterialInfo
{
    public string CarName;
    public Material CarMaterial;
}

[System.Serializable]
public class CarGameObjectInfo
{
    [SerializeField] public string CarName;
    [SerializeField] public GameObject CarGameObject;
}

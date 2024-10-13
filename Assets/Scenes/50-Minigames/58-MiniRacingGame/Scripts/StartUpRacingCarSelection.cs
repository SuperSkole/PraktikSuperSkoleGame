using Cinemachine;
using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartUpRacingCarSelection : MonoBehaviour
{
    public Dictionary<string, List<CarMaterialnfo>> MaterialDic = new();
    public Dictionary<string, GameObject> CarGODic = new();

    public List<SpeceficCarMaterialnfo> CarListMaterials;
    public List<CarGameObjectInfo> CarListGameObjects;

    PlayerData playerData;
    [SerializeField] private Transform CarSpawnPos;
    public GameObject spawnedCar;

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] Text speedTxt;


    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>();
        populateDics();
        foreach (var item in playerData.ListOfCars)
        {
            if (item.IsActive)
            {
                spawnedCar = Instantiate(ReturnThePlayerCar(item.Name), CarSpawnPos);
                PreviewColorOfCar(item.Name);
                break;
            }
        }
        
        virtualCamera.Follow = spawnedCar.transform;
        virtualCamera.LookAt = spawnedCar.transform;

        spawnedCar.GetComponent<PrometeoCarController>().carSpeedText = speedTxt;

    }
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
    private GameObject ReturnThePlayerCar(string value)
    {
        return CarGODic[value];
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
    // Update is called once per frame
    void Update()
    {
        
    }
}

using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarShowCaseRoomManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> ShowcasedCar = new List<GameObject>();
    [SerializeField] private List<SpeceficCarMaterialnfo> CarListMaterials;
    [SerializeField] private Transform ShowcasedSpawnPoint;
    private GameObject spawnedCar;
    [SerializeField] private float rotationSpeed = 20f;  // Adjust speed here
    string clickedMaterialName = string.Empty;


    private PlayerData playerData;
    [SerializeField] private TextMeshProUGUI LettersTxt;
    private int lettersCount;
    [SerializeField] private Sprite buyImg;
    [SerializeField] private Sprite equipImg;
    [SerializeField] private Sprite haveEquipImg;
    [SerializeField] private Image imgHolder;
    [SerializeField] private Image priceHolder;
    [SerializeField] TextMeshProUGUI price;

    private CarColorShowCaseButtons buttonInstance;
    private string clickedButtonName;

    [SerializeField] GameObject colorOptionsPrefab;
    [SerializeField] GameObject colorOptionsParent;
    private string carNameFromList;


    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>();

        foreach (var item in playerData.listOfCars)
        {
            if (item.IsActive)
            {
                carNameFromList = item.Name;

                var tmp = CarListMaterials.Find(car => car.CarName == item.Name);
                for (int i = 0; i < tmp.materialInfo.Count; i++)
                {
                    InstantiateColorBoxes(tmp.materialInfo[i], i);
                }

                SpawnCar(ReturnThePlayerCar(item.Name));
                PreviewColorOfCar(new CarColorShowCaseButtons(ReturnTheRightMaterial(item.MaterialName), item.MaterialName));
                StartCoroutine(StartRotationOfCar());
                break;
            }
        }
        UpdateValues();


        //SpawnCar(ShowcasedCar[0]);
        //StartCoroutine(StartRotationOfCar());
    }

    private void InstantiateColorBoxes(CarMaterialnfo info, int index)
    {
        //var spawnedObj = Instantiate(colorOptionsPrefab, colorOptionsParent).GetComponent<CarShowCaseButtons>();

        // Instantiate the prefab and get the component
        var spawnedObj = Instantiate(colorOptionsPrefab, colorOptionsParent.transform);
        var showcaseButtons = spawnedObj.GetComponent<CarColorShowCaseButtons>();
        showcaseButtons.material = info.CarMaterial;
        showcaseButtons.nameOfMaterial = info.MaterialName;
        spawnedObj.GetComponentInChildren<TextMeshProUGUI>().text = info.MaterialName;

        // Set the object's name to a custom name without the "(Clone)" suffix
        spawnedObj.gameObject.name = $"ColorButton ({index})";
    }
    private void UpdateValues()
    {
        lettersCount = playerData.CollectedLetters.Count;
        LettersTxt.text = lettersCount.ToString();

        List<CarColorShowCaseButtons> colorButtons = new();
        for (int i = 0; i < 6; i++)
        {
            CarColorShowCaseButtons button = GameObject.Find($"ColorButton ({i})").GetComponent<CarColorShowCaseButtons>();
            if (button != null)
            {
                colorButtons.Add(button);
            }
        }
        foreach (CarInfo car in playerData.listOfCars)
        {
            // Loop through each car's materials
            foreach (MaterialInfo material in car.materialList)
            {
                // Find the corresponding button for this material
                foreach (CarColorShowCaseButtons button in colorButtons)
                {
                    if (button.nameOfMaterial == material.nameOfMaterial)
                    {
                        // Set the 'Bought' status of the button
                        button.Bought = material.Bought;
                    }
                }
            }
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">Car from list name</param>
    /// <returns></returns>
    private GameObject ReturnThePlayerCar(string value)
    {
        foreach (var item in ShowcasedCar)
        {
            if (value == item.name)
            {
                return item;
            }
        }
        return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">Material Name</param>
    /// <returns></returns>
    private Material ReturnTheRightMaterial(string value)
    {
        foreach (var item in CarListMaterials)
        {
            foreach (var info in item.materialInfo)
            {
                if (value == info.MaterialName)
                {
                    return info.CarMaterial;
                }
            }
        }
        return null;
    }


    public void PreviewColorOfCar(CarColorShowCaseButtons previewMaterial)
    {
        Transform carBodyTransform = spawnedCar.transform.Find("Body");

        if (carBodyTransform != null)
        {
            Renderer carRenderer = carBodyTransform.GetComponent<Renderer>();

            if (carRenderer != null)
            {
                // Set the new material
                carRenderer.material = previewMaterial.material;
                clickedMaterialName = previewMaterial.nameOfMaterial;
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
    
    public void PreviewCar(GameObject car)
    {
        Destroy(spawnedCar);
        SpawnCar(car);
    }

    public void SettingButtonsUp(CarColorShowCaseButtons button)
    {
        buttonInstance = button;
        if (button.Bought)
        {
            priceHolder.enabled = false;
            price.enabled = false;
            imgHolder.sprite = equipImg;
        }
        else if (!button.Bought)
        {
            priceHolder.enabled = true;
            price.enabled = true;
            imgHolder.sprite = buyImg;
            price.text = button.price.ToString();
        }
    }


    public void SaveMaterialName()
    {
        if (buttonInstance.Bought)
        {
            SaveCarMaterialNameData(FindIndexInCarMaList());
        }
        else
        {
            //Buying a color
            if (buttonInstance.price <= lettersCount)
            {
                RemoveLetters(buttonInstance.price);
                var tmp = GameObject.Find(clickedButtonName);
                tmp.GetComponent<CarColorShowCaseButtons>().Bought = true;
                SaveCarMaterialNameData(FindIndexInCarMaList());
                AddNewMaterialToCarList(FindIndexInCarMaList());

                priceHolder.enabled = false;
                price.enabled = false;
                imgHolder.sprite = equipImg;

                UpdateValues();
            }
            else
            {
                print("Can't afford the color");
            }
        }
    }
    private int FindIndexInCarMaList()
    {
        int savedIndex = 0;
        for (int i = 0; i < playerData.listOfCars.Count; i++)
        {
            if (carNameFromList == playerData.listOfCars[i].Name)
            {
                savedIndex = i;
                break;
            }
        }

        return savedIndex;
    }
    private void SaveCarMaterialNameData(int indexer)
    {
        playerData.listOfCars[indexer].MaterialName = clickedMaterialName;
    }
    private void AddNewMaterialToCarList(int indexer)
    {
        playerData.listOfCars[indexer].materialList.Add(new MaterialInfo(buttonInstance.Bought, buttonInstance.nameOfMaterial));
    }
    public void SetButtonName(GameObject gO) => clickedButtonName = gO.name;
    private void RemoveLetters(int amountTimes)
    {
        for (int i = 0; i < amountTimes; i++)
        {
            playerData.CollectedLetters.RemoveAt(0);
        }
    }

    private void SpawnCar(GameObject ActiveCar)
    {
        spawnedCar = Instantiate(ActiveCar, ShowcasedSpawnPoint);
    }


    private IEnumerator StartRotationOfCar()
    {
        while (true)
        {
            // Rotate smoothly at a constant speed around the Y-axis
            ShowcasedSpawnPoint.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            yield return null;  // Yield until the next frame
        }
    }
}

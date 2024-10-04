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

    private CarColorShowCaseButtons colorButtonInstance;
    private CarShowCaseButton carButtonInstance;
    private string clickedButtonName;

    [SerializeField] GameObject colorOptionsPrefab;
    [SerializeField] GameObject colorOptionsParent;
    private string carNameFromList;
    private bool isColor;

    private List<GameObject> colorBoxsList = new();

    [SerializeField] private GameObject colorsTab;
    [SerializeField] private GameObject carsTabs;

    [Header("Car Info")]
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider accSlider;
    [SerializeField] private Slider driftSlider;
    [SerializeField] private Slider fuelSlider;

    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>();

        foreach (var item in playerData.ListOfCars)
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

        var backgroundImage = spawnedObj.transform.Find("BucketBackground").GetComponent<Image>();

        ColorContainer colorDic = new ColorContainer();

        try
        {
            backgroundImage.color = colorDic.ReturnColorFromString(info.MaterialName, out Color color);
        }
        catch
        {
            Debug.LogError($"Invalid color string: {info.MaterialName}. Defaulting to white.");
            backgroundImage.color = Color.white;
        }

        //spawnedObj.GetComponentInChildren<TextMeshProUGUI>().text = info.MaterialName;

        // Set the object's name to a custom name without the "(Clone)" suffix
        spawnedObj.gameObject.name = $"ColorButton ({index})";

        colorBoxsList.Add(spawnedObj);
    }
    private void UpdateValues()
    {
        lettersCount = playerData.CollectedLetters.Count;
        LettersTxt.text = lettersCount.ToString();

        List<CarColorShowCaseButtons> colorButtons = new();
        for (int i = 0; i < ReturnAmountInMaterialList(); i++)
        {
            CarColorShowCaseButtons button = GameObject.Find($"ColorButton ({i})").GetComponent<CarColorShowCaseButtons>();
            if (button != null)
            {
                colorButtons.Add(button);
            }
        }
        foreach (CarInfo car in playerData.ListOfCars)
        {
            if (car.IsActive)
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
    }
    private int ReturnAmountInMaterialList()
    {
        foreach (var item in CarListMaterials)
        {
            if (item.CarName == carNameFromList)
            {
                return item.materialInfo.Count;
            }
        }
        return 0;
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
                if (spawnedCar.name == "Van(Clone)")
                {
                    spawnedCar.transform.Find("BackDoor (0)").GetComponent<Renderer>().material = previewMaterial.material;
                    spawnedCar.transform.Find("BackDoor (1)").GetComponent<Renderer>().material = previewMaterial.material;
                }
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
        isColor = true;
        colorButtonInstance = button;
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
    public void SettingCarButtonsUp(CarShowCaseButton button)
    {
        isColor = false;
        carButtonInstance = button;
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
        //spawnedCar.GetComponent<CarShowCaseButton>().
        speedSlider.value = carButtonInstance.MaxSpeed;
        accSlider.value = carButtonInstance.Acacceleration;
        driftSlider.value = carButtonInstance.DriftStats;
        fuelSlider.value = carButtonInstance.FuelUsageMultiplier;
    }


    public void SaveMaterialName()
    {
        if (isColor)
        {
            SaveColorButton();
        }
        else
        {
            SaveCarButton();
        }
    }
    private void SaveColorButton()
    {
        if (colorButtonInstance.Bought)
        {
            SaveCarMaterialNameData(FindIndexInCarMaList());
        }
        else //Buying a color
        {

            if (colorButtonInstance.price <= lettersCount)
            {
                RemoveLetters(colorButtonInstance.price);
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
    private void SaveCarButton()
    {
        if (carButtonInstance.Bought)
        {
            SaveActiveCar();
        }
        else//This Buys the car
        {
            if (carButtonInstance.price <= lettersCount)
            {
                RemoveLetters(carButtonInstance.price);
                var tmp = GameObject.Find(clickedButtonName);
                tmp.GetComponent<CarShowCaseButton>().Bought = true;
                CarInfo carInfo = new CarInfo("", "", false, null);
                //Add to this for each car thats avaiable for purchase 
                switch (tmp.GetComponent<CarShowCaseButton>().nameOfCar)
                {
                    case "Mustang":
                        carInfo = new CarInfo(tmp.GetComponent<CarShowCaseButton>().nameOfCar,
                        "Red", true,
                        new List<MaterialInfo> { new MaterialInfo(true, "Red") });
                        break;
                    case "Van":
                        carInfo = new CarInfo(tmp.GetComponent<CarShowCaseButton>().nameOfCar,
                        "Gray", true,
                        new List<MaterialInfo> { new MaterialInfo(true, "Gray") });
                        break;
                    case "Police":
                        carInfo = new CarInfo(tmp.GetComponent<CarShowCaseButton>().nameOfCar,
                        "White", true,
                        new List<MaterialInfo> { new MaterialInfo(true, "White") });
                        break;
                    case "Cab":
                        carInfo = new CarInfo(tmp.GetComponent<CarShowCaseButton>().nameOfCar,
                        "Yellow", true,
                        new List<MaterialInfo> { new MaterialInfo(true, "Yellow") });
                        break;
                    case "TopHatCar":
                        carInfo = new CarInfo(tmp.GetComponent<CarShowCaseButton>().nameOfCar,
                        "Black", true,
                        new List<MaterialInfo> { new MaterialInfo(true, "Black") });
                        break;
                    case "GoKart":
                        carInfo = new CarInfo(tmp.GetComponent<CarShowCaseButton>().nameOfCar,
                        "Blue", true,
                        new List<MaterialInfo> { new MaterialInfo(true, "Blue") });
                        break;

                }
                AddNewCarToCarList(carInfo);
                //FindActiveCar();
                //SaveActiveCar(FindIndexInCarMaList());
                SaveActiveCar();

                priceHolder.enabled = false;
                price.enabled = false;
                imgHolder.sprite = equipImg;

                UpdateCarButtonInfo();
            }
            else
            {
                print("Can't afford the Car");
            }
        }
    }

    private int FindIndexInCarMaList()
    {
        int savedIndex = 0;
        for (int i = 0; i < playerData.ListOfCars.Count; i++)
        {
            if (carNameFromList == playerData.ListOfCars[i].Name)
            {
                savedIndex = i;
                break;
            }
        }

        return savedIndex;
    }
    private void SaveCarMaterialNameData(int indexer) => playerData.ListOfCars[indexer].MaterialName = clickedMaterialName;
    private void SaveActiveCar()
    {
        foreach (var item in playerData.ListOfCars)
        {
            item.IsActive = false;
        }
        for (int i = 0; i < playerData.ListOfCars.Count; i++)
        {
            if (playerData.ListOfCars[i].Name == carButtonInstance.nameOfCar)
            {
                playerData.ListOfCars[i].IsActive = true;
                carNameFromList = playerData.ListOfCars[i].Name;
            }
        }
    }
    private void AddNewMaterialToCarList(int indexer) => playerData.ListOfCars[indexer].materialList.Add(new MaterialInfo(colorButtonInstance.Bought, colorButtonInstance.nameOfMaterial));
    private void AddNewCarToCarList(CarInfo car) => playerData.ListOfCars.Add(car);
    public void SetButtonName(GameObject gO) => clickedButtonName = gO.name;
    private void RemoveLetters(int amountTimes)
    {
        for (int i = 0; i < amountTimes; i++)
        {
            playerData.CollectedLetters.RemoveAt(0);
        }
    }
    private void SpawnCar(GameObject ActiveCar) => spawnedCar = Instantiate(ActiveCar, ShowcasedSpawnPoint);

    public void ClickOnCarTab()
    {
        colorsTab.SetActive(false);
        carsTabs.SetActive(true);

        UpdateCarButtonInfo();

        foreach (var item in colorBoxsList)
        {
            Destroy(item);
        }
        colorBoxsList.Clear();
        isOnColorTab = false;
    }
    private void UpdateCarButtonInfo()
    {
        List<CarShowCaseButton> carButtons = new List<CarShowCaseButton>();
        for (int i = 0; i < ShowcasedCar.Count; i++)
        {
            carButtons.Add(GameObject.Find($"CarButton ({i})").GetComponent<CarShowCaseButton>());
        }

        for (int i = 0; i < playerData.ListOfCars.Count; i++)
        {
            foreach (var item in carButtons)
            {
                if (playerData.ListOfCars[i].Name == item.nameOfCar)
                {
                    item.Bought = true;
                }
            }
        }
    }

    private bool isOnColorTab = true;
    public void ClickOnColorTab()
    {
        if (ReturnIsCarActive())
        {
            colorsTab.SetActive(true);
            carsTabs.SetActive(false);
            foreach (var item in playerData.ListOfCars)
            {
                if (item.IsActive)
                {
                    var tmp = CarListMaterials.Find(car => car.CarName == item.Name);
                    for (int i = 0; i < tmp.materialInfo.Count; i++)
                    {
                        InstantiateColorBoxes(tmp.materialInfo[i], i);
                    }
                    UpdateValues();
                    isOnColorTab = true;
                    break;
                }
            }
        }
        else
        {
            print("Can't see colors of this car before its equiped");
        }
    }

    /// <summary>
    /// Use for checking if car is equpied or not
    /// </summary>
    /// <returns></returns>
    private bool ReturnIsCarActive()
    {
        foreach (var item in playerData.ListOfCars)
        {
            if (carButtonInstance.nameOfCar == item.Name)
            {
                return true;
            }
        }
        return false;
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

public class ColorContainer
{
    Dictionary<string, Color> colorsDic = new Dictionary<string, Color>()
    {
        {"Red", new Color(1f, 0f, 0f)},           // RGB: (255, 0, 0)
        {"Green", new Color(0f, 1f, 0f)},         // RGB: (0, 255, 0)
        {"Blue", new Color(0f, 0f, 1f)},          // RGB: (0, 0, 255)
        {"White", new Color(1f, 1f, 1f)},         // RGB: (255, 255, 255)
        {"Black", new Color(0f, 0f, 0f)},         // RGB: (0, 0, 0)
        {"Yellow", new Color(1f, 1f, 0f)},        // RGB: (255, 255, 0)
        {"Cyan", new Color(0f, 1f, 1f)},          // RGB: (0, 255, 255)
        {"Magenta", new Color(1f, 0f, 1f)},       // RGB: (255, 0, 255)
        {"Gray", new Color(0.5f, 0.5f, 0.5f)},    // RGB: (128, 128, 128)
        {"Grey", new Color(0.5f, 0.5f, 0.5f)},    // RGB: (128, 128, 128)
        {"Clear", new Color(0f, 0f, 0f, 0f)},     // RGBA: (0, 0, 0, 0)
        {"Orange", new Color(1f, 0.5f, 0f)},      // RGB: (255, 128, 0)
        {"Brown", new Color(0.65f, 0.16f, 0.16f)},// RGB: (165, 42, 42)
        {"Purple", new Color(0.5f, 0f, 0.5f)},    // RGB: (128, 0, 128)
        {"Pink", new Color(1f, 0.75f, 0.8f)},     // RGB: (255, 192, 203)
        {"Lime", new Color(0.75f, 1f, 0f)},       // RGB: (191, 255, 0)
        {"Indigo", new Color(0.29f, 0f, 0.51f)},  // RGB: (75, 0, 130)
        {"Violet", new Color(0.93f, 0.51f, 0.93f)},// RGB: (238, 130, 238)
        {"Gold", new Color(1f, 0.84f, 0f)},       // RGB: (255, 215, 0)
        {"Silver", new Color(0.75f, 0.75f, 0.75f)},// RGB: (192, 192, 192)
        {"Teal", new Color(0f, 0.5f, 0.5f)},      // RGB: (0, 128, 128)
        {"Navy", new Color(0f, 0f, 0.5f)}         // RGB: (0, 0, 128)

    };
    public Color ReturnColorFromString(string colorName, out Color color)
    {
        color = colorsDic.GetValueOrDefault(colorName);
        return color;
    }
}
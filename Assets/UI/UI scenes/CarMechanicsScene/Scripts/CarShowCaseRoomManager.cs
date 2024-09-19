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
    private CarShowCaseButton carButtonInstance;
    private string clickedButtonName;

    [SerializeField] GameObject colorOptionsPrefab;
    [SerializeField] GameObject colorOptionsParent;
    private string carNameFromList;
    private bool isColor;

    private List<GameObject> colorBoxsList = new();

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
        isColor = true;
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
    private void SaveCarButton()
    {
        if (carButtonInstance.Bought)
        {
            FindActiveCar();
            SaveActiveCar(FindIndexInCarMaList());
        }
        else
        {
            if (carButtonInstance.price <= lettersCount)
            {
                RemoveLetters(carButtonInstance.price);
                var tmp = GameObject.Find(clickedButtonName);
                tmp.GetComponent<CarShowCaseButton>().Bought = true;
                CarInfo carInfo = new CarInfo("", "", false, null);
                switch (tmp.GetComponent<CarShowCaseButton>().nameOfCar)
                {
                    case "Van":
                        carInfo = new CarInfo(tmp.GetComponent<CarShowCaseButton>().nameOfCar,
                        "Gray", true,
                        new List<MaterialInfo> { new MaterialInfo(true, "Gray") });
                        break;
                }
                AddNewCarToCarList(carInfo);
                FindActiveCar();
                SaveActiveCar(FindIndexInCarMaList());

                priceHolder.enabled = false;
                price.enabled = false;
                imgHolder.sprite = equipImg;

                //UpdateValues();
            }
            else
            {
                print("Can't afford the Car");
            }
        }
    }
    private void FindActiveCar()
    {
        for (int i = 0; i < playerData.listOfCars.Count; i++)
        {
            if (playerData.listOfCars[i].IsActive)
            {
                carNameFromList = playerData.listOfCars[i].Name;
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
    private void SaveActiveCar(int indexer)
    {
        foreach (var item in playerData.listOfCars)
        {
            item.IsActive = false;
        }
        playerData.listOfCars[indexer].IsActive = true;
        carNameFromList = playerData.listOfCars[indexer].Name;


    }
    private void AddNewMaterialToCarList(int indexer)
    {
        playerData.listOfCars[indexer].materialList.Add(new MaterialInfo(buttonInstance.Bought, buttonInstance.nameOfMaterial));
    }
    private void AddNewCarToCarList(CarInfo car)
    {
        playerData.listOfCars.Add(car);
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

    public void ClickOnCarTab()
    {

        foreach (var item in colorBoxsList)
        {
            Destroy(item);
        }
        colorBoxsList.Clear();
        isOnColorTab = false;
    }
    private bool isOnColorTab = true;
    public void ClickOnColorTab()
    {
        if (isOnColorTab)
        {

        }
        else
        {

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
                    break;
                }
            }
        }
        isOnColorTab = true;
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
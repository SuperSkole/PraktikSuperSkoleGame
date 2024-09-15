using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class CarShowCaseRoomManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> ShowcasedCar = new List<GameObject>();
    [SerializeField] private List<CarMaterialInfo> CarListMaterials;
    [SerializeField] private Transform ShowcasedSpawnPoint;
    private GameObject spawnedCar;
    [SerializeField] private float rotationSpeed = 20f;  // Adjust speed here
    string clickedMaterialName = string.Empty;

    // Start is called before the first frame update
    void Start()
    {

        foreach (var item in PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>().listOfCars)
        {
            if (item.IsActive)
            {   
                SpawnCar(ReturnThePlayerCar(item.Name));
                PreviewColorOfCar(ReturnTheRightMaterial(item.MaterialName));
                StartCoroutine(StartRotationOfCar());
                break;
            }
        }


        //SpawnCar(ShowcasedCar[0]);
        //StartCoroutine(StartRotationOfCar());
    }
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
    private Material ReturnTheRightMaterial(string value)
    {
        foreach (var item in CarListMaterials)
        {
            if (value == item.CarName)
            {
                return item.CarMaterial;
            }
        }
        return null;
    }
    /// <summary>
    /// This methode changes the material of the spawnedPlayerCar, mostly used by buttons´.
    /// </summary>
    /// <param name="previewMaterial"></param>
    public void PreviewColorOfCar(Material previewMaterial)
    {
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

    public void SetStringNameOfMaterial(string materialName) => clickedMaterialName = materialName;


    public void SaveMaterialName() => PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>().listOfCars[0].MaterialName = clickedMaterialName;


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

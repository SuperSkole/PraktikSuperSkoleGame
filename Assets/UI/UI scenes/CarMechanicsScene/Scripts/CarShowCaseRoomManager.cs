using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarShowCaseRoomManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> ShowcasedCar = new List<GameObject>();
    [SerializeField] private Transform ShowcasedSpawnPoint;
    private GameObject spawnedCar;
    [SerializeField] private float rotationSpeed = 20f;  // Adjust speed here

    // Start is called before the first frame update
    void Start()
    {

        //Check for car name thingy
        //switch (PlayerManager.Instance.SpawnedPlayer.ActiveCar)
        //{
        //    case"RedCar":
        //      SpawnCar(ShowcasedCar[0]);
        //      StartCoroutine(StartRotationOfCar(ShowcasedCar[0]));
        //      break;
        //}

        SpawnCar(ShowcasedCar[0]);
        StartCoroutine(StartRotationOfCar());
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


    private void SpawnCar(GameObject ActiveCar)
    {
        spawnedCar = Instantiate(ActiveCar,ShowcasedSpawnPoint);
    }
    private IEnumerator StartRotationOfCar()
    {
        while (true)
        {
            // Rotate smoothly at a constant speed around the Y-axis
            ShowcasedSpawnPoint.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            print("JustMovedCar");
            yield return null;  // Yield until the next frame
        }
    }
}

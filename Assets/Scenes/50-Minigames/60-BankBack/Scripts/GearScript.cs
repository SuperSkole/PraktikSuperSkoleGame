using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Class to handle gears in the bank back minigame
/// </summary>
public class GearScript : MonoBehaviour
{
    [SerializeField]private GameObject toothPrefab;
    private float deltaAngle;
    private bool rotating = false;

    private List<MeshRenderer> meshRenderers;

    public int currentNumber = 0;
    /// <summary>
    /// Currently just calls the generateTeeth method
    /// </summary>
    void Start()
    {
        GenerateTeeth(transform, 0.6f, 10);
    }

    /// <summary>
    /// Starts rotating the gear if it is not currently rotating
    /// </summary>
    public void OnMouseDown()
    {
        if(!rotating)
        {
            StartCoroutine(RotateGear(1));
        }
    }
    

    /// <summary>
    /// Generates teeth around the parent transform in a circle, starting from the 9 o'clock position.
    /// Also sets up various teeth variables and the deltaangle
    /// </summary>
    /// <param name="parent">The parent transform to attach the teeth.</param>
    /// <param name="radius">The radius at which to place the teeth.</param>
    /// <param name="numberOfTeeth">The total number of teeth to generate.</param>
    private void GenerateTeeth(Transform parent, float radius, int toothnumber)
    {
        meshRenderers = new List<MeshRenderer>();
        deltaAngle = 360f / toothnumber;
        //Creates the teeth and sets up their variables
        for (var i = 0; i < toothnumber; i++)
        {
            float angle = i * Mathf.PI * 2 / toothnumber + Mathf.PI;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Quaternion rotation = Quaternion.LookRotation(position, Vector3.up);
            var tooth = Instantiate(toothPrefab, parent);
            tooth.transform.localPosition = position;
            tooth.transform.localRotation = rotation;
            tooth.transform.localScale = new Vector3(0.25f, tooth.transform.localScale.y, 0.25f);
            int displayNumber = i + 2;
            if(displayNumber > 9)
            {
                displayNumber -= 10;
            }
            tooth.name = "Tooth " + displayNumber;
            tooth.transform.GetChild(0).GetComponent<TextMeshPro>().text = displayNumber.ToString();
            tooth.GetComponent<GearChildScript>().parentScript = this;
            meshRenderers.Add(tooth.GetComponent<MeshRenderer>());
        }
    }

    /// <summary>
    /// Rotates the gear to display a new number with a speed based on the amount of seconds it should take
    /// </summary>
    /// <param name="seconds">how long the rotation should take</param>
    /// <returns></returns>
    private IEnumerator RotateGear(int seconds)
    {
        
        
        Vector3 remainingRotation = new Vector3(0, deltaAngle, 0);
        rotating = true;
        Vector3 deltaRotation = new Vector3(0, deltaAngle / (seconds * 25));
        //Rotates a bit until the gear has been rotated the correct amount
        while(remainingRotation.y > 0)
        {
            if(remainingRotation.y >= deltaRotation.y)
            {
                transform.Rotate(deltaRotation);
                remainingRotation.y -= deltaRotation.y;

                yield return new WaitForSeconds(0.01f);
            }
            else if(remainingRotation.y > 0)
            {
                transform.Rotate(remainingRotation);
                break;
            }
        }
        rotating = false;
        currentNumber++;
        if(currentNumber > 9)
        {
            currentNumber = 0;
        }
    }

    /// <summary>
    /// Changes the material of all the teeth
    /// </summary>
    /// <param name="material">The desired material</param>
    public void ChangeMaterial(Material material)
    {
        foreach(MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material = material;
        }
    }
}

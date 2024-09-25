using Assets.Scenes._50_Minigames._67_WordProductionLine.Scripts;
using Scenes._50_Minigames._67_WordProductionLine.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using static UnityEditor.Rendering.CameraUI;

public class ProductionLineController : MonoBehaviour
{
    
    private static GameObject selectedLetterBox, selectedImageBox;


    [SerializeField] private Material selectedMaterial, defaultMaterial, wrongMaterial;

    private static Material staticDefaultMaterial;

    [SerializeField] Camera mainCamera;


    [SerializeField]
    private int points = 0;


    RaycastHit hit;
    Ray ray;


    // Start is called before the first frame update
    void Start()
    {
        staticDefaultMaterial = defaultMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickHit();
        }
    }


    private void ClickHit()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if (hit.transform == null) return;

        GameObject hitObject = hit.transform.gameObject;

        if (!hitObject.CompareTag("ProductionCube"))
        {
            return;
        }

        ClickLetterCubes(hitObject);


        CheckIfCorrect();



    }


    private void ClickLetterCubes(GameObject hitObject)
    {
        if (hitObject.GetComponentInChildren<LetterBox>())
        {
            if (selectedLetterBox != null)
            {
                selectedLetterBox.GetComponentInChildren<MeshRenderer>().material = defaultMaterial;
            }

            selectedLetterBox = hitObject;
            selectedLetterBox.GetComponentInChildren<MeshRenderer>().material = selectedMaterial;
        }
        else
        {
            if (selectedImageBox != null)
            {
                selectedImageBox.GetComponentInChildren<MeshRenderer>().material = defaultMaterial;
            }

            selectedImageBox = hitObject;
            selectedImageBox.GetComponentInChildren<MeshRenderer>().material = selectedMaterial;

        }
    }

    /// <summary>
    /// Used for resetting selected material
    /// </summary>
    /// <param name="cube"></param>
    public static void ResetCubes(GameObject cube)
    {
        if (cube != selectedLetterBox && cube != selectedImageBox)
        {
            return;
        }

        if (cube == selectedImageBox)
        {
            selectedImageBox.GetComponentInChildren<MeshRenderer>().material = staticDefaultMaterial;

            selectedImageBox = null;
        }

        if (cube == selectedLetterBox)
        {
            selectedLetterBox.GetComponentInChildren<MeshRenderer>().material = staticDefaultMaterial;

            selectedLetterBox = null;
        }


    }


    /// <summary>
    /// Checks if the contents is correct
    /// </summary>
    /// <returns></returns>
    private void CheckIfCorrect()
    {
        if (selectedLetterBox != null && selectedImageBox != null)
        {


            char letter = selectedLetterBox.GetComponentInChildren<TextMeshProUGUI>().text.ToLower()[0];
            string imageName = selectedImageBox.GetComponentInChildren<RawImage>().texture.name.ToLower();
            imageName.Replace("(aa)", "\u00e5");
            imageName.Replace("(ae)", "\u00e6");
            imageName.Replace("(oe)", "\u00f8");
            char imageFirstLetter = imageName[0];

            if (letter == imageFirstLetter)
            {
                selectedLetterBox.GetComponentInChildren<IBox>().ResetCube();
                selectedImageBox.GetComponentInChildren<IBox>().ResetCube();
                ResetCubes(selectedImageBox);
                ResetCubes(selectedLetterBox);
                points++;
            }

            else
            {
                selectedLetterBox.GetComponentInChildren<MeshRenderer>().material = wrongMaterial;
                selectedImageBox.GetComponentInChildren<MeshRenderer>().material = wrongMaterial;
                StartCoroutine(WaitForXSeconds());
                selectedLetterBox.GetComponentInChildren<IBox>().ResetCube();
                selectedImageBox.GetComponentInChildren<IBox>().ResetCube();
                ResetCubes(selectedImageBox);
                ResetCubes(selectedLetterBox);
            }
        }
    }


    IEnumerator WaitForXSeconds()
    {
            // Wait for x seconds
            yield return new WaitForSeconds(2); 
    }
}

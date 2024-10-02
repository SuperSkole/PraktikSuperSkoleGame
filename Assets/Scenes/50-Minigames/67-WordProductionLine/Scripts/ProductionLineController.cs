using Assets.Scenes._50_Minigames._67_WordProductionLine.Scripts;
using Scenes;
using Scenes._50_Minigames._67_WordProductionLine.Scripts;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionLineController : MonoBehaviour
{

    private static GameObject selectedLetterBox, selectedImageBox;


    [SerializeField] private Material selectedMaterial, defaultMaterial, wrongMaterial, correctMaterial;

    private static Material staticDefaultMaterial;

    [SerializeField] Camera mainCamera;

    [SerializeField]
    private GameObject winScreen;


    [SerializeField]
    private int points = 0;

    [SerializeField]
    private GameObject scoreTextObject;

    public TextMeshProUGUI scoreText;

    public bool checking = false;


    RaycastHit hit;
    Ray ray;


    void Start()
    {
        scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        staticDefaultMaterial = defaultMaterial;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickHit();
        }
    }


    /// <summary>
    /// If you click a ProductionCube it registeres.
    /// </summary>
    private void ClickHit()
    {
        if (!checking)
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
    }

    /// <summary>
    /// Checks what you've clicked.
    /// </summary>
    /// <param name="hitObject">Describes what object was hit.</param>
    ///
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
    /// <param name="cube">what cube needs to reset.</param>
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
                StartCoroutine(WaitForRightXSeconds());

                if (points >= 5)
                {
                    StartCoroutine(CheckIfYouWin());
                }
            }

            else
            {
                CheckIfWrong();
            }
        }
    }


    /// <summary>
    /// checks if the chosen are wrong.
    /// </summary>
    private void CheckIfWrong()
    {
        
        // Start koroutinen
        StartCoroutine(WaitForWrongXSeconds());
    }

    /// <summary>
    /// Sets winscreen active and after a few seconds switches to GameWorld
    /// </summary>
    /// <returns> 2 second delay</returns>
    IEnumerator CheckIfYouWin()
    {
        
        winScreen.SetActive(true);
        yield return new WaitForSeconds(2);

        SwitchScenes.SwitchToMainWorld();
    }


    /// <summary>
    /// changes the color of the cube to green to indiacte Success and increases the Score
    /// </summary>
    /// <returns> 1 second delay</returns>
    IEnumerator WaitForRightXSeconds()
    {
        checking = true;
        selectedLetterBox.GetComponentInChildren<MeshRenderer>().material = correctMaterial;
        selectedImageBox.GetComponentInChildren<MeshRenderer>().material = correctMaterial;
        points++;
        scoreText.text = $"Score: {points}";

        yield return new WaitForSeconds(1);

        selectedLetterBox.GetComponentInChildren<IBox>().ResetCube();
        selectedImageBox.GetComponentInChildren<IBox>().ResetCube();
        ResetCubes(selectedImageBox);
        ResetCubes(selectedLetterBox);
        checking = false;
    }

    /// <summary>
    /// changes the color of the cube to red to indiacte error
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForWrongXSeconds()
    {
        checking = true;
        selectedLetterBox.GetComponentInChildren<MeshRenderer>().material = wrongMaterial;
        selectedImageBox.GetComponentInChildren<MeshRenderer>().material = wrongMaterial;


        yield return new WaitForSeconds(1);

        selectedLetterBox.GetComponentInChildren<IBox>().ResetCube();
        selectedImageBox.GetComponentInChildren<IBox>().ResetCube();
        ResetCubes(selectedImageBox);
        ResetCubes(selectedLetterBox);
        checking = false;
    }
}

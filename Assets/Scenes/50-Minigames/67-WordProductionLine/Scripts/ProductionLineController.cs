using Assets.Scenes._50_Minigames._67_WordProductionLine.Scripts;
using Scenes;
using Scenes._50_Minigames._67_WordProductionLine.Scripts;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionLineController : MonoBehaviour
{

    private static GameObject selectedLetterBox, selectedImageBox, lineObject;

    [SerializeField] GameObject particals;


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
    private bool hasChecked = false;


    private static LineRenderer lineRend;

    
    
    private Vector3 mousePos;

    RaycastHit hit;
    Ray ray;


    void Start()
    {
        scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        staticDefaultMaterial = defaultMaterial;
        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = 2;
        
    }

    void Update()
    {
        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit contact, 100, LayerMask.GetMask("RayBlocker")))
        {
            mousePos = contact.point;
        }

        

        if (Input.GetMouseButtonDown(0))
        {
            ClickHit();
        }

        if (selectedLetterBox != null && selectedImageBox == null)
        {
            DrawLineToMouse(selectedLetterBox);
        }

        if (selectedLetterBox == null && selectedImageBox != null)
        {
            DrawLineToMouse(selectedImageBox);
        }

        if (selectedLetterBox != null && selectedImageBox != null)
        {
            DrawLineBetweenBoxes();

            if (!hasChecked)
            {
                hasChecked = true;
            }
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
            Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("RayBox"));
            Debug.Log(hit);
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
    private void ClickLetterCubes(GameObject hitObject)
    {
        // Hvis det er en LetterBox
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
    /// Draws line from chosen box to mouse position.
    /// </summary>
    /// /// <param name="selectedBox">Describes the selected box.</param>
    private void DrawLineToMouse(GameObject selectedBox)
    {
        Vector3 boxPosition = selectedBox.transform.position;
        lineRend.SetPosition(0, new Vector3(boxPosition.x, boxPosition.y, 3.5f));
        lineRend.SetPosition(1, mousePos);
    }

    /// <summary>
    /// Draws line between 2 chosen boxes.
    /// </summary>
    private void DrawLineBetweenBoxes()
    {
        Vector3 startPos = selectedLetterBox.transform.position;
        Vector3 endPos = selectedImageBox.transform.position;

        lineRend.SetPosition(0, new Vector3(startPos.x, startPos.y, 3f));
        lineRend.SetPosition(1, new Vector3(endPos.x, endPos.y, 3f));
    }

    /// <summary>
    /// Used for resetting selected material
    /// </summary>
    /// <param name="cube">what cube needs to reset.</param>
    public static void ResetCubes(GameObject cube)
    {
        if (cube == selectedLetterBox)
        {
            selectedLetterBox.GetComponentInChildren<MeshRenderer>().material = staticDefaultMaterial;
            selectedLetterBox = null;

            if (selectedImageBox == null)
            {
                ResetLine();
            }
        }

        else if (cube == selectedImageBox)
        {
            // Reset selectedImageBox and its material
            selectedImageBox.GetComponentInChildren<MeshRenderer>().material = staticDefaultMaterial;
            selectedImageBox = null;

            // If both are null, reset the line as well
            if (selectedLetterBox == null)
            {
                ResetLine();
            }
        }


    }

    /// <summary>
    /// Resets the line to default
    /// </summary>
    public static void ResetLine()
    {
        // Assuming you have a method or logic to reset the line (could be hiding it or setting positions to default)
        lineRend.SetPosition(0, Vector3.zero);
        lineRend.SetPosition(1, Vector3.zero);
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

        particals.transform.localScale = new Vector3(3, 3, 3);
        Instantiate(particals, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(1);

        if (selectedLetterBox != null)
        {
            selectedLetterBox.GetComponentInChildren<IBox>().ResetCube();
            ResetCubes(selectedLetterBox);
        }

        if (selectedImageBox != null)
        {
            selectedImageBox.GetComponentInChildren<IBox>().ResetCube();
            ResetCubes(selectedImageBox);
        }
        

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

        if (selectedLetterBox != null)
        {
            selectedLetterBox.GetComponentInChildren<IBox>().ResetCube();
            ResetCubes(selectedLetterBox);
        }

        if(selectedImageBox != null)
        { 
            selectedImageBox.GetComponentInChildren<IBox>().ResetCube();
            ResetCubes(selectedImageBox);
        }
            

        checking = false;
    }
}

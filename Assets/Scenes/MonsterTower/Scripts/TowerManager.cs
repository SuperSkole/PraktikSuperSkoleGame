using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORE.Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{

    private int towerHeight;
    private int towerWidth;
    private int towerDepth = 3;


    private bool updateDimensions = false;
    [SerializeField] GameObject brickPrefab;
    [SerializeField] public List<Sprite> wrongImages;
    [SerializeField] public List<Sprite> wrongImages2;
    [SerializeField] Sprite image;
    private List<Sprite> allImagesInCurrentRow;
    private List <BrickData> brickLanes;
    private int currentLane = 0;
    public bool correctAnswer = false;



    string currentQuestion;
    int currentQuestionIndex = 0;
    [SerializeField] TextMeshProUGUI displayBox;
    [SerializeField] GameObject imageHolerPrefab;
    string[] sentanses;
    int difficulty = 0;
    GameObject[,] tower;

    RawImage mainImgae;
    RawImage topImage;
    RawImage bottomImage;


    // Start is called before the first frame update



    void Start()
    {
        BuildTower();
    }

    void BuildTower()
    {
        tower = new GameObject[towerWidth , towerHeight];
        for (int z = 0; z < towerHeight; z++)
        {
            for (int x = 0; x < towerWidth; x++)
            {
                Vector3 SpaceBetween = new(x * 2, z * 0, 0);
                Vector3 brickPos = gameObject.transform.position + new Vector3(x * brickPrefab.GetComponent<MeshRenderer>().bounds.size.x + SpaceBetween.x, z * brickPrefab.GetComponent<MeshRenderer>().bounds.size.y + SpaceBetween.y, 0);
                tower[x, z] = Instantiate(brickPrefab, brickPos, Quaternion.identity);
                tower[x, z].transform.parent = gameObject.transform;

                GameObject imageholder = Instantiate(imageHolerPrefab, tower[x,z].transform);
            }
        }
    }


    // Update is called once per frame
    // The tower dimensions gets updated here. 
    void Update()
    {

        TowerDimensionsUpdater();
    }

    /// <summary>
    /// updates the displaybox to the given string
    /// </summary>
    /// <param name="textToDispay">the string the displaybox is set to</param>
    void SetDispay(string textToDispay)
    {
        displayBox.text = textToDispay;
    }


    /// <summary>
    /// returns the next question
    /// </summary>
    /// <returns>the next question</returns>
    string GetQuestion()
    {
        return sentanses[currentQuestionIndex];
    }


    /// <summary>
    /// Method for setting the towerdata which is a list of Brickmanagers that holds a sentence,Correct image and list of incorrect images.
    /// </summary>
    /// <param name="bricks"></param>
    public void SetTowerData(string[] sentanses)
    {
        this.sentanses = sentanses;
        currentQuestion = GetQuestion();
        SetDispay(currentQuestion);
        //this.brickLanes = bricks;

        towerWidth = difficulty + 3;
        towerHeight = sentanses.Length;

        mainImgae = imageHolerPrefab.transform.GetChild(0).GetComponent<RawImage>();
        topImage = imageHolerPrefab.transform.GetChild(1).GetComponent<RawImage>();
        bottomImage = imageHolerPrefab.transform.GetChild(2).GetComponent<RawImage>();

        SetUpCurrentLevel();

        //allImagesInCurrentRow = brickLanes[currentLane].wrongImages;
        //allImagesInCurrentRow.Add(brickLanes[currentLane].correctImage);


        updateDimensions = true;
    }

    void SetUpCurrentLevel()
    {
        StringBuilder currentWord = new();
        List<string> allWords = new();
        foreach (char ch in currentQuestion)
        {
            if(ch !=  ' ')
            {
                allWords.Add(currentWord.ToString());
                currentWord = new();
                continue;
            }
            currentWord.Append(ch);
        }

        Image[] images = ImageManager.GetImageFromWord(allWords.ToArray());

        if (allWords[1].ToLower() == "på") topImage.texture = images[0].ConvertTo<Texture>();
        else bottomImage.texture = images[0].ConvertTo<Texture>();
        mainImgae.texture = images[1].ConvertTo<Texture>();


        GameObject imageholder = Instantiate(imageHolerPrefab);
    }
   


    /// <summary>
    /// Method for Updating dimensions of tower with the updateDimensions bool initiating it. 
    /// The dimensions are based on towerWidth and towerHeight that is set when you use the method SetTowerData.
    /// </summary>
    /// 
    public void TowerDimensionsUpdater()
    {
        //When the correct answer bool is set to true the current lane will be removed from the list of bricklanes. 
        // The tower width and height will be set according to the next lanes number of wrong images+1 and the number of lanes left.
        // If there is no more lanes left then all of the children of the tower, which is the bricks, is destroyed. 

        if(correctAnswer == true)
        {


            brickLanes.RemoveAt(currentLane);


            if (brickLanes.Count!=0)
            {
                //you got a function for this?
                towerWidth = brickLanes[currentLane].wrongImages.Count + 1;
                towerHeight = brickLanes.Count;

                allImagesInCurrentRow = brickLanes[currentLane].wrongImages;
                allImagesInCurrentRow.Add(brickLanes[currentLane].correctImage);

                correctAnswer = false;
                updateDimensions = true;
            }
            else
            {
                for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(gameObject.transform.GetChild(i).gameObject);
                }


                correctAnswer = false;
            }

           

        }


        // Deletes all bricks and inserts them according to the tower height and width set at the moment. 
        // Also sets the bricks images on the front lowest lane to the images from the current lane. 
        if (updateDimensions == true)
        {
            for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }


            for (int y = 0; y < towerHeight; y++)
            {

                for (int x = 0; x < towerWidth; x++)
                {
                    Vector3 SpaceBetween = new Vector3(x * 2, y * 0, 0);
                    Vector3 brickPos = gameObject.transform.position + new Vector3(x * brickPrefab.GetComponent<MeshRenderer>().bounds.size.x + SpaceBetween.x, y * brickPrefab.GetComponent<MeshRenderer>().bounds.size.y + SpaceBetween.y, 0);
                    // Debug.Log("brickPos:" + brickPos);


                    // Gets a hold of the child of the child to the brick which is an image component.
                    // Then sets the sprite to the image for the current box in the row.
                    // The image is taken from the list of allImagesInCurrentRow
                    // the brick also gets info on the correct image and the image that it is. 
                    // Only the front row of the lowest lane of bricks gets an image so therefor y==0
                    Brick controller = brickPrefab.GetComponent<Brick>();
                    if (y == 0)
                    {
                        brickPrefab.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = allImagesInCurrentRow[x];
                        controller.correctSprite = brickLanes[currentLane].correctImage;
                        controller.sprite = allImagesInCurrentRow[x];
                        controller.isShootable = true;
                    }
                    else
                    {
                        brickPrefab.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
                        controller.isShootable = false;
                    }

                    //instantiates the brick and makes it parrent to the tower gameobject. 
                    var brickInstans = Instantiate(brickPrefab, brickPos, Quaternion.identity);

                    brickInstans.transform.parent = gameObject.transform;



                    // builds the depth of the tower. 
                    for (int z = 1; z< towerDepth; z++)
                    {
                        Vector3 SpaceBetweenZ = new Vector3(x*1, y * 0, z*1);
                        Vector3 brickPosZ = gameObject.transform.position + new Vector3(x * brickPrefab.GetComponent<MeshRenderer>().bounds.size.x + SpaceBetween.x, y * brickPrefab.GetComponent<MeshRenderer>().bounds.size.y + SpaceBetween.y, z * brickPrefab.GetComponent<MeshRenderer>().bounds.size.z + SpaceBetween.z);
                        //Debug.Log("brickPos:" + brickPos);

                        brickPrefab.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
                        var brickInstansZ = Instantiate(brickPrefab, brickPosZ, Quaternion.identity);

                        brickInstansZ.transform.parent = gameObject.transform;

                        
                    }

                }


               
            }


           

            updateDimensions = false;

        }
    }
    

    
}

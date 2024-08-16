using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORE.Scripts;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;




public class TowerManager : MonoBehaviour,IDataPersistence
{

    private int towerHeight;
  
    private GameObject[,] tower;

    private GameObject[,] loadedTower;

    private int rowToDelete=0;


    [SerializeField] GameObject brickPrefab;

    [SerializeField] GameObject canvasPrefab;

    [SerializeField] public List<Sprite> wrongImages;
    [SerializeField] public List<Sprite> wrongImages2;
    
    [SerializeField] Sprite image;

    private int towerRadius = 20;
    private int numberOfBricksInLane = 40;
  


    private List<Sprite> allImagesInCurrentRow;

  
    private List <BrickLane> BrickLanes;

    private int currentLane = 0;

    public bool correctAnswer = false;

    private Vector3 brickDimensions;

    private int amountOfOptions = 4;



    private List<BrickLane> loadedBrickLanes;

    public bool isLevelLoaded = false;



    string currentQuestion;
    int currentQuestionIndex = 0;
    [SerializeField] TextMeshProUGUI displayBox;
    [SerializeField] GameObject imageHolerPrefab;
    string[] sentanses;

    RawImage topImage;
    RawImage bottomImage;
    // Start is called before the first frame update


    /// <summary>
    /// if the images arent loaded, waits with building the tower until the images are loaded
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitUntillDataIsLoaded()
    {
        while (!ImageManager.IsDataLoaded)
        {
            yield return null;
        }
        BuildTower();
    }

    /// <summary>
    /// sets up all the data for the tower
    /// </summary>
    /// <param name="input">an array of sentenses the is used in the game</param>
    public void SetTowerData(string[] input)
    {
        towerHeight = input.Length;
        sentanses = input;

        topImage = imageHolerPrefab.transform.GetChild(0).GetComponent<RawImage>();
        bottomImage = imageHolerPrefab.transform.GetChild(1).GetComponent<RawImage>();

        brickDimensions = brickPrefab.GetComponent<MeshRenderer>().bounds.size;
        SetNextQuestion();
        StartCoroutine(WaitUntillDataIsLoaded());
    }


    /// <summary>
    /// updates the display to show the next question
    /// </summary>
    void SetNextQuestion()
    {
        if (sentanses.Length <= currentQuestionIndex) return;
        currentQuestion = sentanses[currentQuestionIndex];
        displayBox.text = currentQuestion;
        currentQuestionIndex++;
    }


    IEnumerator WaitUntilDataIsLoaded()
    {
        while(!isLevelLoaded)
        {
            yield return null;
        }

      

       
            towerHeight = loadedBrickLanes.Count;


    
          
            brickDimensions = brickPrefab.GetComponent<MeshRenderer>().bounds.size;

            BuildTower();
     
      

    }

    // Update is called once per frame
    // The tower�checks if the right answer has been chosen and destroys the lowest tower lane. 
    void Update()
    {

        if (correctAnswer)
        {
            SetNextQuestion();
            DestroyLowestTowerLane();
            correctAnswer = false;
        }
    }

    // the lowest tower lane is destroyed by knowing the numberOfBricksInLane and the accessing the 2d tower array that have all the bricks.
    // Lastly the whole tower is lowered the same amount as the height of a brick. 
    void DestroyLowestTowerLane()
    {
        if (rowToDelete >= towerHeight) return;
        for (int i = 0; i < numberOfBricksInLane; i++)
        {

            for (int i = 0; i < numberOfBricksInLane; i++)
            {

                Destroy(loadedTower[i, rowToDelete]);
                

            }

           

            loadedBrickLanes.RemoveAt(0);



            rowToDelete++;
        }
        rowToDelete++;

        // sets the next rows pictures active and shows them. 
        for (int i = 0; i < numberOfBricksInLane; i++)
        {
            if (i <= amountOfOptions - 1)
            {
                tower[i, rowToDelete].transform.GetChild(0).gameObject.SetActive(true);
                Brick brickComponent = tower[i, rowToDelete].GetComponent<Brick>();
                brickComponent.isShootable = true;
               
            }
        }
        gameObject.transform.Translate(0, -brickDimensions.y, 0);
    }


    /// <summary>
    /// Method for building the tower based on numberOfBricksInLane, a list of brick data,towerHeight and radius. 
    /// The bricks are put into a 2D array with the parameteres x and z.
    /// x is the id of the brick in the x-axis. 
    /// z is the id of the height/lane the brick is in.  
    ///
    /// </summary>
    /// 
    void BuildTower()
    {

        //The tower angle is a value that represents the angle between the bricks and center of the tower. 
        //The start angle is an angle i chose based on where i want the tower to start build. 
        // The reason for this is so the first bricks in the 2d tower array is the ones used for displaying the pictures.

        float towerAngle = 2*Mathf.PI / numberOfBricksInLane;
        float startAngle = 180.3f;
        loadedTower = new GameObject[numberOfBricksInLane, towerHeight];

        // the tower is built based on tower height and numberOfBricksInLane with a for loop. 
        for (int z = 0; z < towerHeight; z++)
        {

            // Random correct image index is used so the right answer is put randomly between the posible positions. 
            int correctImageIndex = UnityEngine.Random.Range(0, amountOfOptions - 1);
            for (int x = 0; x < numberOfBricksInLane; x++)
            {
                //the new position of each brick is calculated based on angle,tower radius and the dimension of the brick. 
                //Calculating x and z with x=cos(v)*r and z=sin(v)*r
                // y is calculated based on the y dimension of the brick so the next lane is directly on top of the previus one.  
                float posX = Mathf.Cos(startAngle) * towerRadius;
                float posY = Mathf.Sin(startAngle) * towerRadius;
                Vector3 newPos = gameObject.transform.position+new Vector3(posX, z * brickDimensions.y, posY);

                // brick is the instantiated and the angle set as startangle. 
                // The brick is put into the 2d tower array. 
                // the brick is rotated so the picture would be facing the right way.
                // Then the towerobject is set as the parrent to the brick. 

                loadedTower[x, z] = Instantiate(brickPrefab, newPos, quaternion.Euler(0,-startAngle,0));
                loadedTower[x, z].transform.Rotate(new Vector3(0, -90, 0));
                loadedTower[x, z].transform.parent = gameObject.transform;

                // The amount of options is a value that can be set based on difficulty if more potenial options is needed.
                if (x <= amountOfOptions - 1)
                {
                    Brick brickComponent = tower[x, z].GetComponent<Brick>();
                    // The images are set here and instantiatetd on the right bricks. 
                    // and based on the value of correctImageIndex the right answer is set. 
                    if (x == loadedBrickLanes[z].correctImageIndex)
                    {
                        SetCorrectImage(sentanses[z]);
                        brickComponent.isCorrect = true;
                    }
                    else
                        SetRandomImage();
                    GameObject imageholder = Instantiate(imageHolerPrefab, tower[x, z].transform);
                    imageholder.GetComponent<RectTransform>().localPosition = new(0, 0, -0.5001f);
                    if (z == 0)
                    {
                        brickComponent.isShootable = true;
                        imageholder.SetActive(true);
                    }

                }
                // startAngle is updated so the next brick gets placed further along the circle.

                startAngle += towerAngle;

            }
        }

    }

    public void LoadData(GameData data)
    {
        this.loadedBrickLanes = data.BrickLanes;
    }
    /// <summary>
    /// sets the image up to match the sentens given
    /// </summary>
    /// <param name="sent">the sentens that the images is matching</param>
    void SetCorrectImage(string sent)
    {
        List<string> words = new();
        StringBuilder currentWord = new();
        for (int i = 0; i < sent.Length; i++)
        {
            char ch = sent[i];
            if(ch == ' ')
            {
                words.Add(currentWord.ToString());
                currentWord = new StringBuilder();
                continue;
            }

            currentWord.Append(ch);
        }
        words.Add(currentWord.ToString());
        if (words.Count < 3)
        {
            Debug.Log("Tower expected 3 words sentences but got less. setting random image as correct image");
            SetRandomImage();
            return;
        }

        switch (words[1])
        {
            case "på":
                bottomImage.texture = ImageManager.GetImageFromWord(words[2]);
                topImage.texture = ImageManager.GetImageFromWord(words[0]);
                break;
            case "under":
                topImage.texture = ImageManager.GetImageFromWord(words[2]);
                bottomImage.texture = ImageManager.GetImageFromWord(words[0]);
                break;
            default:
                Debug.Log("word is not in switch case please add it.");
                break;
        }
    }


    /// <summary>
    /// sets random wrong images
    /// </summary>
    void SetRandomImage()
    { 
        bottomImage.texture = ImageManager.GetRandomImage();
        topImage.texture = ImageManager.GetRandomImage();
    }


    

       

        Debug.Log("Data Loaded");

        isLevelLoaded = true;
    }

    public void SaveData(ref GameData data)
    {
        data.BrickLanes = this.loadedBrickLanes;
       

        Debug.Log("Data Saved");


    }
}

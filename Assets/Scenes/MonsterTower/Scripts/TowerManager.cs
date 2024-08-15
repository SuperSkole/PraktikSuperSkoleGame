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




public class TowerManager : MonoBehaviour
{

    private int towerHeight;
  
    private GameObject[,] tower;

    private int rowToDelete=0;


    [SerializeField] GameObject brickPrefab;

    [SerializeField] GameObject canvasPrefab;

    [SerializeField] public List<Sprite> wrongImages;
    [SerializeField] public List<Sprite> wrongImages2;
    
    [SerializeField] Sprite image;

    private int towerRadius = 20;
    private int numberOfBricksInLane = 40;
  
    public bool correctAnswer = false;

    private Vector3 brickDimensions;

    private int amountOfOptions = 5;

    



    string currentQuestion;
    int currentQuestionIndex = 0;
    [SerializeField] TextMeshProUGUI displayBox;
    [SerializeField] GameObject imageHolerPrefab;
    string[] sentanses;

    RawImage mainImgae;
    RawImage topImage;
    RawImage bottomImage;
    // Start is called before the first frame update


   
    void Start()
    {

    }

    public void SetTowerData(string[] input)
    {
        towerHeight = input.Length;
        sentanses = input;

        mainImgae = imageHolerPrefab.transform.GetChild(0).GetComponent<RawImage>();
        topImage = imageHolerPrefab.transform.GetChild(1).GetComponent<RawImage>();
        bottomImage = imageHolerPrefab.transform.GetChild(2).GetComponent<RawImage>();

        brickDimensions = brickPrefab.GetComponent<MeshRenderer>().bounds.size;
        SetNextQuestion();
        BuildTower();
    }

    void SetNextQuestion()
    {
        currentQuestion = sentanses[currentQuestionIndex];
        currentQuestionIndex++;
    }

    // Update is called once per frame
    // The tower�checks if the right answer has been chosen and destroys the lowest tower lane. 
    void Update()
    {

        if (correctAnswer)
        {
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
            Destroy(tower[i, rowToDelete]);
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
        tower = new GameObject[numberOfBricksInLane, towerHeight];

        // the tower is built based on tower height and numberOfBricksInLane with a for loop. 
        for (int z = 0; z < towerHeight; z++)
        {

            // Random correct image index is used so the right answer is put randomly between the posible positions. 
            int correctImageIndex = UnityEngine.Random.Range(0, amountOfOptions);
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

                tower[x, z] = Instantiate(brickPrefab, newPos, quaternion.Euler(0,-startAngle,0));
                tower[x, z].transform.Rotate(new Vector3(0, -90, 0));
                tower[x, z].transform.parent = gameObject.transform;

                // The amount of options is a value that can be set based on difficulty if more potenial options is needed.
                if (x <= amountOfOptions-1)
                {
                    Brick brickComponent = tower[x, z].GetComponent<Brick>();
                    if (z == 0)
                    {
                        brickComponent.isShootable = true;
                    }
                    // The images are set here and instantiatetd on the right bricks. 
                    // and based on the value of correctImageIndex the right answer is set. 
                    if (x == correctImageIndex)
                        SetCorrectImage();
                    else
                        SetRandomImage();
                    GameObject imageholder = Instantiate(imageHolerPrefab, tower[x, z].transform);
                    imageholder.transform.position = new(0, 0, -0.51f);

                }

                // startAngle is updated so the next brick gets placed further along the circle.

                startAngle += towerAngle;

            }
        }

    }

    void SetCorrectImage()
    {

    }

    void SetRandomImage()
    {

    }


    
}

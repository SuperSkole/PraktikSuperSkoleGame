using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AsteroidGameManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject hexagon;

    [SerializeField] GameObject pentagon;

    [SerializeField] GameObject square;

    [SerializeField] GameObject triangle;

    [SerializeField] DestroyOnAsteroidContact destroyOnAsteroidContact;

    [SerializeField] GameObject HealthUI;

    [SerializeField] GameObject GuideUI;
    public int score=0;

    [SerializeField] TextMeshProUGUI textMesh;

    [SerializeField] float minXForce;
    [SerializeField] float maxXForce;
    [SerializeField] float minYForce;
    [SerializeField] float maxYForce;
    private float speed;
    float timer=0;

    int spawnedAsteroids=0;

    float spawnFrequency = 10;

    float spawnCoolDown = 20;

    int polygonIndex;

    Vector3 spawnPosition;
    void Start()
    {
        spawnPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
      

        //updates the score text
        textMesh.text = "Score:" + score;

        //updates timer for spawning. 
        timer += Time.deltaTime;

        // sets a longer spawnfrequency when 5 asteroids have been spawned. 
        if(spawnedAsteroids==5)
        {
            spawnFrequency = spawnCoolDown;
            spawnedAsteroids = 0;
        }


        //When the timer is over the spawnfrequency the guide ui is deactivated and a random asteroid is spawned with a random force added to its rigidbody2D. 
        // The speed is chosen based on the kind of asteroid is spawned.
        // The AsteroidGamemanager is also added so the public score can be updated when an asteroid is destroyed. 
        if(timer>=spawnFrequency)
        {
            GuideUI.SetActive(false);
            if(timer>=spawnCoolDown)
            {
                spawnFrequency = 10;
            }

            timer = 0;

            

            spawnedAsteroids++;
            polygonIndex =Random.Range(0, 4);
            Vector3 randomForce = new Vector3(Random.Range(minXForce, maxXForce), Random.Range(minYForce, maxYForce), 0);

            
            switch (polygonIndex)
            {
                case 0:
                    speed = 7000;
                   var spawnedTriangle= Instantiate(triangle, spawnPosition, Quaternion.identity, gameObject.transform);
                    spawnedTriangle.GetComponent<Rigidbody2D>().AddForce(randomForce * speed);
                    spawnedTriangle.GetComponent<TriangleManager>().gameManager = this;

                    break;
                case 1:
                    speed = 5000;
                    var spawnedSquare=Instantiate(square, spawnPosition, Quaternion.identity, gameObject.transform);
                    spawnedSquare.GetComponent<Rigidbody2D>().AddForce(randomForce * speed);
                    spawnedSquare.GetComponent<SquareManager>().gameManager = this;
                    break;

                case 2:
                    speed = 3000;
                    var spawnedPentagon=Instantiate(pentagon, spawnPosition, Quaternion.identity, gameObject.transform);
                    spawnedPentagon.GetComponent<Rigidbody2D>().AddForce(randomForce * speed);
                    spawnedPentagon.GetComponent<PentagonManager>().gameManager = this;
                    break;
                case 3:
                    speed = 2000;
                    var spawnedHexagon=Instantiate(hexagon,spawnPosition,Quaternion.identity, gameObject.transform);
                    spawnedHexagon.GetComponent<Rigidbody2D>().AddForce(randomForce * speed);
                    spawnedHexagon.GetComponent<HexagonManager>().gameManager = this;
                    break;

            }
            
        }
        
    }


    /// <summary>
    /// Updates the healthUI component based on lifepoints and deactivating one by one the small spaceships that are indicating the number of lifepoints left. 
    /// </summary>
    private void UpdateHealthUI()
    {
        switch (destroyOnAsteroidContact.lifePoints)
        {
            case 2:
                HealthUI.transform.GetChild(2).gameObject.SetActive(false);
                break;

            case 1:
                HealthUI.transform.GetChild(1).gameObject.SetActive(false);
                break;

            case 0:
                HealthUI.transform.GetChild(0).gameObject.SetActive(false);
                break;
        }
    }
}

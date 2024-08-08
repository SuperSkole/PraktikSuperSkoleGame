using Minigames.RulleMarie.Managers;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : MonoBehaviour
{

    private int towerHeight;
    private int towerWidth;
    private int towerDepth;


    private bool updateDimensions=true;
    [SerializeField] GameObject brick;

    [SerializeField] public List<Sprite> wrongImages;

    [SerializeField] public List<Sprite> wrongImages2;

    [SerializeField] Sprite image;


    private List<Sprite> allImagesInCurrentRow;

  
    private List <BrickManager> brickLanes;

    private int currentLane = 0;

    public bool CorrectAnswer = false;



    // Start is called before the first frame update
    void Start()
    {
       


        brickLanes = new List<BrickManager>() {
          new BrickManager("hello", image, wrongImages)
          ,new BrickManager("Goodbye", image, wrongImages2)
         };


        towerWidth = brickLanes[currentLane].wrongImages.Count + 1;
        towerHeight = brickLanes.Count;
        towerDepth = 3;

        allImagesInCurrentRow = brickLanes[currentLane].wrongImages;
        allImagesInCurrentRow.Add(brickLanes[currentLane].correctImage);


    }

    // Update is called once per frame
    void Update()
    {

        TowerDimensionsUpdater();
    }


   

    /// <summary>
    /// Method for Updating dimensions of tower with the updateDimensions bool initiating it. 
    /// The dimensions are based on towerWidth and towerHeight. 
    /// </summary>
    /// 
    public void TowerDimensionsUpdater()
    {
        if(CorrectAnswer==true)
        {

            brickLanes.RemoveAt(currentLane);

           
            
            towerWidth = brickLanes[currentLane].wrongImages.Count+1;
            towerHeight = brickLanes.Count;

            allImagesInCurrentRow = brickLanes[currentLane].wrongImages;
            allImagesInCurrentRow.Add(brickLanes[currentLane].correctImage);

            CorrectAnswer =false;
            updateDimensions = true;

           

        }

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
                    Vector3 brickPos = gameObject.transform.position + new Vector3(x * brick.GetComponent<MeshRenderer>().bounds.size.x + SpaceBetween.x, y * brick.GetComponent<MeshRenderer>().bounds.size.y + SpaceBetween.y, 0);
                    Debug.Log("brickPos:" + brickPos);



                    if (y == 0)
                    {
                        brick.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = allImagesInCurrentRow[x];
                        brick.GetComponent<BrickController>().correctSprite = brickLanes[currentLane].correctImage;
                        brick.GetComponent<BrickController>().sprite = allImagesInCurrentRow[x];
                    }
                    else
                    {
                        brick.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
                    }


                    var brickInstans = Instantiate(brick, brickPos, Quaternion.identity);

                    brickInstans.transform.parent = gameObject.transform;


                    for (int z = 1; z< towerDepth; z++)
                    {
                        Vector3 SpaceBetweenZ = new Vector3(x*1, y * 0, z*1);
                        Vector3 brickPosZ = gameObject.transform.position + new Vector3(x * brick.GetComponent<MeshRenderer>().bounds.size.x + SpaceBetween.x, y * brick.GetComponent<MeshRenderer>().bounds.size.y + SpaceBetween.y, z * brick.GetComponent<MeshRenderer>().bounds.size.z + SpaceBetween.z);
                        Debug.Log("brickPos:" + brickPos);

                        brick.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
                        var brickInstansZ = Instantiate(brick, brickPosZ, Quaternion.identity);

                        brickInstansZ.transform.parent = gameObject.transform;

                        
                    }

                }


               
            }


           

            updateDimensions = false;

        }
    }
    

    
}

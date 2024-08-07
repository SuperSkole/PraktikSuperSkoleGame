using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{

    [SerializeField] int towerHeight;
    [SerializeField] int towerWidth;
    [SerializeField] bool updateDimensions;
    [SerializeField] GameObject brick;
    private Vector3 towerPosition;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {

        TowerDimensionsUpdater();
    }

    public void TowerDimensionsUpdater()
    {
        if (updateDimensions == true)
        {
            for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }

            for (int x = 0; x < towerWidth; x++)
            {
                for (int y = 0; y < towerHeight; y++)
                {
                    Vector3 SpaceBetween = new Vector3(x * 2, y * 2, 0);
                    Vector3 brickPos = gameObject.transform.position + new Vector3(x * brick.GetComponent<MeshRenderer>().bounds.size.x + SpaceBetween.x, y * brick.GetComponent<MeshRenderer>().bounds.size.y + SpaceBetween.y, 0);
                    Debug.Log("brickPos:" + brickPos);
                    var brickInstans = Instantiate(brick, brickPos, Quaternion.identity);

                    brickInstans.transform.parent = gameObject.transform;
                }

            }

            updateDimensions = false;

        }
    }
    

    
}

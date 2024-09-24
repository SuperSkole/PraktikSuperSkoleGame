using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounce : MonoBehaviour
{
   
    public Vector3 startPosition;
    public PathOfDangerManager manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.y<=-5 && manager.playerLifePoints>0)
        {
            manager.playerLifePoints -= 1;
            gameObject.transform.position = startPosition;
        }
        else
        {
            //To do: Make scene switch to lose screen. 
        }
    }
}

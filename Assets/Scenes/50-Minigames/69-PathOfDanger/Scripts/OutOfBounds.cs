using Scenes;
using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
   
    public GameObject startPosition;
    public PathOfDangerManager manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Checks if the player is under a certain treshold on the y axis and therefor out of bounds.
        //Also checks the amount of lifepoints the player has left and depending on the amount is either sent back to the start position or to the lose screen. 
        if(gameObject.transform.position.y<=-2 && manager.playerLifePoints>1)
        {
           
            manager.playerLifePoints -= 1;
            manager.DestroyAllPanels();
            PlayerManager.Instance.PositionPlayerAt(startPosition);
            StartCoroutine(manager.WaitUntillDataIsLoaded());
          
        }
        else if(gameObject.transform.position.y <= -3 &&manager.playerLifePoints<=1)
        {
            manager.StartGoToLoseScreenCoroutine();

          
        }
    }


  

   
}

using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounce : MonoBehaviour
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
        if(gameObject.transform.position.y<=-5 && manager.playerLifePoints>0)
        {
            Debug.Log("Out of Bounce");
            manager.playerLifePoints -= 1;
            manager.DestroyAllPanels();
            PlayerManager.Instance.PositionPlayerAt(startPosition);
            StartCoroutine(manager.WaitUntillDataIsLoaded());
          
        }
        else
        {
            //To do: Make scene switch to lose screen. 
        }
    }

   
}

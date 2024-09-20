using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWarp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Makes sure that the gameobject the component is attached to warps back into view from the opposite end of the screen. 

        // Gets a WorldToViewportPoint based on the players position.
        // A WorldToViewportPoint is a vector describing points within the camera view.
        // viewportPosition.x=1 is the furthest to the right and -1 is the furthest to the left.
        // Then based on the players WorldToViewportPoint an adjustment can be made so the player ends up in the oposite end of the screen. 
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        Vector3 moveadjustment = Vector3.zero;

        if(viewportPosition.x<0)
        {
            moveadjustment.x += 1;
        }
        else if(viewportPosition.x>1)
        {
            moveadjustment.x -= 1;
        }
        else if(viewportPosition.y<0)
        {
            moveadjustment.y += 1;
        }
        else if(viewportPosition.y>1)
        {
            moveadjustment.y -= 1;
        }

        transform.position = Camera.main.ViewportToWorldPoint(viewportPosition + moveadjustment);
        
    }
}

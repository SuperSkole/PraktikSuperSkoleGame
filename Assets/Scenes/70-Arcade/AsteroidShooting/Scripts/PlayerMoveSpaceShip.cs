using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMoveSpaceShip : MonoBehaviour
{
    [SerializeField] int speed; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetAxis("Vertical") > 0f)
        {
            gameObject.transform.Translate(new Vector3(0, speed, 0));
        }

        if (Input.GetAxis("Vertical") < 0f)
        {
            gameObject.transform.Translate(new Vector3(0, -speed, 0));
        }



    }
}

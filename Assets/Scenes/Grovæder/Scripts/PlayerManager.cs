using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posCorrect = new Vector3(0, 0, 0);
        if (transform.position.x > 19.5f){
            transform.Translate(transform.position.x - 19.5f, 0, 0);
        }
        if(Input.GetKeyDown(KeyCode.W) && player.transform.position.x < 19.5f){
            player.transform.Translate(1, 0, 0);
        }
        else if(Input.GetKeyDown(KeyCode.S) && player.transform.position.x > 10.5f){
            player.transform.Translate(-1, 0, 0);
        }
        else if(Input.GetKeyDown(KeyCode.A) && player.transform.position.z < -10.5f){
            player.transform.Translate(0, 0, 1);
        }
        else if(Input.GetKeyDown(KeyCode.D) && player.transform.position.z > -19.5f){
            player.transform.Translate(0, 0, -1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{

    public GameObject car; //player component dragged on the script here in the editor
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - car.transform.position; //Camera transform pos - player transform pos = offset
    }

    // Update is called once per frame
    void LateUpdate() //late update used because we want the physics of the player to run first
    {
        transform.position = car.transform.position + offset;
    }
}


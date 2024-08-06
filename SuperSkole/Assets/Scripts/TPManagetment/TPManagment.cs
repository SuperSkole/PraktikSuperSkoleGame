using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPManagment : MonoBehaviour
{
    //public List<GameObject> list = new List<GameObject>();
    public GameObject Player;
    public GameObject schoolEnter;
    public GameObject schoolExit;

    public void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Hit");
        if (collision.gameObject == schoolEnter && collision.gameObject.tag == "Player")
        {
            Player.transform.position = schoolExit.transform.position;
        }
    }
}

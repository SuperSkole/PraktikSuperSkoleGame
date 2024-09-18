using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsStartUp : MonoBehaviour
{
    private void Awake()
    {
     var manager = GameObject.FindWithTag("Manager").GetComponent<CarShowCaseRoomManager>();   
    }
}

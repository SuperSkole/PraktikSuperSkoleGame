using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingGoldAndExp : MonoBehaviour
{
    public GameObject GameManger;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameManger.GetComponent<GernalManagement>().AddGold(10);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GameManger.GetComponent<GernalManagement>().AddEXP(5);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameManger.GetComponent<GernalManagement>().RemoveGold(5);
        }

    }
}

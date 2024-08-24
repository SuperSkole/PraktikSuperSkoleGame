using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingGoldAndExp : MonoBehaviour
{
    public GameObject GameManger;
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameManger.GetComponent<GeneralManagement>().AddGold(10);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GameManger.GetComponent<GeneralManagement>().AddEXP(5);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameManger.GetComponent<GeneralManagement>().RemoveGold(5);
        }

    }
}

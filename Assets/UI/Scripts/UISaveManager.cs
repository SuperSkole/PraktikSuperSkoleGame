using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISaveManager : MonoBehaviour
{
    private Image imageOne = null;
    private Image nameOne = null;
    private string textOne = null;
    private bool savedOne = false;

    private Image imageTwo = null;
    private Image nameTwo = null;
    private string textTwo = null;
    private bool savedTwo = false;

    private Image imageThree = null;
    private Image nameThree = null;
    private string textThree = null;
    private bool savedThree = false;

    [SerializeField] SavePanel savePanelOne;
    [SerializeField] SavePanel savePanelTwo;
    [SerializeField] SavePanel savePanelThree;

    private void Awake()
    {
        //Udfyld variablerne her

        FillSaves();
    }

    private void FillSaves()
    {
        //send alt dataen til de tre savepanaler i inspektoren
        savePanelOne.SetSaveData(imageOne, nameOne, textOne, savedOne);
        savePanelTwo.SetSaveData(imageTwo, nameTwo, textTwo, savedTwo);
        savePanelThree.SetSaveData(imageThree, nameThree, textThree, savedThree);
    }

}

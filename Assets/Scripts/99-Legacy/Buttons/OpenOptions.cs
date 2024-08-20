using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class OpenOptions : MonoBehaviour
{
    [SerializeField] private GameObject settingsTab;
    //[SerializeField] private Animator animator;
    private Vector3 openOptions = new Vector3(-220, -378, 0);
    private Vector3 closeOptions = new Vector3(300, -378, 0);
    public void PlayerSettingsAnim()
    {
        settingsTab.SetActive(true);
        settingsTab.GetComponent<Animator>().SetInteger("Open",1);
        settingsTab.transform.localPosition = openOptions;
    }
    public void PlayerCloseSettingsAnim()
    {
        settingsTab.GetComponent<Animator>().SetInteger("Open", -1);
        settingsTab.transform.localPosition = closeOptions;
       // settingsTab.SetActive(false);
    }

}

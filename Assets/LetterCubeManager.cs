using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterCubeManager : MonoBehaviour
{

    [SerializeField]private GameObject cube;
    [SerializeField]private TextMeshPro text;
    private bool active;
    private string letter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(string letter){
        int lower = UnityEngine.Random.Range(0, 2);
        if(lower == 0){
            letter = letter.ToLower();
        }
        text.text = letter;
        this.letter = letter;
        if(!active){
           active = true;
           cube.transform.Translate(0, 0.2f, 0);
        }
    }

    public void Deactivate(){
        text.text = ".";
        letter = "";
        if(active){
            active = false;
            cube.transform.Translate(0, -0.2f, 0);
        }
    }
}

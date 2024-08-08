using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]private float MoveDelayRemaining = 0;
    [SerializeField]private GameObject textObject;
    private TextMeshPro cooldownText;



    // Start is called before the first frame update
    void Start()
    {
        cooldownText = textObject.GetComponent<TextMeshPro>();
        cooldownText.text = "";
    }
    
    // Update is called once per frame
    void Update()
    {
        if(MoveDelayRemaining == 0){
            if(Input.GetKeyDown(KeyCode.W) && transform.position.x < 19.5f){
            transform.Translate(1, 0, 0);
            }
            else if(Input.GetKeyDown(KeyCode.S) && transform.position.x > 10.5f){
                transform.Translate(-1, 0, 0);
            }
            else if(Input.GetKeyDown(KeyCode.A) && transform.position.z < 19.5f){
                transform.Translate(0, 0, 1);
            }
            else if(Input.GetKeyDown(KeyCode.D) && transform.position.z > 10.5f){
                transform.Translate(0, 0, -1);
            }
        }
        else{
            MoveDelayRemaining -= Time.deltaTime;
            if(MoveDelayRemaining < 0){
                MoveDelayRemaining = 0;
                cooldownText.text = "";
            }
            else {
                cooldownText.text = MoveDelayRemaining + " sek. tilbage";
            }
        }
    }

    public void IncorrectGuess(){
        MoveDelayRemaining = 6;
        cooldownText.text = MoveDelayRemaining + " sek. tilbage";
    }
}

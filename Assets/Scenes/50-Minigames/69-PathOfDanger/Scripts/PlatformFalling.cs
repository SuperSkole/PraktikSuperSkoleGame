using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFalling : MonoBehaviour
{

    [SerializeField] Rigidbody rb;
    public PathOfDangerManager manager;
    public bool isCorrectAnswer = false;
    [SerializeField] bool enteredBefore = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// When anything is colliding with the platform ,in this case the player, a check is made if is the correct platform with the right answer. 
    /// If correctAnswer is false the rigidbody on the platform will be set with no constraint on the y position axes and will fall. 
    /// If correctanswer is true the manager gets told and the next question is qued up. 
    /// also makes sure the code is also only run once with the enteredBefore bool. 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        
        if (isCorrectAnswer == false && enteredBefore == false)
        {
            manager.hasAnsweredWrong = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;

            enteredBefore = true;
        }

        if(isCorrectAnswer==true && enteredBefore==false)
        {

            manager.correctAnswer = true;
            enteredBefore = true;
        }
    }
}

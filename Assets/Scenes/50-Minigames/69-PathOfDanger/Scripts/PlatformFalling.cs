using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFalling : MonoBehaviour
{

    [SerializeField] Rigidbody rb;
    public PathOfDangerManager manager;
    public bool isCorrectAnswer = false;
    private bool enteredBefore = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isCorrectAnswer == false && enteredBefore == false)
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            enteredBefore = true;
        }

        if(isCorrectAnswer==true && enteredBefore==false)
        {

            manager.correctAnswer = true;
            enteredBefore = true;
        }
    }
}

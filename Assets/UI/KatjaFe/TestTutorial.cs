using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTutorial : MonoBehaviour
{
    private KatjaFe katjafe;

    public AudioClip audioClip1;
    public AudioClip audioClip2;

    private bool waitingForInput = false;

    private void Start()
    {

        katjafe = this.GetComponent<KatjaFe>();

        katjafe.Initialize(true,audioClip2);

        Speak();
    }

    private void Speak()
    {
        katjafe.KatjaIntro(() =>
        {
            katjafe.KatjaSpeak(audioClip1, () => 
            { 
                waitingForInput = true; 
            }); 

        });
            
    }

    private void Update()
    {
        if (waitingForInput)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                waitingForInput = false;
                katjafe.KatjaSpeak(audioClip2, () => { katjafe.KatjaExit(); });
            }
        }


    }
}

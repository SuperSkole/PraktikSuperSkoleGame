using CORE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTutorial : MonoBehaviour
{
    private KatjaFe katjafe;

    [SerializeField] private AudioClip Hey;
    [SerializeField] private AudioClip Move;
    [SerializeField] private AudioClip Good;
    [SerializeField] private AudioClip GoToDoor;

    private bool waitingForInput = false;

    private void Start()
    {
        katjafe = GetComponent<KatjaFe>();
        katjafe.Initialize(true,Move);
        if (GameManager.Instance.PlayerData.TutorialHouse)
        {
            katjafe.Initialize(false, Move);
            return;
        }
        Speak();
    }

    private void Speak()
    {
        katjafe.KatjaIntro(() =>
        {
            katjafe.KatjaSpeak(Hey, () => 
            {
                katjafe.KatjaSpeak(Move, () =>
                {
                    waitingForInput = true;
                });
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
                katjafe.KatjaSpeak(Good, () => 
                {
                    katjafe.KatjaSpeak(GoToDoor, () => 
                    { 
                        katjafe.KatjaExit();
                        GameManager.Instance.PlayerData.TutorialHouse = true;
                    });                    
                });
            }
        }
    }
}

using CORE.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGardenTutorual : MonoBehaviour
{
    private KatjaFe katjafe;

    [SerializeField] private AudioClip Hey;
    [SerializeField] private AudioClip Move;

    private bool waitingForInput = false;

    private void Start()
    {

        katjafe = GetComponent<KatjaFe>();

        katjafe.Initialize(true, Move);

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
                katjafe.KatjaSpeak(CongratsAudioManager.GetAudioClipFromDanishSet(Random.Range(0,CongratsAudioManager.GetLenghtOfAudioClipDanishList())), () =>
                {
                    katjafe.KatjaExit();
                });
            }
        }
    }
}

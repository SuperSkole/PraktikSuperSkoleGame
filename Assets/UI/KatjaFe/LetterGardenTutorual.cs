using CORE;
using CORE.Scripts;
using Scenes.Minigames.LetterGarden.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGardenTutorual : MonoBehaviour
{
    private KatjaFe katjafe;

    [SerializeField] private AudioClip Hey;
    [SerializeField] private AudioClip followBee;

    private bool waitingForInput = false;
    private DrawingHandler drawingHandler;

    private void Start()
    {
        katjafe = GetComponent<KatjaFe>();
        katjafe.Initialize(true, followBee);
        if (GameManager.Instance.PlayerData.TutorialLetterGarden) return;
        drawingHandler = FindFirstObjectByType<DrawingHandler>();
        Speak();
    }

    private void Speak()
    {
        katjafe.KatjaIntro(() =>
        {
            katjafe.KatjaSpeak(Hey, () =>
            {
                katjafe.KatjaSpeak(followBee, () =>
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
            if (drawingHandler.IsTutorualOver)
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

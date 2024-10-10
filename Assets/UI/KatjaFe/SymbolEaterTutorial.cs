using CORE;
using CORE.Scripts;
using Scenes._50_Minigames._54_SymbolEater.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class SymbolEaterTutorual : MonoBehaviour
{
    private KatjaFe katjafe;

    [SerializeField] private AudioClip Hey;
    [SerializeField] private AudioClip Move;

    private bool waitingForInput = false;
    private BoardController controller;

    private void Start()
    {
        katjafe = GetComponent<KatjaFe>();
        if (GameManager.Instance.PlayerData.TutorialSymbolEater)
        {

            katjafe.Initialize(false, Move);
            return;
        }
        katjafe.Initialize(true, Move);
        controller = FindFirstObjectByType<BoardController>();
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
            if (controller.isTutorialOver)
            {
                waitingForInput = false;
                katjafe.KatjaSpeak(CongratsAudioManager.GetAudioClipFromDanishSet(Random.Range(0, CongratsAudioManager.GetLenghtOfAudioClipDanishList())), () =>
                {
                    katjafe.KatjaExit();
                    GameManager.Instance.PlayerData.TutorialSymbolEater = true;
                });
            }
        }
    }
}

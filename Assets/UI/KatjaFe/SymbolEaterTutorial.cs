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
    [SerializeField] private AudioClip Intro;
    [SerializeField] private AudioClip letterSound;
    [SerializeField] private AudioClip onsetForPicture;
    [SerializeField] private AudioClip PictureToOnset;
    [SerializeField] private AudioClip PictureToVowel;
    [SerializeField] private AudioClip PictureToWord;
    [SerializeField] private AudioClip VowelForPicture;
    [SerializeField] private AudioClip VowelHunt;
    [SerializeField] private AudioClip WordToPicture;
    public int gamemode = 0;
    private bool waitingForInput = false;
    private BoardController controller;

    private void Start()
    {
        katjafe = GetComponent<KatjaFe>();
        if (GameManager.Instance.PlayerData.TutorialSymbolEater)
        {

            katjafe.Initialize(false, onsetForPicture);
            return;
        }
        katjafe.Initialize(true, onsetForPicture);
        controller = FindFirstObjectByType<BoardController>();
        Speak();
    }

    private void Speak()
    {
        katjafe.KatjaIntro(() =>
        {
            katjafe.KatjaSpeak(Hey, () =>
            {
                katjafe.KatjaSpeak(Intro, () =>
                {
                    switch (gamemode)
                    {
                        case 0:
                            katjafe.KatjaSpeak(onsetForPicture, () =>
                            {
                                waitingForInput = true;
                            });
                            break;
                        case 1:
                            katjafe.KatjaSpeak(PictureToOnset, () =>
                            {
                                waitingForInput = true;
                            });
                            break;
                        case 2:
                            katjafe.KatjaSpeak(PictureToVowel, () =>
                            {
                                waitingForInput = true;
                            });
                            break;
                        case 3:
                            katjafe.KatjaSpeak(PictureToWord, () =>
                            {
                                waitingForInput = true;
                            });
                            break;
                        case 4:
                            katjafe.KatjaSpeak(VowelForPicture, () =>
                            {
                                waitingForInput = true;
                            });
                            break;
                        case 5:
                            katjafe.KatjaSpeak(VowelHunt, () =>
                            {
                                waitingForInput = true;
                            });
                            break;
                        case 6:
                            katjafe.KatjaSpeak(WordToPicture, () =>
                            {
                                waitingForInput = true;
                            });
                            break;
                        default:
                            Debug.LogError("no gamemode selected");
                            break;
                    }
                    if (true)
                    {
                        
                    }
                    else
                    {
                        katjafe.KatjaSpeak(PictureToOnset, () =>
                        {
                            waitingForInput = true;
                        });
                    }

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

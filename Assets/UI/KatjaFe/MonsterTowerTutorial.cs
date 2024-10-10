using CORE;
using CORE.Scripts;
using Scenes._50_Minigames._54_SymbolEater.Scripts;
using Scenes._50_Minigames._65_MonsterTower.Scrips;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTowerTutorial : MonoBehaviour
{
    private KatjaFe katjafe;

    [SerializeField] private AudioClip Hey;
    [SerializeField] private AudioClip Explane;

    private bool waitingForInput = false;
    private TowerManager controller;

    private void Start()
    {
        katjafe = GetComponent<KatjaFe>();
        if (GameManager.Instance.PlayerData.TutorialMosterTower)
        {

            katjafe.Initialize(false, Explane);
            return;
        }
        katjafe.Initialize(true, Explane);
        controller = FindFirstObjectByType<TowerManager>();
        Speak();
    }

    private void Speak()
    {
        katjafe.KatjaIntro(() =>
        {
            katjafe.KatjaSpeak(Hey, () =>
            {
                katjafe.KatjaSpeak(Explane, () =>
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
                    GameManager.Instance.PlayerData.TutorialMosterTower = true;
                });
            }
        }
    }
}

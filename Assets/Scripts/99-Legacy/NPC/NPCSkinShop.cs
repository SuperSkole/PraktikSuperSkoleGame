using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class NPCSkinShop : MonoBehaviour
{
    [SerializeField] private GameObject nPCHandler;
    [SerializeField] private bool startAction;
    [SerializeField] private string nameOfNPC;
    [SerializeField] private Sprite imageOfNPC;
    [SerializeField] private string[] dialogue;

    [SerializeField] private UnityEvent yesButtonEvent;
    [SerializeField] private UnityEvent noButtonEvent;
    public void TalkingToPlayer()
    {
       nPCHandler.GetComponent<TalkingTOPlayer>().TalkingToPlayer(this.gameObject ,yesButtonEvent, noButtonEvent, startAction,nameOfNPC,imageOfNPC, dialogue);
    }
    public void DoneTalkingToPlayer()
    {
        nPCHandler.GetComponent<TalkingTOPlayer>().DoneTalkingToPlayer();
    }
}

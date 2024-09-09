using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarEventsManager : MonoBehaviour
{
    public GameObject interactionIcon;
    public UnityEvent CarInteraction { get; set; } = new UnityEvent();
    public bool IsInCar { get; set; }

    private void Awake()
    {
        if (enabled)
        {
            enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            InvokeAction();
        }
    }
    public void InvokeAction()
    {
        try
        {
            interactionIcon.SetActive(false);
            CarInteraction.Invoke();
            if (!IsInCar)
            {
                CarInteraction = new UnityEvent();
            }

        }
        catch { print("CarEventsManager/InvokeAction/No Car event"); }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonHandler : MonoBehaviour
{
    //Public så de kan tilskrives i inspektoren
    public UnityEvent onClick;
    public UnityEvent onHover;

    //Klik
    public void OnClick()
    {
        onClick?.Invoke();

    }
    //Hover
    public void OnHover()
    {
        onHover?.Invoke();
    }
}

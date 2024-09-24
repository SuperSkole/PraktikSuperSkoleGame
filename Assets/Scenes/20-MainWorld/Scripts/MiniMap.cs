using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    private bool isActive;

    [SerializeField] Image closePanel;

    private void Start()
    {
        isActive = gameObject.activeSelf;
    }

    public void Click()
    {
        if (isActive)
        {
            Exit();
        }
        else
        {
            Open();
        }
    }

    private void Open()
    {
        gameObject.SetActive(true);
        closePanel.gameObject.SetActive(true);
        isActive = true;
    }

    private void Exit()
    {
        gameObject.SetActive(false);
        closePanel.gameObject.SetActive(false);
        isActive = false;
    }
}

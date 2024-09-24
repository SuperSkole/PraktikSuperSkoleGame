using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapIcon : MonoBehaviour
{
    public Camera miniMapCamera;
    public RectTransform miniMapPanel;
    public GameObject iconPrefab;

    private GameObject iconInstance;

    private void Start()
    {
        iconInstance = Instantiate(iconPrefab, miniMapPanel);
        iconInstance.SetActive(false);
    }

    private void Update()
    {
        Vector3 screenPos = miniMapCamera.WorldToViewportPoint(transform.position);

        if (screenPos.z > 0 && screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1)
        {
            iconInstance.SetActive(true);
            Vector2 miniMapPos = new Vector2(
                (screenPos.x * miniMapPanel.sizeDelta.x) - (miniMapPanel.sizeDelta.x * 0.5f),
                (screenPos.y * miniMapPanel.sizeDelta.y) - (miniMapPanel.sizeDelta.y * 0.5f)
            );
            iconInstance.GetComponent<RectTransform>().anchoredPosition = miniMapPos;
        }
        else
        {
            iconInstance.SetActive(false);
        }
    }
}

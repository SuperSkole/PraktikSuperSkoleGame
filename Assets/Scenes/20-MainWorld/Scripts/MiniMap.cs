using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    private bool isActive;

    [SerializeField] Camera MiniMapCamera;
    [SerializeField] Image closePanel;
    [SerializeField] GameObject iconPrefab;
    [SerializeField] RectTransform miniMapUI;

    private List<GameObject> icons = new List<GameObject>();
    private string[] tagsToCheck = {"mark","Player" };



    private void Start()
    {
        isActive = gameObject.activeSelf;
        CreateIcons();
    }

    private void Update()
    {
        if (isActive)
        {
            UpdateIcons();
        }
    }

    private void CreateIcons()
    {
        foreach (string tag in tagsToCheck)
        {
            
            GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

                foreach (GameObject target in targets)
                {
                Debug.Log("found one");
                        GameObject icon = Instantiate(iconPrefab, miniMapUI);
                        icons.Add(icon);
                        MapInfo mapInfo = target.GetComponent<MapInfo>();

                    if (mapInfo != null)
                    {
                    MapIcon mapIcon = icon.GetComponent<MapIcon>();

                        if(mapIcon != null)
                        {
                            mapIcon.ChangeImage(mapInfo.iconImageName);
                        }
                        else
                        {
                            Debug.LogError("No mapIcon found on icon prefab");
                        }

                    }
                    else
                    {
                    Debug.LogError("No Mapinfo found on target");
                    }
                } 

        }
    }
    private void UpdateIcons()
    {
        int iconIndex = 0;

        foreach (string tag in tagsToCheck)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject target in targets)
            {
                Vector3 viewportPos = MiniMapCamera.WorldToViewportPoint(target.transform.position);

                if (viewportPos.z > 0)
                {
                    icons[iconIndex].SetActive(true);
                    Vector2 iconPos = new Vector2( (viewportPos.x - 0.5f) * miniMapUI.rect.width, (viewportPos.y - 0.5f) * miniMapUI.rect.height );
                    icons[iconIndex].GetComponent<RectTransform>().anchoredPosition = iconPos;
                }
                else
                {
                    icons[iconIndex].SetActive(false);
                }
                iconIndex++;
            }
        }
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
        ActivateIcons();
        isActive = true;
    }

    private void Exit()
    {
        gameObject.SetActive(false);
        closePanel.gameObject.SetActive(false);
        DeactivateIcons();
        isActive = false;
    }

    private void DeactivateIcons()
    {
        foreach (GameObject icon in icons)
        {
            icon.SetActive(false);
        }
    }

    private void ActivateIcons()
    {
        foreach (GameObject icon in icons)
        {
            icon.SetActive(true);
        }
    }
}

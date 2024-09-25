using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    private bool isActive;

    [SerializeField] Camera MiniMapCamera;
    [SerializeField] Image closePanel;
    [SerializeField] GameObject iconPrefab;
    [SerializeField] RectTransform miniMapUI;

    private List<GameObject> icons = new List<GameObject>();
    private string[] tagsToCheck = {"mark","Player"};

    private Dictionary<string, List<GameObject>> cachedTargets = new Dictionary<string, List<GameObject>>();

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
        //check tag list
        foreach (string tag in tagsToCheck)
        {
            //find all gameobjects with the tag
            cachedTargets[tag] = new List<GameObject>(GameObject.FindGameObjectsWithTag(tag));
                
                //Creating icon for each gameobject found
                foreach (GameObject target in cachedTargets[tag])
                {
                        //create icon as a child of miniMap
                        GameObject icon = Instantiate(iconPrefab, miniMapUI);
                        icons.Add(icon);

                        //Get map info from tagged gameobject
                        MapInfo mapInfo = target.GetComponent<MapInfo>();


                    if (mapInfo != null)
                    {
                        //get script from icon prefab
                        MapIcon mapIcon = icon.GetComponent<MapIcon>();

                        if(mapIcon != null)
                        {
                        //change image

                            mapIcon.ChangeImage(mapInfo.iconImageName);
                        }
                        else
                        {
                            Debug.Log("No mapIcon found on icon prefab");
                        }

                    }
                    else
                    {
                    Debug.Log("No Mapinfo found on target");
                    }
                } 

        }
    }
    private void UpdateIcons()
    {
        //keeping track of icon
        int iconIndex = 0;

        //Iteration through taglist
        foreach (string tag in tagsToCheck)
        {
            //if not in the cachedTarget list, from CreateIcons, skip it.
            if (!cachedTargets.ContainsKey(tag)) continue;

            //go through alle gameobjects in the cachedTarget list
            foreach (GameObject target in cachedTargets[tag])
            {
                //make sure we don't go above the registrede icon amount
                if (iconIndex < icons.Count)
                {
                    //find out the target's relative position to the camera
                    Vector3 viewportPos = MiniMapCamera.WorldToViewportPoint(target.transform.position);

                    //target needs to be in front the camera's view
                    if (viewportPos.z > 0)
                    {
                        icons[iconIndex].SetActive(true);
                        //konvert camera koordinates to minimap koordinates.
                        Vector2 iconPos = new Vector2((viewportPos.x - 0.5f) * miniMapUI.rect.width, (viewportPos.y - 0.5f) * miniMapUI.rect.height);
                        icons[iconIndex].GetComponent<RectTransform>().anchoredPosition = iconPos;
                    }
                    else
                    {
                        icons[iconIndex].SetActive(false);
                    }
                    iconIndex++;
                }
                else
                {
                    Debug.LogWarning("More targets than icons.");
                }
            }

        }
    }

    public void ShakeIcon (string typedName)
    {
        foreach (GameObject target in icons)
        {
              MapIcon gottenIcon = target.GetComponent<MapIcon>();

                if (gottenIcon != null && gottenIcon.IconName == typedName)
                {
                    gottenIcon.Highlight();
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

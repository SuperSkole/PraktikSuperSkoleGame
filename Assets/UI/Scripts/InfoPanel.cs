using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public RectTransform infoPanelRect;
    public RectTransform canvasRect;
    public RectTransform pivot;

    public Text contentText;

    //this.transform.position = mousePos;

    private void Update()
    {
       
    }

    void AdjustPanelSizeAndFlip()
    {
        //Text size
        Vector2 textSize = GetPreferredTextSize();

        //Adjust the panel size based on text size
        infoPanelRect.sizeDelta = new Vector2(textSize.x, textSize.y);

        //get panel's position in world space
        Vector3[] worldCorners = new Vector3[4];
        pivot.GetWorldCorners(worldCorners);

        //Get center position of the panel
        Vector3 panelCenter = (worldCorners[0]+worldCorners[2])/2;

        //Get the center position of Canvas
        Vector2 panelCenterInCanvas;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, panel)


    }
    Vector2 GetPreferredTextSize()
    {
        TextGenerationSettings settings = contentText.GetGenerationSettings(contentText.rectTransform.rect.size);
        return contentText.cachedTextGenerator.GetPreferredWidth(contentText.text, settings) * Vector2.right +
               contentText.cachedTextGenerator.GetPreferredHeight(contentText.text, settings) * Vector2.up;
    }
}

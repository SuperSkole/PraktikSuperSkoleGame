using Import.LeanTween.Framework;
using System.Collections;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public RectTransform infoPanelRect; 
    public TextMeshProUGUI contentText; 

    public RectTransform pivot; 

    private Canvas canvas;
    private RectTransform canvasRectTransform;
    private Vector2 canvasCenter;

    private bool lastFlipped = false;
    private bool lastFlopped = false;

    private Vector2 padding = new Vector2(50f, 50f);
    public Vector4 marginOffsets = new Vector4(10f, 10f, 10f, 10f);

    private void Start()
    {
        canvas = FindAnyObjectByType<Canvas>();

        if (canvas != null )
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }
        setInfo("Tilbage");
    }

    private void setInfo(string info)
    {
        canvasCenter = GetCanvasCenter();
        contentText.text = info;

        Vector2 preferredSize = contentText.GetPreferredValues();

        Vector2 newSize = new Vector2(preferredSize.x + padding.x, preferredSize.y + padding.y);
        infoPanelRect.sizeDelta = newSize;

    }

    private void Update()
    {
        // Set the panel position to the mouse position
        Vector2 mousePos = Input.mousePosition;
        this.transform.position = mousePos;

        if (canvasRectTransform != null)
        {

            if(mousePos.x < canvasCenter.x)
            {
                if(mousePos.y < canvasCenter.y)
                {
                    FlipTheBox(false, true);
                }
                else
                FlipTheBox(false, false);
            }
            else
            {
                if(mousePos.y < canvasCenter.y)
                {
                    FlipTheBox(true, true);
                }
                else
                FlipTheBox(true, false);
            }
        }
    }

    private Vector2 GetCanvasCenter()
    {
        Vector2 screenpoint = RectTransformUtility.WorldToScreenPoint(null, canvasRectTransform.position);

        return new Vector2(screenpoint.x, screenpoint.y);
    }

    private void FlipTheBox(bool flipped, bool flopped)
    {

        if (flipped != lastFlipped || flopped != lastFlopped)
        {
            lastFlipped = flipped;
            lastFlopped = flopped;

            Vector3 oldPosition = contentText.rectTransform.position;

            Vector2 newPivot = contentText.rectTransform.pivot;

            //Right
            if (flipped)
            {
                Vector3 scale = pivot.localScale;
                scale.x = -Mathf.Abs(scale.x);
                pivot.localScale = scale;

                Vector3 scaleText = contentText.rectTransform.localScale;
                scaleText.x = -Mathf.Abs(scaleText.x);
                contentText.rectTransform.localScale = scaleText;

                newPivot.x = 1f;
            }

            //Left
            if (!flipped)
            {
                Vector3 scale = pivot.localScale;
                scale.x = Mathf.Abs(scale.x);
                pivot.localScale = scale;

                Vector3 scaleText = contentText.rectTransform.localScale;
                scaleText.x = Mathf.Abs(scaleText.x);
                contentText.rectTransform.localScale = scaleText;

                newPivot.x = 0f;
            }

            //Bottom
            if (flopped)
            {
                Vector3 scale = pivot.localScale;
                scale.y = -Mathf.Abs(scale.y);
                pivot.localScale = scale;

                Vector3 scaleText = contentText.rectTransform.localScale;
                scaleText.y = -Mathf.Abs(scaleText.y);
                contentText.rectTransform.localScale = scaleText;

                newPivot.y = 0f;
            }
            //Top
            if (!flopped)
            {
                Vector3 scale = pivot.localScale;
                scale.y = Mathf.Abs(scale.y);
                pivot.localScale = scale;

                Vector3 scaleText = contentText.rectTransform.localScale;
                scaleText.y = Mathf.Abs(scaleText.y);
                contentText.rectTransform.localScale = scaleText;

                newPivot.y = 1f;
            }

            contentText.rectTransform.pivot = newPivot;
            AlignToPivot(contentText.rectTransform, pivot, oldPosition);

            float panelHeight = infoPanelRect.rect.height;
            float panelWidth = infoPanelRect.rect.width;

            contentText.margin = new Vector4(
                panelWidth * 0.05f + marginOffsets.x,
                panelHeight * 0.05f + marginOffsets.y,
                panelWidth * 0.05f + marginOffsets.z,
                panelHeight * 0.05f + marginOffsets.w);
        }

    }
    private void AlignToPivot(RectTransform content, RectTransform targetPivot, Vector3 oldPosition)
    {
        Vector3 pivotDelta = content.position - oldPosition;

        content.position = targetPivot.position - pivotDelta;
    }

}

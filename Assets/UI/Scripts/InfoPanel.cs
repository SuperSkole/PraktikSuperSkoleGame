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

    private Coroutine resetCoroutine;

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
        if(resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }

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

            //Right
            if (flipped)
            {
                Vector3 scale = pivot.localScale;
                scale.x = -Mathf.Abs(scale.x);
                pivot.localScale = scale;

                Vector3 scaleText = contentText.rectTransform.localScale;
                scaleText.x = -Mathf.Abs(scaleText.x);
                contentText.rectTransform.localScale = scaleText;

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

            }

            contentText.rectTransform.position = infoPanelRect.position;

           
        }

    }
 

}

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public RectTransform infoPanelRect; 
    public TextMeshProUGUI contentText; 

    public RectTransform pivot; 

    public Canvas canvas;
    private RectTransform canvasRectTransform;
    private Vector2 canvasCenter;

    private bool lastFlipped = false;
    private bool lastFlopped = false;

    private Vector2 padding = new Vector2(50f, 50f);

    private Coroutine resetCoroutine;


    public void setInfo(string info)
    {
        if(resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }

        if (resetCoroutine == null)
        {
            resetCoroutine = StartCoroutine(CheckMouseIdle());
        }

        canvasRectTransform = canvas.GetComponent<RectTransform>();

        canvasCenter = GetCanvasCenter();
        contentText.text = info;

        Vector2 preferredSize = contentText.GetPreferredValues();

        Vector2 newSize = new Vector2(preferredSize.x + padding.x, preferredSize.y + padding.y);
        infoPanelRect.sizeDelta = newSize;

        Debug.Log("This");

    }

    public void StopInfo()
    {
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            resetCoroutine = null;
        }

        infoPanelRect.gameObject.SetActive(false);
    }

    private IEnumerator CheckMouseIdle()
    {
        Vector3 lastMousePosition = Input.mousePosition;
        float timer = 0f;

        while (true)
        {
            if (Input.mousePosition != lastMousePosition)
            { 
               lastMousePosition = Input.mousePosition;
                timer = 0f;
                infoPanelRect.gameObject.SetActive(false);
                Debug.Log("mouse moved");
            
            }
            else
            {
                Debug.Log("mouse not moving");
                timer += Time.deltaTime;

                if (timer > 1f)
                {
                    Debug.Log("Open");
                    infoPanelRect.gameObject.SetActive(true);
                }
            }
            yield return null;
        }
        
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

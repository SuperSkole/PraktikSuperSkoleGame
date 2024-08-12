using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInputManager : MonoBehaviour
{


    public LayerMask buttonLayer;

    // Update is called once per frame
    void Update()
    {
        
    }

    //Muse input
    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInteraction(Input.mousePosition, true);
        }
        else if (Input.GetMouseButton(0))
        {
            HandleInteraction(Input.mousePosition, false);
        }
    }

    //Berørings input
    private void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            HandleInteraction(touch.position, touch.phase == TouchPhase.Began);
        }
    }

    private void HandleInteraction(Vector2 screenPos, bool isClick)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buttonLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            if(hitObject.CompareTag("Buttons"))
            {
                ButtonHandler buttonHandler = hitObject.GetComponent<ButtonHandler>();
                if(buttonHandler != null)
                {
                    if(isClick)
                    {
                        buttonHandler.OnClick();
                    }
                    else
                    {
                        buttonHandler.OnHover();
                    }
                }
            }
        }
    }
}

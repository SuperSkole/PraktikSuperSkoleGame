using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Minigames.MonsterTower.Scrips
{

    public class ToggleZoom : MonoBehaviour
    {
        float currentZoom;
        float zoomSpeed = 500;
        float minZoom = 100;
        [SerializeField] float maxZoom = 60;
        float velocity = 0;
        float smoothTime = 0.25f;

        bool doneZoom = true;
        bool zoomingIn = true;

        [SerializeField] Camera cam;

        void Start()
        {
            minZoom = cam.fieldOfView;
            currentZoom = minZoom;
        }


        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Z) && doneZoom)
            {
                doneZoom = false;
            }
            if(!doneZoom)
            {
                Zoom();
            }
        }

        void Zoom()
        {
            if(zoomingIn) currentZoom -= zoomSpeed * 0.1f * Time.deltaTime;
            else currentZoom -= zoomSpeed * -0.1f * Time.deltaTime;
            currentZoom = Mathf.Clamp(currentZoom, maxZoom, minZoom);
            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView,currentZoom,ref velocity,smoothTime);
            if (cam.fieldOfView <= maxZoom + 0.1f && zoomingIn || cam.fieldOfView >= minZoom -0.1f && !zoomingIn)
            {
                doneZoom = true;
                zoomingIn = !zoomingIn;
            }
        }
    }

}
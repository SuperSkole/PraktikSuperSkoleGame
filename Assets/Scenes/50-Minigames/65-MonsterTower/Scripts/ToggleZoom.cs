using Scenes._50_Minigames._65_MonsterTower.Scripts;
using System;
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
       public bool zoomingIn = true;

        [SerializeField] Camera cam;

        public bool towerLaneDestroyed = false;

        
      
       
        public Difficulty difficulty;

        void Start()
        {
            minZoom = cam.fieldOfView;
            currentZoom = minZoom;

            switch (difficulty)
            {
                case Difficulty.Easy:
                    maxZoom = 35;
                    break;
                case Difficulty.Medium:
                    maxZoom = 21;
                    break;
                case Difficulty.Hard:
                    maxZoom = 21;
                    break;
                default:
                    break;
            }

        }


        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Z) && doneZoom ||towerLaneDestroyed)
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

                // Makes it so when the Difficulty is set to hard you can't zoom out. 
                // And when it's set to hard the camera can only zoom in. 
                if (difficulty== Difficulty.Hard)
                {
                    zoomingIn = true;
                    towerLaneDestroyed = false;
                 
                }
                else
                {
                    zoomingIn = !zoomingIn;
                    towerLaneDestroyed = false;
                }
            }
        }

        /// <summary>
        /// Sets the right bools so the camera zooms out. Is only meant to be used when a towerlane is destroyed.
        /// </summary>
        public void ZoomOutWhenTowerLaneIsDestroyed()
        {
            zoomingIn = false;
            towerLaneDestroyed = true;
          

        }
    }

}
using Scenes._11_PlayerHouseScene.script.SaveData;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scenes._11_PlayerHouseScene.script.HouseScripts
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Camera sceneCamera;
        private Vector3 lastPos;
        [SerializeField] private LayerMask placementLayermask;

        public event Action OnClicked, OnExit, RotateMinus, RotatePlus;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClicked?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnExit?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateMinus?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                RotatePlus?.Invoke();
            }

        }

        public bool IsPointerOverUI() 
            => EventSystem.current.IsPointerOverGameObject();

        public Vector3 GetSelectedMapPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = sceneCamera.nearClipPlane;
            Ray ray = sceneCamera.ScreenPointToRay(mousePos);   
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,100, placementLayermask))
            {
                lastPos = hit.point;
            }
            return lastPos;
        }

    }
}

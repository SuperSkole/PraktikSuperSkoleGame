using System;
using System.Collections;
using System.Collections.Generic;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes.Minigames.LetterGarden.Scripts.Gamemodes;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace Scenes.Minigames.LetterGarden.Scripts
{
    public class DrawingHandler : MonoBehaviour, IMinigameSetup
    {
        public Camera m_camera;
        public GameObject brushPrefab;

        LineRenderer currentLineRenderer;
        Vector3 lastPos;
        public bool isPainting = true;
        public float offsetDistance = 0.5f;

        private List<GameObject> drawnBrushInstances = new List<GameObject>();

        [SerializeField] ActiveLetterHandler letterHandler;
        public Slider inkMeterSlider;
        public float maxInkAmount = 100f;
        private float currentInkAmount;
        private float minDist = 0.2f;

        private SplineSymbolDataHolder currentSymbol;

        [SerializeField] public GameObject bee;
        BeeMovement beeMovement;

        [SerializeField] private RectTransform screenShotBounds;
        [SerializeField] private GameObject correctLetterBox;
        [SerializeField] private TMP_FontAsset altFont;
        public bool IsTutorualOver = false;

        private void Start()
        {
            //Setup(new DrawNumbers());
        }


        private void Update()
        {
            if (isPainting)
            {
                Drawing();
            }

            inkMeterSlider.value = currentInkAmount;
        }


        /// <summary>
        /// used for drawing
        /// </summary>
        void Drawing()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                TryCreateBrush();
            }
            else if (Input.GetKey(KeyCode.Mouse0) && currentLineRenderer != null)
            {
                PointToMousePos();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                EndDrawing();
            }
        }

        /// <summary>
        /// gets the mouse pos in the world and sends it to CreateBrush.
        /// </summary>
        void TryCreateBrush()
        {
            if (Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
            {
                Vector3 mousePos = hit.point;
                mousePos += hit.normal * offsetDistance;

                CreateBrush(mousePos, hit.normal);
            }
        }

        /// <summary>
        /// creates a new brush(linesecment) to draw with.
        /// </summary>
        /// <param name="position">the start position of were the line starts</param>
        /// <param name="normal">the normal of the raycast</param>
        void CreateBrush(Vector3 position, Vector3 normal)
        {
            GameObject brushInstance = Instantiate(brushPrefab);
            currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
            brushInstance.transform.SetPositionAndRotation(position, Quaternion.LookRotation(normal));
            currentLineRenderer.positionCount = 2;
            currentLineRenderer.SetPosition(0, position);
            currentLineRenderer.SetPosition(1, position);
            lastPos = position;
            drawnBrushInstances.Add(brushInstance);
        }


        /// <summary>
        /// adds a point to the current line rendere for a secment.
        /// </summary>
        /// <param name="pointPos">the pos of the new point for the line</param>
        void AddAPoint(Vector3 pointPos)
        {
            float distance = Vector3.Distance(lastPos, pointPos);
            if (distance <= minDist) return;
            currentInkAmount -= distance;
            lastPos = pointPos;
            if (currentInkAmount <= 0)
            {
                EndDrawing();
                return;
            }

            currentLineRenderer.positionCount++;
            int positionIndex = currentLineRenderer.positionCount - 1;
            currentLineRenderer.SetPosition(positionIndex, pointPos);
        }


        /// <summary>
        /// adds a line to the mouse from the last mouse position.
        /// </summary>
        void PointToMousePos()
        {
            if (currentLineRenderer == null) return;

            RaycastHit hit;
            if (Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                Vector3 mousePos = hit.point;
                mousePos += hit.normal * offsetDistance;
                AddAPoint(mousePos);
            }
        }

        /// <summary>
        /// stops drawing, and check how good the player did.
        /// </summary>
        void EndDrawing()
        {
            if (currentLineRenderer != null)
            {
                if(currentSymbol == null)
                {
                    currentSymbol = letterHandler.currentSymbol;
                }
                if(letterHandler.CheakDwaingQualaty(currentLineRenderer))
                {
                    currentLineRenderer = null;
                    IsTutorualOver = true;
                    if (currentSymbol != letterHandler.currentSymbol)
                    {
                        currentSymbol = letterHandler.currentSymbol;
                        StartCoroutine(TakeScreenShot());
                        
                    }
                    if(letterHandler.GameOver())
                    {
                        OnGameOver();
                    }
                }
            }
            currentInkAmount = maxInkAmount;
        }

        /// <summary>
        /// clears all drawing segments to clear the bord.
        /// </summary>
        public void ClearDrawnSegments()
        {

            foreach (var brushInstance in drawnBrushInstances)
            {
                Destroy(brushInstance);
            }
            drawnBrushInstances.Clear();
        }


        /// <summary>
        /// Returns the player to the main world then the game is over
        /// </summary>
        private void OnGameOver(){
            SwitchScenes.SwitchToMainWorld();
        }


        /// <summary>
        /// Sets up various variables and the gamemode
        /// </summary>
        /// <param name="gameMode">the gamemode to be used</param>
        /// <param name="gameRules">the gamerules to be used</param>
        public void SetupGame(IGenericGameMode gameMode, IGameRules gameRules)
        {
            currentInkAmount = maxInkAmount;
            inkMeterSlider.maxValue = maxInkAmount;
            inkMeterSlider.value = currentInkAmount;
            beeMovement = bee.gameObject.GetComponentInChildren<BeeMovement>();
            
            letterHandler.StartGame((LettergardenGameMode)gameMode, gameRules);
        }

        /// <summary>
        /// Takes a "screenshot" of the area around the letter and instantiates the correctletterbox
        /// </summary>
        /// <returns></returns>
        public IEnumerator TakeScreenShot()
        {
            yield return new WaitForEndOfFrame (); // it must be a coroutine 

            //Sets up the bounds of the screenshot
            float width = 340;
            float height = 600;
            Vector3 worldStart = new Vector3(screenShotBounds.transform.position.x, -38 - height/2, 340 - width/2);
            Vector3 worldEnd = new Vector3(screenShotBounds.transform.position.x, -38 + height/2, 340 + width/2);
            Vector2 screenStart = Camera.main.WorldToScreenPoint(worldStart);
            Vector2 screenEnd = Camera.main.WorldToScreenPoint(worldEnd);
            width = screenStart.x - screenEnd.x;
            height = screenEnd.y - screenStart.y;
            
            //takes the screenshot
            var tex = new Texture2D ((int)System.MathF.Round(width), (int)System.MathF.Round(height), TextureFormat.RGB24, false);
            tex.ReadPixels (new Rect(screenStart.x, screenStart.y, width, height), 0, 0);
            tex.Apply();

            //Instantiates the correctletterbox
            GameObject correctLetter = Instantiate(correctLetterBox);
            correctLetter.transform.position = new Vector3(correctLetter.transform.position.x - 0.5f,correctLetter.transform.position.y, correctLetter.transform.position.z);
            CorrectLetterHandler correctLetterHandler = correctLetter.GetComponent<CorrectLetterHandler>();
            correctLetterHandler.image.texture = tex;
            
            //Replaces the font if the letter is a small A or small Å
            if(letterHandler.oldLetter == "a" || letterHandler.oldLetter == "\u00E5")
            {
                correctLetterHandler.exampleLetter.font = altFont;
            }
            correctLetterHandler.exampleLetter.text = letterHandler.oldLetter;
            ClearDrawnSegments();
        }
    }
}
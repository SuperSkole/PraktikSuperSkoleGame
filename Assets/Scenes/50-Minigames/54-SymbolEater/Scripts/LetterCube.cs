using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes._50_Minigames._54_SymbolEater.Scripts
{
    /// <summary>
    /// Manager for letter cubes in the Symbol Eater mini game.
    /// </summary>
    public class LetterCube : MonoBehaviour
    {

        [SerializeField] private TextMeshPro text;


        /// <summary>
        /// A gameobject to have the images sprite onto.
        /// </summary>
        [SerializeField] private GameObject imageObject;


        /// <summary>
        /// these 2 are used for the FindImageFromSound gamemode, with a SpriteRendere and a sprite, and a String for the current word.
        /// </summary>
        private RawImage rawImage;

        private string isCurrentWord;

        SpriteRenderer spriteRenderer;

        /// <summary>
        /// The gameboard the letter cube is connected to
        /// </summary>
        [SerializeField] private BoardController board;
        private GameObject coinPrefab;

        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material correctMaterial;
        [SerializeField] private Material incorrectMaterial;

        [SerializeField] private MeshRenderer meshRenderer;

        public bool randomizeFont = false;

        [SerializeField] List<TMP_FontAsset> fonts;

        bool readyForDeactivation = false;

        public bool active;

        private string letter;


        void Start()
        {
            rawImage = imageObject.GetComponent<RawImage>();
        }


        /// <summary>
        /// Reacts to the players stepping on it and reacts according to if the letter is the correct one
        /// </summary>
        /// <param name="other"></param>
        void OnTriggerEnter(Collider other)
        {
            // Sets the meshRenderer and the defaultmaterial if it has not been set yet
            if (meshRenderer == null)
            {
                meshRenderer = gameObject.GetComponent<MeshRenderer>();
                defaultMaterial = meshRenderer.material;
            }
            // Checks if the overlap is with the player, if the player should trigger it and if the symbol on it is the one the player is looking for
            if (!readyForDeactivation)
            {
                if (other.gameObject.tag == "Player" && active && !board.IsCorrectSymbol(letter) && !board.GetPlayer().thrown && board.GetPlayer().hasMoved)
                {
                    StartCoroutine(IncorrectGuess());
                    board.GetPlayer().IncorrectGuess();
                }
                else if (active && other.gameObject.tag == "Player" && !board.GetPlayer().thrown && board.GetPlayer().hasMoved)
                {
                    StartCoroutine(CorrectGuess());
                    Instantiate(coinPrefab);
                }
            }
        }

        /// <summary>
        /// Overload on the activate method in case it is not important whether the letter is lower case. Takes the desired letter as input
        /// </summary>
        /// <param name="letter">The letter which should be displayed</param>
        public void Activate(string letter)
        {
            Activate(letter, false);
        }

        /// <summary>
        /// Overload on the activate method in case it is not important whether the Word is lower case. Takes the desired Word as input
        /// </summary>
        /// <summary>
        /// Overload on the activate method in case it is not important whether the Word is lower case. Takes the desired Word as input
        /// </summary>
        public void ActivateImage(Texture2D texture2D, string word)
        {
            if (rawImage == null)
            {
                rawImage = imageObject.GetComponent<RawImage>();
            }
            rawImage.gameObject.SetActive(true);
            
            rawImage.texture = texture2D;

            

            letter = word;

            if (!active)
            {
                active = true;
                transform.Translate(0, 0.2f, 0);
            }
        }

        /// <summary>
        /// Same as Above but Meant for the find imageGamemode
        /// </summary>
        /// <param name="texture2D"></param>
        /// <param name="currentWord"></param>
        public void ActivateImage(Texture2D texture2D)
        {
            ActivateImage(texture2D, isCurrentWord);
        }

        public string GetLetter()
        {
            return letter;
        }

        /// <summary>
        /// Sets the letter of the letterbox and activates it by moving it upwards. If capitalization is not important it sets it to lower case half of the time randomly
        /// </summary>
        /// <param name="letter">The letter Which should be displayed</param>
        /// <param name="specific">Whether capitilzation should be preserved</param>
        public void Activate(string letter, bool specific)
        {
            //Randomly sets some letters to lower case
            int lower = Random.Range(0, 2);
            if (lower == 0 && !specific)
            {
                letter = letter.ToLower();
            }
            //Randomizes the font if the setting is turned on
            if (randomizeFont)
            {
                text.font = fonts[Random.Range(0, fonts.Count)];
            }

            //Sets up the the letter variable and the visual display of the symbol
            text.text = letter;
            this.letter = letter;
            if (!active)
            {
                active = true;
                transform.Translate(0, 0.2f, 0);
            }
        }

        /// <summary>
        /// Deactivates the letterbox by moving it back below the board and reseting the text and value of the letter variable
        /// </summary>
        public void Deactivate()
        {
            text.text = ".";
            letter = "";

            if (active)
            {
                active = false;
                transform.Translate(0, -0.2f, 0);
                readyForDeactivation = false;
            }
        }



        /// <summary>
        /// Deactivates the letterbox by moving it back below the board and reseting the Image and value of the Word variable
        /// </summary>
        public void DeactivateImage()
        {
            rawImage.texture = null;
            isCurrentWord = ".";
            
            rawImage.gameObject.SetActive(false);

            if (active)
            {
                active = false;
                transform.Translate(0, -0.2f, 0);
                readyForDeactivation = false;
            }
        }


        /// <summary>
        /// Sets the board used by the lettercube
        /// </summary>
        /// <param name="board">the board to be used</param>
        public void SetBoard(BoardController board)
        {
            this.board = board;
        }

        /// <summary>
        /// Sets the coin prefab so it can be instanciated later
        /// </summary>
        /// <param name="coinPrefab">the prefab to be created</param>
        public void SetCoin(GameObject coinPrefab)
        { 
            this.coinPrefab = coinPrefab; 
        }


        /// <summary>
        /// tells the board to replace this lettercube and deactivate it
        /// </summary>
        private void SelfDeactivate()
        {
            board.ReplaceLetter(this);
        }

        /// <summary>
        /// Changes the color of the lettercube for some time if it does not contain the correct letter
        /// </summary>
        /// <returns></returns>
        IEnumerator IncorrectGuess()
        {
            readyForDeactivation = true;
            rawImage.color = Color.red;
            meshRenderer.material = incorrectMaterial;
            yield return new WaitForSeconds(board.GetPlayer().maxIncorrectSymbolStepMoveDelayRemaining);
            rawImage.color = Color.white;
            meshRenderer.material = defaultMaterial;
            SelfDeactivate();
        }

        /// <summary>
        /// Changes the color of the lettercube if it contains the correct letter
        /// </summary>
        /// <returns></returns>
        IEnumerator CorrectGuess()
        {
            readyForDeactivation = true;
            rawImage.color = Color.green;
            meshRenderer.material = correctMaterial;
            yield return new WaitForSeconds(1);
            rawImage.color = Color.white;
            meshRenderer.material = defaultMaterial;
            SelfDeactivate();
        }
    

        /// <summary>
        /// Toggles whether the image on the letter cube is displayed
        /// </summary>
        public void ToggleImage(){
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }

    }

}
    


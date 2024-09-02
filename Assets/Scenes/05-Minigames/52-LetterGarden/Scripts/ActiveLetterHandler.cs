using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;
using UnityEngine.Splines;

namespace Scenes.Minigames.LetterGarden.Scrips
{

    public class ActiveLetterHandler : MonoBehaviour
    {
        [SerializeField] SymbolManager symbolManager;
        [SerializeField] Transform splineHolder;
        [SerializeField] BeeMovement bee;

        private List<SplineSymbolDataHolder> splines = new();
        public SplineSymbolDataHolder currentSymbol;
        private int currentSymbolIndex = 0;

        /// <summary>
        /// used for testing only!!!!
        /// </summary>
        private void Start () //TODO remove and replace when we got a loading scene
        {
            symbolManager.StartLoad();
            StartGame();
        }
        /// <summary>
        /// call to start the game
        /// </summary>
        public void StartGame()
        {
            splines = GetCapitalSymbolseToDraw(5);
            if (splines.Count <= 0) return;//end game

            currentSymbol = splines[0];
            splines.RemoveAt(0);
            bee.NextLetter(currentSymbol.splineContainer);
        }

        /// <summary>
        /// cheaks if the drawing given is good and handles sending the bee to the next letter/segment of the letter.
        /// </summary>
        /// <param name="dwaing">the line the player has drawn</param>
        public bool CheakDwaingQualaty(LineRenderer dwaing)
        {
            if (LineSegmentEvaluator.EvaluateSpline(currentSymbol.splineContainer[currentSymbolIndex],dwaing))
            {
                currentSymbolIndex++;
                if(!bee.NextSplineInLetter())
                {
                    PlayerEvents.RaiseGoldChanged(1);
                    PlayerEvents.RaiseXPChanged(1);
                    //next letter
                    currentSymbolIndex = 0;
                    if(splines.Count <= 0) return true;//end game

                    currentSymbol = splines[0];
                    splines.RemoveAt(0);
                    bee.NextLetter(currentSymbol.splineContainer);
                    return true;
                }
                //next Spline in container
                return true;
            }
            dwaing.positionCount = 0;
            return false;
        }

        /// <summary>
        /// used to get random symbols for the game form the SymbolManager.
        /// </summary>
        /// <param name="amount">the amount of symbols you want to get</param>
        /// <returns>a list of SplineSymbolDataHolders whitch have data in them</returns>
        private List<SplineSymbolDataHolder> GetCapitalSymbolseToDraw(int amount)
        {
            List<SplineSymbolDataHolder> output = new();
            for (int i = 0; i < amount; i++)
            {
                int randomIndex = Random.Range(0, SymbolManager.capitalLettersObjects.Count);
                output.Add(new(Instantiate(SymbolManager.capitalLettersObjects.ElementAt(randomIndex).Value, splineHolder),
                                            SymbolManager.capitalLetters.ElementAt(randomIndex).Value, 
                                            SymbolManager.capitalLetters.ElementAt(randomIndex).Key));
            }
            return output;
        }

        public bool GameOver()
        {
            return splines.Count == 0;
        }
    }

    public class SplineSymbolDataHolder
    {
        public GameObject splineObject;
        public SplineContainer splineContainer;
        public char Symbol;

        /// <summary>
        /// this is a class for holding information about a Symbol for LetterGarden
        /// </summary>
        /// <param name="splineObject">a refrence to the instanc of a symbol</param>
        /// <param name="splineContainer">the splineContainer for the symbol</param>
        /// <param name="Symbol">the spisific symbol(used to update player after finishing a dwaing)</param>
        public SplineSymbolDataHolder(GameObject splineObject, SplineContainer splineContainer, char Symbol)
        {
            this.splineObject = splineObject;
            this.splineContainer = splineContainer;
            this.Symbol = Symbol;
        }
    }

}
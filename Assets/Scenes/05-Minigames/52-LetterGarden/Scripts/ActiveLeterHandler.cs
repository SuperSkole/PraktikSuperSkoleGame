using Minigames.LetterGarden.Scripts;
using Scenes.Minigames.LetterGarden.Scrips;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class ActiveLeterHandler : MonoBehaviour
{
    [SerializeField] SymbolManager symbolManager;
    [SerializeField] Transform splineHolder;

    private List<SplineContainer> splines = new();
    private List<GameObject> splineObjects = new();
    private SplineContainer currentSymbol;
    private int currentSymbolIndex = 0;
    void StartGame()
    {
        GetSymbolseToDwar(5);
    }

    public void CheakDwaingQualaty(LineRenderer dwaing)
    {
        if (LineSecmentEvaluator.EvaluateSpline(currentSymbol[currentSymbolIndex],dwaing))
        {
            currentSymbolIndex++;
            if(currentSymbol.Splines.Count >= currentSymbolIndex)
            {
                //next letter
                return;
            }
            //next Spline in container
        }
    }

    void GetSymbolseToDwar(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, symbolManager.capitalLettersObjects.Count);
            splineObjects.Add(Instantiate(symbolManager.capitalLettersObjects.ElementAt(randomIndex).Value,splineHolder));
            splines.Add(symbolManager.capitalLetters.ElementAt(randomIndex).Value);
        }
    }
}

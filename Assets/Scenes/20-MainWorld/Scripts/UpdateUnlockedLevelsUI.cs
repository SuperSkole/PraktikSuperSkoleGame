using CORE.Scripts;
using Scenes._10_PlayerScene.Scripts;
using Scenes._50_Minigames.Gamemode;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateUnlockedLevelsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    private MonsterTowerSetter towerSetter= new MonsterTowerSetter();
    private SymbolEaterSetter symbolEaterSetter= new SymbolEaterSetter();
    private LetterGardenSetter letterGardenSetter= new LetterGardenSetter();
    private MiniRacingSetter racingSetter=new MiniRacingSetter();

    

    private int towerMinLevelRequired=1;
    private int symbolEaterMinLevelRequired=1;
    private int letterGardenMinLevelRequired=1;
    private int racingMinLevelRequired=1;

    private bool firstItemFound;

    public int currentPlayerLevel;

    // Start is called before the first frame update
    void Start()
    {
        //Gets the minumum level requirement for each of the minigames. 
        towerMinLevelRequired = GetMinLevelRequirements(towerSetter.gamemodes);
        symbolEaterMinLevelRequired = GetMinLevelRequirements(symbolEaterSetter.gamemodes);
        letterGardenMinLevelRequired = GetMinLevelRequirements(letterGardenSetter.gamemodes);
        racingMinLevelRequired = GetMinLevelRequirements(racingSetter.gamemodes);

        //Debug.Log("TowerMinLevel:"+towerMinLevelRequired);
        //Debug.Log("SymbolMinLevel:"+symbolEaterMinLevelRequired);
        //Debug.Log("LetterMinLevel:"+letterGardenMinLevelRequired);
        //Debug.Log("RacingMinLevel:"+racingMinLevelRequired);

    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerLevel = PlayerManager.Instance.PlayerData.CurrentLevel;
        string level1Text="Du kan Spille:";

        if(towerMinLevelRequired<=currentPlayerLevel)
        {
            level1Text += "\nMonsterTårnet";
        }

        if (letterGardenMinLevelRequired <= currentPlayerLevel)
        {
            level1Text += "\nBogstavsHaven";
        }

        if (symbolEaterMinLevelRequired <= currentPlayerLevel)
        {
            level1Text += "\nGrovæder";
        }

        if (racingMinLevelRequired <= currentPlayerLevel)
        {
            level1Text += "\nRacerSpillet";
        }

        textMesh.text = level1Text;
               
    }

    /// <summary>
    /// Gets the minimumlevel requirement for the minigame by adding plus 1 to an int variable until the first item in the gameModes list is found. 
    /// </summary>
    private int GetMinLevelRequirements(List<IGenericGameMode> gameModeList)
    {
        int minimumlevelRequired = 1;
        foreach (var item in gameModeList)
        {
            if (item != null && firstItemFound == false)
            {
                firstItemFound = true;
                break;
            }
            else if (firstItemFound == false)
            {
                minimumlevelRequired++;
            }
        }

        firstItemFound = false;

        return minimumlevelRequired;

    }



   
    
   
}

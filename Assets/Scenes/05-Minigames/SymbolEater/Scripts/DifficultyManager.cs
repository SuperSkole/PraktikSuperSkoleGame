using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum for different preset difficulty settings 
/// </summary>
public enum DiffcultyPreset {CUSTOM, EASY, MEDIUM, HARD}

/// <summary>
/// Manager for the diffculty of the Symbol Eater mini game
/// </summary>
public class DifficultyManager
{
    private BoardController boardController;

    public GameObject monsterPrefab;

    private Monster monster;


    /// <summary>
    /// Sets up the various field variables to be ready for use
    /// </summary>
    /// <param name="boardController">the active boardcontroller</param>
    /// <param name="monsterPrefab">the prefab which should be used for the monster</param>
    public void SetBoardControllerAndMonsterPrefab(BoardController boardController, GameObject monsterPrefab)
    {
        this.boardController = boardController;
        this.monsterPrefab = monsterPrefab;
        monster = this.monsterPrefab.GetComponent<Monster>();
        monster.playerObject = boardController.GetPlayer().gameObject;
    }

    /// <summary>
    /// Sets the various difficulty settings based on the given preset
    /// </summary>
    /// <param name="preset"></param>
    public void SetDifficulty(DiffcultyPreset preset)
    {
        switch(preset)
        {
            case DiffcultyPreset.CUSTOM:
                break;
            case DiffcultyPreset.EASY:
                ChangePlayerSpeed(8);
                ChangeWrongLetterMoveBlockTime(2);
                ChangeMonsterSpeed(0.5f);
                SpawnMonsters(1);
                ChangeMinAndMaxWrongLetters(5, 10);
                ChangeMinAndMaxCorrectLetters(5, 10);
                break;
            case DiffcultyPreset.MEDIUM:
                ChangePlayerSpeed(4);
                ChangeWrongLetterMoveBlockTime(4);
                ChangeMonsterSpeed(1);
                SpawnMonsters(2);
                ChangeMinAndMaxWrongLetters(10, 20);
                ChangeMinAndMaxCorrectLetters(5, 10);
                break;
            case DiffcultyPreset.HARD:
                ChangePlayerSpeed(2);
                ChangeWrongLetterMoveBlockTime(6);
                ChangeMonsterSpeed(2);
                SpawnMonsters(3);
                ChangeMinAndMaxWrongLetters(20, 40);
                ChangeMinAndMaxCorrectLetters(5, 10);
                break;
        }
    }

    /// <summary>
    /// Spawns a given number of monsters
    /// </summary>
    /// <param name="monsterNum">The number of monsters which should be spawned</param>
    public void SpawnMonsters(int monsterNum)
    {
        Player player = boardController.GetPlayer();
        List<Vector3> usedPositions = new List<Vector3>(){player.transform.position};
        //Adds the given number of monster
        for(int i = 0; i < monsterNum; i++)
        {
            Vector3 monsterPos = new Vector3(Random.Range(10, 20) + 0.5f, 0.8f, Random.Range(10, 20) + 0.5f);
            while(usedPositions.Contains(monsterPos))
            {
                monsterPos = new Vector3(Random.Range(10, 20) + 0.5f, 0.8f, Random.Range(10, 20) + 0.5f);
            }
            usedPositions.Add(monsterPos);
            boardController.InstantitateMonster(monsterPrefab, monsterPos);
        }
    }

    /// <summary>
    /// Changes the players move speed
    /// </summary>
    /// <param name="speed">the new player move speed</param>
    public void ChangePlayerSpeed(float speed)
    {
        boardController.GetPlayer().speed = speed;
    }

    /// <summary>
    /// Changes the move speed of the monster(s)
    /// </summary>
    /// <param name="speed">the new monster move speed</param>
    public void ChangeMonsterSpeed(float speed)
    {
        monster.speed = speed;
    }

    /// <summary>
    /// Changes the time the player is stuck before being able to move after hitting a lettercube with a wrong letter
    /// </summary>
    /// <param name="time">the time befere the player can move again</param>
    public void ChangeWrongLetterMoveBlockTime(float time)
    {
        boardController.GetPlayer().maxIncorrectSymbolStepMoveDelayRemaining = time;
    }

    /// <summary>
    /// Changes the minimum and maximum wrong letters on the board
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void ChangeMinAndMaxWrongLetters(int min, int max)
    {
        boardController.ChangeMinAndMaxWrongSymbols(min, max);
    }

    /// <summary>
    /// Changes the minimum and maximum correct letters on the board
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void ChangeMinAndMaxCorrectLetters(int min, int max)
    {
        boardController.ChangeMinAndMaxCorrectSymbols(min, max);
    }
}
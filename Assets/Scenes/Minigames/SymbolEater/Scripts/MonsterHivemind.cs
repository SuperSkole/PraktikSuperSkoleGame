using System.Collections.Generic;

/// <summary>
/// Controls the monsters on the board in grovæder as a group
/// </summary>
public class MonsterHivemind
{
    /// <summary>
    /// List of all monsters on the board
    /// </summary>
    public List<Monster> monsters = new List<Monster>();

    
    /// <summary>
    /// Stops movement for all monsters on the board
    /// </summary>
    public void OnGameOver(){
        foreach(Monster monster in monsters){
            monster.StopMovement();
        }
    }

    public void PauseMovement(){
        foreach(Monster monster in monsters){
            monster.StopMovement();
        }
    }

    public void StartMovement(){
        foreach(Monster monster in monsters){
            monster.StartMovement();
        }
    }
}

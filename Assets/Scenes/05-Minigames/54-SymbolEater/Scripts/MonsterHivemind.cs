using System.Collections.Generic;


namespace Scenes.Minigames.SymbolEater.Scripts
{


    /// <summary>
    /// Controls the monsters on the board in Symbol Eater as a group
    /// </summary>
    public class MonsterHivemind
    {

        public List<Monster> monsters = new List<Monster>();


        /// <summary>
        /// Stops movement for all monsters on the board
        /// </summary>
        public void OnGameOver()
        {
            foreach (Monster monster in monsters)
            {
                monster.StopMovement();
            }
        }

        /// <summary>
        /// Pauses monster movement
        /// </summary>
        public void PauseMovement()
        {
            foreach (Monster monster in monsters)
            {
                monster.StopMovement();
            }
        }

        public void StartMovement()
        {
            foreach (Monster monster in monsters)
            {
                monster.StartMovement();
            }
        }
    }
}
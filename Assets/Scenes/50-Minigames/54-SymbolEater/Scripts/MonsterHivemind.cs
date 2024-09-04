using System.Collections.Generic;


namespace Scenes._50_Minigames._54_SymbolEater.Scripts
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


        /// <summary>
        /// Increase monster speed
        /// </summary>
        public void IncreaseMonsterSpeed()
        {
            foreach (Monster monster in monsters)
            {
                monster.speed += 1; 
            }
        }

        /// <summary>
        /// reset to the default monster speed.
        /// </summary>
        public void ResetSpeed()
        {
            foreach (Monster monster in monsters)
            {
                monster.ResetMoveSpeed();
            }
        }
    }
}
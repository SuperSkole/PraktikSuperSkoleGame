

using Analytics;

namespace CORE.Scripts.Game_Rules 
{
    /// <summary>
    /// Interface for handling game rules across games
    /// </summary>
    public interface IGameRules
    {
        /// <summary>
        /// Returns the correct answer
        /// </summary>
        /// <returns><the correct answer/returns>
        public string GetCorrectAnswer();

        /// <summary>
        /// Sets the correct answer as determined by the game
        /// </summary>
        public void SetCorrectAnswer();

        /// <summary>
        /// Returns a wrong answer
        /// </summary>
        /// <returns></returns>
        public string GetWrongAnswer();

        /// <summary>
        /// Returns either the complete display text or just the specific part required
        /// </summary>
        /// <returns></returns>
        public string GetDisplayAnswer();

        public string GetSecondaryAnswer();

        /// <summary>
        /// Checks whether the symbol is the correct one
        /// </summary>
        /// <param name="symbol">the symbol to be checked</param>
        /// <returns>Whether it is the correct one</returns>
        public bool IsCorrectSymbol(string symbol);

        /// <summary>
        /// Checks whether the sequence is complete. Only used in games where an answer has multiple parts
        /// </summary>
        /// <returns>Whether the sequence is complete</returns>
        public bool SequenceComplete();

    }
}
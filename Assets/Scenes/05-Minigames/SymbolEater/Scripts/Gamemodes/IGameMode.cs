using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Interface for interaction between the board controller and the active gamemode in the Symbol Eater mini game
/// </summary>
public interface IGameMode
{
    /// <summary>
    /// Gets the list of lettercubes and the boardController from the boardcontroller
    /// </summary>
    /// <param name="letterCubes">List of lettercubes</param>
    /// <param name="board">the board connected to the lettercubes</param>
    public void SetLetterCubesAndBoard(List<LetterCube> letterCubes, BoardController board);

    /// <summary>
    /// Gets the symbols for the current game
    /// </summary>
    public void GetSymbols();

    /// <summary>
    /// Checks if the letter is the same as the correct one
    /// </summary>
    /// <param name="symbol">The letter which should be checked</param>
    /// <returns>Whether the letter is the correct one</returns>
    public bool IsCorrectSymbol(string symbol);

    /// <summary>
    /// Replaces an active lettercube with another one
    /// </summary>
    /// <param name="symbol">The symbol which should be replaced</param>
    public void ReplaceSymbol(LetterCube symbol);


    /// <summary>
    /// Sets the minimum and maximum wrong letters which appears on the board
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void SetMinAndMaxWrongSymbols(int min, int max);

    /// <summary>
    /// Sets the minimum and maximum correct letters which appears on the board
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void SetMinAndMaxCorrectSymbols(int min, int max);

    /// <summary>
    /// Sets the game rules for the game
    /// </summary>
    /// <param name="gameRules">the set of game rules which should be used</param>
    public void SetGameRules(IGameRules gameRules);

}

using CORE;
using Scenes;
using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject audioMenu;
    private PlayerControles playerControles;


    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        playerControles = new();
        if(FindObjectsOfType<MenuManager>().Length <= 1)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// showes the "pause" menu when the "P" buton is pressed
    /// </summary>
    private void OnPause()
    {
        if(!SceneManager.GetActiveScene().name.StartsWith("0"))
            transform.GetChild(0).gameObject.SetActive(true);
    }

    /// <summary>
    /// called when the player wants to tp to there house
    /// </summary>
    public void PlayerHouse()
    {
        SwitchScenes.SwitchToPlayerHouseScene();
        Back();
    }

    /// <summary>
    /// used to send the player back to the CharacterSelect scean
    /// </summary>
    public void CharacterSelect()
    {
        Back();
        ManualSave();
        Destroy(PlayerManager.Instance.SpawnedPlayer);
        Destroy(PlayerManager.Instance.gameObject);

        //SceneManager.LoadScene(SceneNames.Boot);
        SceneManager.LoadScene(SceneNames.Start);
    }

    /// <summary>
    /// saves the game
    /// </summary>
    public void ManualSave()
    {
        GameManager.Instance.SaveGame();
    }

    /// <summary>
    /// goes to the leaderboard scean
    /// </summary>
    public void Leaderboard()
    {
        SwitchScenes.SwitchToLeaderBoard();
        Back();
    }

    /// <summary>
    /// displayes the audio menu where you can change volumen
    /// </summary>
    public void Audio()
    {
        audioMenu.SetActive(!audioMenu.activeInHierarchy);
    }

    /// <summary>
    /// used if the player wants to log out of the game
    /// </summary>
    public void Logout()
    {
        ManualSave();

        AuthenticationService.Instance.SignOut();
        Destroy(PlayerManager.Instance.SpawnedPlayer);
        Destroy(PlayerManager.Instance.gameObject);
        Destroy(GameManager.Instance.gameObject);
        Back();
        SwitchScenes.SwitchToLogin();
        SceneManager.LoadSceneAsync(SceneNames.Boot, LoadSceneMode.Additive);
    }

    /// <summary>
    /// used to close the "pause" menu
    /// </summary>
    public void Back()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}

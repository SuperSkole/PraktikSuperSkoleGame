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
        DontDestroyOnLoad(gameObject);
    }

    private void OnPause()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void PlayerHouse()
    {
        SwitchScenes.SwitchToPlayerHouseScene();
        Back();
    }

    public void CharacterSelect()//need somone too look at this
    {
        Back();
        //SceneManager.LoadScene(SceneNames.Start);
    }

    public void ManualSave()
    {
        GameManager.Instance.SaveGame();
    }

    public void Leaderboard()
    {
        SwitchScenes.SwitchToLeaderBoard();
        Back();
    }

    public void Audio()
    {
        audioMenu.SetActive(!audioMenu.activeInHierarchy);
    }

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

    public void Back()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}

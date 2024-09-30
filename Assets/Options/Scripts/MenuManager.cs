using CORE;
using Scenes;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.InputSystem;

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
    }

    public void CharacterSelect()
    {
        //how do i switch to this sceen?
    }

    public void ManualSave()
    {
        GameManager.Instance.SaveGame();
    }

    public void Leaderboard()
    {
        SwitchScenes.SwitchToLeaderBoard();
    }

    public void Audio()
    {
        audioMenu.SetActive(!audioMenu.activeInHierarchy);
    }

    public void Logout()
    {
        ManualSave();
        AuthenticationService.Instance.SignOut();
        SwitchScenes.SwitchToLogin();
    }

    public void Back()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}

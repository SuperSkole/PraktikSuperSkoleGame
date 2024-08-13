using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UILoginManager : MonoBehaviour
{
    //der hvor du vælger din karakter
    [SerializeField] GameObject CharacterChoice;
    //der hvor du vælger om du laver ny karakter eller bruger gammel
    [SerializeField] GameObject SaveOrNew;
    //Login 
    [SerializeField] GameObject LogInScreen;
    //Der hvor savesene ligger
    [SerializeField] GameObject SaveScene;

    //Game verdenen
    [SerializeField] private string sceneName;

    private GameObject currentActiveScreen;


    //starter ud med login aktiveret først
    private void Awake ()
    {
        LogInScreen.SetActive(true);
        currentActiveScreen = LogInScreen;
    }

    //slukker det sidste panel der var
    private void DeativateCurrent()
    {
        if (currentActiveScreen != null)
        {
            currentActiveScreen.SetActive(false);
        }
    }

    //Vælge karakter
    public void ActivateCharacterChoice()
    {
        DeativateCurrent();
        CharacterChoice.SetActive(true);
        currentActiveScreen = CharacterChoice;
    }

    //Ny eller gammel karakter
    public void ActivateSaveOrNewScreen()
    {
        DeativateCurrent();
        SaveOrNew.SetActive(true);
        currentActiveScreen = SaveOrNew;
    }

    //Starte et save
    public void ActivateSaveSceneScreen()
    {
        Debug.log("Klikket");
        DeativateCurrent();
        SaveScene.SetActive(true);
        currentActiveScreen = SaveScene;
    }

    //til spil verdenen
    public void ChangeToGameScene()
    {
        SceneManager.LoadScene(sceneName);
    }

}

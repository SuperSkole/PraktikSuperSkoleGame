using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UILoginManager : MonoBehaviour
{
    //Bruger dette script til at håndtere UI screens og karaktervalg.

    //--UI SCREENS--

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

    //nuværende aktive UI screen
    private GameObject currentActiveScreen;

    //--karakter--

    //nuværende karaktervalg
    private CharacterChoice currentChoice;
    private string CurrentId;

    [SerializeField] Image displayImage;

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

    //FYLD LOGIN TJEK LOGIK IND HER
    public void ActivateSaveOrNewScreen()
    {
        DeativateCurrent();
        SaveOrNew.SetActive(true);
        currentActiveScreen = SaveOrNew;
    }

    //Vælge karakter
    public void ActivateCharacterChoice()
    {
        DeativateCurrent();
        CharacterChoice.SetActive(true);
        currentActiveScreen = CharacterChoice;
    }

    //Starte et save
    public void ActivateSaveScene()
    {
        DeativateCurrent();
        SaveScene.SetActive(true);
        currentActiveScreen = SaveScene;
    }

    //til spil verdenen
    public void ChangeToGameScene()
    {
        //Skal lige loade spillerens nuværende info før scenen skifter

        SceneManager.LoadScene(sceneName);
    }

}

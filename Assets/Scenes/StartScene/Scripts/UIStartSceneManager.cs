using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes.StartScene.Scripts
{
    public class UIStartSceneManager : MonoBehaviour
    {
        //Bruger dette script til at håndtere UI screens og karaktervalg.

        //--UI SCREENS--
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject characterChoiceScreen;
        [SerializeField] private GameObject loadOldSaveScreen;

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
            startScreen.SetActive(true);
            currentActiveScreen = startScreen;
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
        public void ActivateStartScreen()
        {
            DeativateCurrent();
            startScreen.SetActive(true);
            currentActiveScreen = startScreen;
        }

        //Vælge karakter
        public void ActivateCharacterChoice()
        {
            DeativateCurrent();
            characterChoiceScreen.SetActive(true);
            currentActiveScreen = characterChoiceScreen;
        }

        //Starte et save
        public void ActivateSaveScene()
        {
            DeativateCurrent();
            loadOldSaveScreen.SetActive(true);
            currentActiveScreen = loadOldSaveScreen;
        }

        //til spil verdenen
        public void ChangeToGameScene()
        {
            //Skal lige loade spillerens nuværende info før scenen skifter

            SceneManager.LoadScene(sceneName);
        }

    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.StartScene.Scripts
{
    public class SavePanel : MonoBehaviour
    {
        [SerializeField] private Image profilImage;
        [SerializeField] private Image playername;
        [SerializeField] private TextMeshProUGUI playerText;
        [SerializeField] private Image blockingImage;
        
        private Image thisImage = null;
        private Image thisName = null;
        private string thisText = null;
        private bool thisSaved = false;

        public void SetSaveData(Image image, Image playerName, string text, bool isSaved)
        {
            //UISaveManager bruger denne funktion til at udfylde SavePanelets variabler
            thisImage = image;
            thisName = playerName;
            thisText = text;
            thisSaved = isSaved;

            UpdatePanel();

        }

        public void UpdatePanel()
        {
            //Hvis saved er sand, �bnes panelet og opdatere UI save Panelet.
            if (thisSaved)
            {
                blockingImage.enabled = false;

                profilImage.sprite = thisImage.sprite;
                playername.sprite = thisName.sprite;
                playerText.text = thisText;
            }
        }
    }
}

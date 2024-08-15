using UnityEngine;
using UnityEngine.UI;

namespace Scenes.StartScene.Scripts
{
    public class UISaveManager : MonoBehaviour
    {
        [SerializeField] private SavePanel savePanelOne;
        [SerializeField] private SavePanel savePanelTwo;
        [SerializeField] private SavePanel savePanelThree;
        
        private Image imageOne = null;
        private Image nameOne = null;
        private string textOne = null;
        private bool savedOne = false;

        private Image imageTwo = null;
        private Image nameTwo = null;
        private string textTwo = null;
        private bool savedTwo = false;

        private Image imageThree = null;
        private Image nameThree = null;
        private string textThree = null;
        private bool savedThree = false;

        private void Awake()
        {
            //Udfyld variablerne her

            FillSaves();
        }

        private void FillSaves()
        {
            //send alt dataen til de tre savepanaler i inspektoren
            savePanelOne.SetSaveData(imageOne, nameOne, textOne, savedOne);
            savePanelTwo.SetSaveData(imageTwo, nameTwo, textTwo, savedTwo);
            savePanelThree.SetSaveData(imageThree, nameThree, textThree, savedThree);
        }

    }
}

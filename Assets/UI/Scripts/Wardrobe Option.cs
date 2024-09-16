using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class WardrobeOption : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] TextMeshProUGUI Name;
        [SerializeField] Image profilImage;

        //private Image profilImage;
        private string SpineName;

        Outline outlineComponent;

        private Wardrop wardrop;

        private void Awake()
        {
            wardrop = FindObjectOfType<Wardrop>();
            outlineComponent = GetComponent<Outline>();
        }

        public void Initialize(string newItemName, Sprite newImage, string newSpineName)
        {
            profilImage.sprite = newImage;
            SpineName = newSpineName;
            Name.text = newItemName;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("click");
            wardrop.Click(SpineName,this);
            outlineComponent.enabled = true;
        }

        public void UnSelect()
        {
            outlineComponent.enabled = false;
        }
    }
}

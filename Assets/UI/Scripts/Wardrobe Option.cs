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

        public string SpineName;
        public bool chosen;

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
            wardrop.Click(SpineName,this);
        }

        public void LightUp()
        {
            outlineComponent.enabled = true;
        }

        public void UnSelect()
        {
            outlineComponent.enabled = false;
        }
    }
}

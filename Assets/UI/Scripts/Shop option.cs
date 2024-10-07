using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class Shopoption : MonoBehaviour, IPointerClickHandler
    {
        private int Price;
        [SerializeField] TextMeshProUGUI priceText;
        [SerializeField] TextMeshProUGUI Name;
        [SerializeField] Image profilImage;
        //private Image profilImage;
        private string SpineName;

        public int ID;

        Image imageComponent;
        Outline outlineComponent;

        private ShopManager shopManager;

        private void Awake()
        {
            shopManager = FindObjectOfType<ShopManager>();
            imageComponent = GetComponent<Image>();
            outlineComponent = GetComponent<Outline>();
            priceText.text = Price.ToString();
        }

        public void Initialize(string newItemName, int newPrice, Sprite newImage, string newSpineName, int newID)
        {
            ID = newID;
            Price = newPrice;
            profilImage.sprite = newImage;
            SpineName = newSpineName;
            Name.text = newItemName;

            if (priceText != null)
            {
                priceText.text = Price.ToString();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            shopManager.Click(SpineName, Price, this);
            imageComponent.enabled = true;
            outlineComponent.enabled = true;
        }

        public void UnSelect()
        {
            imageComponent.enabled = false;
            outlineComponent.enabled = false;
        }

    }
}

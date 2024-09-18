using UnityEngine;
using UnityEngine.UI;

public class ButtonsStartUp : MonoBehaviour
{
    private void Awake()
    {
        var manager = GameObject.FindWithTag("Manager").GetComponent<CarShowCaseRoomManager>();
        //var self =GetComponent<CarShowCaseButtons>();
        //GetComponent<Button>().onClick.AddListener(delegate { manager.PreviewColorOfCar(self); });
        var self = GetComponent<CarColorShowCaseButtons>();
        GetComponent<Button>().onClick.AddListener(()
            => manager.PreviewColorOfCar(self));
        GetComponent<Button>().onClick.AddListener(()
            => manager.SettingButtonsUp(self));
        GetComponent<Button>().onClick.AddListener(()
            => manager.SaveMaterialName());

    }
}

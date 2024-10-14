using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Made to be attached to the color prefab for the cars
/// On awake it findes the scenemanger and den addes the right listerners for when the player clicks on a color button.
/// </summary>
public class ButtonsStartUp : MonoBehaviour
{
    private void Awake()
    {
        var manager = GameObject.FindWithTag("Manager").GetComponent<CarShowCaseRoomManager>();
        var self = GetComponent<CarColorShowCaseButtons>();
        GetComponent<Button>().onClick.AddListener(()
            => manager.PreviewColorOfCar(self));
        GetComponent<Button>().onClick.AddListener(()
            => manager.SettingButtonsUp(self));
        GetComponent<Button>().onClick.AddListener(()
            => manager.SetButtonName(gameObject));

    }
}

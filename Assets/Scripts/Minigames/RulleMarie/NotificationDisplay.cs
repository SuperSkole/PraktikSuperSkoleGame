using TMPro;
using UnityEngine;

namespace Minigames.RulleMarie
{
    public class NotificationDisplay : MonoBehaviour, INotificationDisplay
    {
        private TextMeshProUGUI notificationText;

        private void Awake()
        {
            notificationText = GetComponent<TextMeshProUGUI>();
            if (notificationText == null)
            {
                Debug.LogError("TextMeshProUGUI component is not found on the NotificationText GameObject.");
            }
        }

        public void DisplayNotification(string message)
        {
            if (notificationText != null)
            {
                notificationText.text = message;
                notificationText.gameObject.SetActive(true);
                notificationText.color = Color.black;
                CancelInvoke("HideNotification");
                Invoke("HideNotification", 4f); // Hide the notification after 2 seconds
            }
        }

        private void HideNotification()
        {
            notificationText.gameObject.SetActive(false);
        }
    }
}
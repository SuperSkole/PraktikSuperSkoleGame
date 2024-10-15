using TMPro;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;

    /// <summary>
    /// Sets the text for a chat message.
    /// </summary>
    public void SetText(string str)
    {
        messageText.text = str;
    }
}

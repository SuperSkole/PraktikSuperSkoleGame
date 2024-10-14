using LoadSave;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager Singleton;

    [SerializeField] private ChatMessage chatMessagePrefab;
    [SerializeField] private CanvasGroup chatContent;
    [SerializeField] private TMP_InputField chatInput;
    private ScrollRect scrollRect;

    public string playerName;

    /// <summary>
    /// Fetches all the relevant components.
    /// </summary>
    private void Awake()
    {
        ChatManager.Singleton = this;
        GameObject uiHolder = GameObject.Find("ChatPanel");
        chatContent = uiHolder.GetComponentInChildren<CanvasGroup>();
        chatInput = uiHolder.GetComponentInChildren<TMP_InputField>();
        scrollRect = uiHolder.GetComponentInChildren<ScrollRect>();
        GameObject originPlayer = GameObject.Find("PlayerMonster");
        playerName = originPlayer.GetComponent<PlayerData>().MonsterName;
    }

    /// <summary>
    /// Checks if the player clicks enter, if so, send a chat message.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage(chatInput.text, playerName);
            chatInput.text = "";
        }
    }

    /// <summary>
    /// Handles sending the chat message as long as it isn't null, whitespace or blank.
    /// </summary>
    public void SendChatMessage(string _message, string _fromWho = null)
    {
        if (string.IsNullOrWhiteSpace(_message)) return;

        string S = _fromWho + " > " + _message;
        SendChatMessageServerRpc(S);
    }

    /// <summary>
    /// Adds new chat messages to the chatbox.
    /// </summary>
    private void AddMessage(string msg)
    {
        ChatMessage CM = Instantiate(chatMessagePrefab, chatContent.transform);
        CM.SetText(msg);
        StartCoroutine(ScrollToBottom());
    }

    /// <summary>
    /// Scrolls to the bottom of the chatbox when receiving new mesasges
    /// </summary>
    private IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    /// <summary>
    /// Gets the server to send a chat message to players.
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void SendChatMessageServerRpc(string message)
    {
        ReceiveChatMessageClientRpc(message);
    }

    /// <summary>
    /// Receives a chat message from the server and puts it in the chatbox for the player.
    /// </summary>
    [ClientRpc]
    private void ReceiveChatMessageClientRpc(string message)
    {
        ChatManager.Singleton.AddMessage(message);
    }
}


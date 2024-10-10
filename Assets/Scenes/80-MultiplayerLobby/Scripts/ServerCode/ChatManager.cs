using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using LoadSave;
using System.Collections;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager Singleton;

    [SerializeField] ChatMessage chatMessagePrefab;
    [SerializeField] CanvasGroup chatContent;
    [SerializeField] TMP_InputField chatInput;
    ScrollRect scrollRect;

    public string playerName;

    private void Awake()
    {
        ChatManager.Singleton = this;
        GameObject uiHolder = GameObject.Find("ChatPanel");
        chatContent = uiHolder.GetComponentInChildren<CanvasGroup>();
        chatInput = uiHolder.GetComponentInChildren<TMP_InputField>();
        scrollRect = uiHolder.GetComponentInChildren<ScrollRect>();
        //chatContent = GameObject.Find("ChatContent").GetComponent<CanvasGroup>();
        //chatInput = GameObject.Find("ChatBox Input").GetComponent<TMP_InputField>();
        GameObject originPlayer = GameObject.Find("PlayerMonster");
        playerName = originPlayer.GetComponent<PlayerData>().MonsterName;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage(chatInput.text, playerName);
            chatInput.text = "";
        }
    }

    public void SendChatMessage(string _message, string _fromWho = null)
    {
        if (string.IsNullOrWhiteSpace(_message)) return;

        string S = _fromWho + " > " + _message;
        SendChatMessageServerRpc(S);
    }

    void AddMessage(string msg)
    {
        ChatMessage CM = Instantiate(chatMessagePrefab, chatContent.transform);
        CM.SetText(msg);
        StartCoroutine(ScrollToTop());
    }

    IEnumerator ScrollToTop()
    {
        yield return new WaitForEndOfFrame();
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    [ServerRpc(RequireOwnership = false)]
    void SendChatMessageServerRpc(string message)
    {
        ReceiveChatMessageClientRpc(message);
    }

    [ClientRpc]
    void ReceiveChatMessageClientRpc(string message)
    {
        ChatManager.Singleton.AddMessage(message);
    }
}


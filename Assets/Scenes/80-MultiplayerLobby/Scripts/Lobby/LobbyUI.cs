using UnityEngine;
using UnityEngine.UI;

namespace Kart
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] Button createLobbyButton;
        [SerializeField] Button joinLobbyButton;
        [SerializeField] Scenes.MultiplayerLobby.Scripts.StartClient client;
        [SerializeField] Scenes.MultiplayerLobby.Scripts.StartHost host;

        //void Awake()
        //{
        //    createLobbyButton.onClick.AddListener(CreateGame);
        //    joinLobbyButton.onClick.AddListener(JoinGame);
        //}

        void CreateGame()
        {
            client.QuickJoinGame();
            //Loader.LoadNetwork(gameScene);
        }

        void JoinGame()
        {
            host.StartHostGame();
        }
    }
}

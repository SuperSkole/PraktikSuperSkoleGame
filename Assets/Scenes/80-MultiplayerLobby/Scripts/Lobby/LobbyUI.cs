using UnityEngine;
using UnityEngine.UI;

namespace Kart
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] Button createLobbyButton;
        [SerializeField] Button joinLobbyButton;

        void Awake()
        {
            createLobbyButton.onClick.AddListener(CreateGame);
            joinLobbyButton.onClick.AddListener(JoinGame);
        }

        async void CreateGame()
        {
            await StartGame.Instance.CreateLobby();
            //Loader.LoadNetwork(gameScene);
        }

        async void JoinGame()
        {
            await StartGame.Instance.QuickJoinLobby();
        }
    }
}

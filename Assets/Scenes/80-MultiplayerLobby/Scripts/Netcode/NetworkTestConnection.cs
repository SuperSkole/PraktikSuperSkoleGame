using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class NetworkTestConnection : MonoBehaviour
    {
        static private NetworkManager m_NetworkManager;

        /// <summary>
        /// Fetches the networkmanager component when waking up
        /// </summary>
        void Awake()
        {
            m_NetworkManager = GetComponent<NetworkManager>();
        }

        /// <summary>
        /// Sets up a gui to connect to the server or host one
        /// </summary>
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!m_NetworkManager.IsClient && !m_NetworkManager.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            GUILayout.EndArea();
        }

        /// <summary>
        /// Sets what the buttons do
        /// </summary>
        static void StartButtons()
        {
            if (GUILayout.Button("Host")) m_NetworkManager.StartHost();
            if (GUILayout.Button("Client")) m_NetworkManager.StartClient();
            if (GUILayout.Button("Server")) m_NetworkManager.StartServer();
        }

        /// <summary>
        /// Sets the statue label based on which button is pressed
        /// </summary>
        static void StatusLabels()
        {
            var mode = m_NetworkManager.IsHost ?
                "Host" : m_NetworkManager.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                m_NetworkManager.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }
    }
}
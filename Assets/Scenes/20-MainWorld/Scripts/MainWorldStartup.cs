using Cinemachine;
using Scenes.PlayerScene.Scripts;
using UnityEngine;

namespace Scenes._03_MainWorld.Scripts
{
    public class MainWorldStartup : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        private GameObject spawnedPlayer;
    
        private void Start()
        {
            Debug.Log("MainWorldStartup/Start/Setting up player camera");
            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;

            //virtualCamera.Follow = spawnedPlayer.transform;
            //virtualCamera.LookAt = spawnedPlayer.transform;
        }
    }
}

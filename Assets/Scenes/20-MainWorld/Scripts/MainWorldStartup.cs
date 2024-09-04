using Cinemachine;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;

namespace Scenes._20_MainWorld.Scripts
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

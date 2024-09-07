using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Scenes._02_LoginScene.Scripts
{
    public class CloudLoginManager : MonoBehaviour
    {
        private async void Awake()
        {
            await InitializeUnityServices();
        }

        /// <summary>
        /// Initializes the Unity services.
        /// </summary>
        private async Task InitializeUnityServices()
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity services initialized.");
        }
    }
}

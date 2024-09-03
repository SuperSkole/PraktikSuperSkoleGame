using CORE;
using UnityEngine;

namespace Scenes._00_Bootstrapper
{
    /// <summary>
    /// Script for detecting the type of device the game is running on.
    /// </summary>
    public class DeviceDetector : MonoBehaviour
    {
        private DeviceType detectedDeviceType;

        /// <summary>
        /// Initializes the device type and logs the detected type.
        /// </summary>
        private void Start()
        {
            DetectDeviceType();
            HandleDeviceType();
        }

        /// <summary>
        /// Detects the device type and stores it in the detectedDeviceType field.
        /// </summary>
        private void DetectDeviceType()
        {
            detectedDeviceType = SystemInfo.deviceType;
            //Debug.Log("Detected Device Type: " + detectedDeviceType.ToString());
        }

        /// <summary>
        /// Log the detected device type TODO something with the info later
        /// </summary>
        private void HandleDeviceType()
        {
            switch (detectedDeviceType)
            {
                case DeviceType.Desktop:
                    Debug.Log("Device detected: Running on a Desktop device.");
                    GameManager.Instance.UserDevice = DeviceType.Desktop;
                    break;
                case DeviceType.Handheld:
                    Debug.Log("Device detected: Running on a Handheld device");
                    GameManager.Instance.UserDevice = DeviceType.Handheld;
                    break;
                case DeviceType.Console:
                    Debug.Log("Device detected: Running on a Console device.");
                    GameManager.Instance.UserDevice = DeviceType.Console;
                    break;
                case DeviceType.Unknown:
                default:
                    Debug.Log("Device detected: Running on an Unknown device type.");
                    GameManager.Instance.UserDevice = DeviceType.Unknown;
                    break;
            }
        }
    }
}
using CORE;
using UnityEngine;

namespace Scenes._10_PlayerScene.Scripts
{
    /// <summary>
    /// Disables the Canvas if GameManager.Instance.IsPlayerBootstrapped is true.
    /// </summary>
    public class PlayerCanvasController : MonoBehaviour
    {
        private Canvas playerCanvas;

        private void Awake()
        {
            playerCanvas = GetComponent<Canvas>();

            // Ensure the Canvas is disabled if player is bootstrapped
            if (GameManager.Instance && GameManager.Instance.IsPlayerBootstrapped)
            {
                playerCanvas.gameObject.SetActive(false);
                Debug.Log("Player UI Canvas has been automatically disabled by PlayerCanvasController.");
            }
        }
    }
}
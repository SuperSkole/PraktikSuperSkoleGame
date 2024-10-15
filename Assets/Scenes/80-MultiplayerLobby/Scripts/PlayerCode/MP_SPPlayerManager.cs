using Scenes._10_PlayerScene.Scripts;
using UnityEngine;

public class MP_SPPlayerManager : MonoBehaviour
{
    public GameObject PlayerSpawnPoint;

    // This should be implemented in a more universal way instead of just for multiplayer.
    /// <summary>
    /// Moves the SP character to a designated spot to keep it out of view.
    /// </summary>
    private void Start()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.PositionPlayerAt(PlayerSpawnPoint);

            PlayerManager.Instance.SpawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = false;
            PlayerManager.Instance.SpawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;
        }
        else
        {
            Debug.LogWarning("MP_SPPlayerManager.Start(): Player Manager is null");
        }
    }
}

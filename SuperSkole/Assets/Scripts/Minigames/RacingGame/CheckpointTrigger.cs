using UnityEngine;


public class CheckpointTrigger : MonoBehaviour
{
    public RacingGameCore racingGameCore;
    public RacingGameCore.Checkpoint checkpointType;
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    private void Awake()
    {
        // Get the SpriteRenderer component attached to this checkpoint
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ActiveCar"))
        {
            racingGameCore.CheckpointTriggered(checkpointType);
            racingGameCore.passedCheckpointTrans = transform;
        }
    }

    // Method to set visibility of the sprite renderer
    public void SetVisible(bool isVisible)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = isVisible;
        }
    }
}

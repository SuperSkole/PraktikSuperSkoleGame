using Scenes._10_PlayerScene.Scripts;
using UnityEngine;

public class FindPlayerForButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SpawnCarCloseToPlayer()
    {
        var carGO = GameObject.FindGameObjectWithTag("Car");

        // Define the spawn position near the player
        Vector3 spawnPosition = PlayerManager.Instance.SpawnedPlayer.transform.position + new Vector3(5, 0, 0);

        // Define the size of the box to check for obstacles (adjust based on your car size)
        Vector3 boxSize = new Vector3(2, 1, 4); // Width, height, length of the car

        // Check if the area is clear
        if (!Physics.CheckBox(spawnPosition, boxSize / 2, Quaternion.identity))
        {
            // The area is clear, spawn the car
            carGO.transform.position = spawnPosition;
            Debug.Log("Car spawned at a safe location.");
        }
        else
        {
            spawnPosition = PlayerManager.Instance.SpawnedPlayer.transform.position + new Vector3(-5, 0, 0);
            if (!Physics.CheckBox(spawnPosition, boxSize / 2, Quaternion.identity))
            {
                // The area is clear, spawn the car
                carGO.transform.position = spawnPosition;
                Debug.Log("Car spawned at a safe location.");
            }
            else
            {
                // The area is obstructed
                Debug.Log("Cannot spawn car, area is obstructed.");
            }
        }
    }
}

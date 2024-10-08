using Scenes._10_PlayerScene.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayerForButton : MonoBehaviour
{
    private float timer;
    [SerializeField] GameObject carGO;
    //Rigidbody carRigidbody;
    private void Start()
    {
        carGO = GameObject.FindGameObjectWithTag("Car");
        //carRigidbody = carGO.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        timer += Time.deltaTime;
    }
    public void SpawnCarCloseToPlayer()
    {
        if (timer > 1f)
        {

            // Define the spawn position near the player
            Vector3 spawnPosition = PlayerManager.Instance.SpawnedPlayer.transform.position + new Vector3(5, 0, 0);

            // Define the size of the box to check for obstacles
            Vector3 boxSize = new Vector3(2, 2, 4); // Width, height, length of the car

            // Check if the area is clear
            if (!Physics.CheckBox(spawnPosition, boxSize / 2, Quaternion.identity))
            {
                MoveCarToLocation(spawnPosition);
            }
            else
            {
                spawnPosition = PlayerManager.Instance.SpawnedPlayer.transform.position + new Vector3(-5, 0, 0);
                if (!Physics.CheckBox(spawnPosition, boxSize / 2, Quaternion.identity))
                {
                    MoveCarToLocation(spawnPosition);
                }
                else //continue the loop if you want up and down add here
                {
                    spawnPosition = PlayerManager.Instance.SpawnedPlayer.transform.position + new Vector3(0, 0, 5);
                    if (!Physics.CheckBox(spawnPosition, boxSize / 2, Quaternion.identity))
                    {
                        MoveCarToLocation(spawnPosition);
                    }
                    else
                    {
                        spawnPosition = PlayerManager.Instance.SpawnedPlayer.transform.position + new Vector3(0, 0, -5);
                        if (!Physics.CheckBox(spawnPosition, boxSize / 2, Quaternion.identity))
                        {
                            MoveCarToLocation(spawnPosition);
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Teleports the players car to a given location.
    /// </summary>
    /// <param name="spawnPosition"></param>
    private void MoveCarToLocation(Vector3 spawnPosition)
    {
        timer = 0;
        carGO.transform.position = spawnPosition;

        //This is the gold mine right here remeber to use this if there are problems with transform.position
        Physics.SyncTransforms();

    }
}

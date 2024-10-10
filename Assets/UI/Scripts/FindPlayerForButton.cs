using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class FindPlayerForButton : MonoBehaviour
{
    private float timer;
    [SerializeField] GameObject carGO;
    //Rigidbody carRigidbody;
    private void Start()
    {
        StartCoroutine(FindCar());
    }
    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (carGO != null)
        {
            if (carGO.GetComponent<PrometeoCarController>().enabled == false)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void SpawnCarCloseToPlayer()
    {
        if (timer > 1f)
        {

            // Define the spawn position near the player
            Vector3 spawnPosition = PlayerManager.Instance.SpawnedPlayer.transform.position + new Vector3(5, 0, 0);

            // Define the size of the box to check for obstacles
            Vector3 boxSizeUpDown = new Vector3(2, 2, 5); // Width, height, length of the car
            Vector3 boxSizeLeftRight = new Vector3(5, 2, 2); // length, height, width of the car

            // Check if the area is clear
            if (!Physics.CheckBox(spawnPosition, boxSizeLeftRight / 2, Quaternion.identity))
            {
                MoveCarToLocation(spawnPosition);
            }
            else
            {
                spawnPosition = PlayerManager.Instance.SpawnedPlayer.transform.position + new Vector3(-5, 0, 0);
                if (!Physics.CheckBox(spawnPosition, boxSizeLeftRight / 2, Quaternion.identity))
                {
                    MoveCarToLocation(spawnPosition);
                }
                else //continue the loop if you want up and down add here
                {
                    spawnPosition = PlayerManager.Instance.SpawnedPlayer.transform.position + new Vector3(0, 0, 5);
                    if (!Physics.CheckBox(spawnPosition, boxSizeUpDown / 2, Quaternion.identity))
                    {
                        MoveCarToLocation(spawnPosition);
                    }
                    else
                    {
                        spawnPosition = PlayerManager.Instance.SpawnedPlayer.transform.position + new Vector3(0, 0, -5);
                        if (!Physics.CheckBox(spawnPosition, boxSizeUpDown / 2, Quaternion.identity))
                        {
                            MoveCarToLocation(spawnPosition);
                        }
                    }
                }
            }
        }
    }
    private IEnumerator FindCar()
    {
        if (carGO == null)
        {
            carGO = GameObject.FindGameObjectWithTag("Car");
        }
        if (carGO != null)
        {
            Debug.Log("Found Car");
            StopCoroutine(FindCar());
        }
        yield return new WaitForSeconds(0.5f);
    }
    /// <summary>
    /// Teleports the players car to a given location.
    /// </summary>
    /// <param name="spawnPosition"></param>
    private void MoveCarToLocation(Vector3 spawnPosition)
    {
        if (carGO == null)
        {
            carGO = GameObject.FindGameObjectWithTag("Car");
        }
        timer = 0;
        carGO.transform.position = spawnPosition;

        //This is the gold mine right here remeber to use this if there are problems with transform.position
        Physics.SyncTransforms();

    }
}

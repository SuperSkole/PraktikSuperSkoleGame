using Scenes.PlayerScene.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class CarSetPlayerPos : MonoBehaviour
{
    [SerializeField] private List<GameObject> PlacementPoints = new List<GameObject>();
    private GameObject spawnedPlayer;
    public bool isDriving = false;
    // Update is called once per frame
    private void Start()
    {
        spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
    }
    void Update()
    {
        if (isDriving)
        {
            spawnedPlayer.transform.position = PlacementPoints[0].transform.position;
        }
    }

    public Transform SetTransformOfPlayer()
    {
        foreach (var item in PlacementPoints)
        {
            if (!item.GetComponent<CarPlacementPoint>().isColliding)
            {
                return item.transform;
            }
        }

        return null;
    }
    public bool ReturningPlayerPlacement()
    {
        foreach (var item in PlacementPoints)
        {
            if (!item.GetComponent<CarPlacementPoint>().isColliding)
            {
                return true;
            }
        }

        return false;
    }
}

using System.Collections.Generic;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;

namespace Scenes._20_MainWorld.Scripts.Car
{
    // TODO : Change this so it uses the Physics.CheckBox instead, Look at FindPlayerForButton for how to use
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
                spawnedPlayer.GetComponent<Rigidbody>().position = PlacementPoints[0].transform.position;
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
}

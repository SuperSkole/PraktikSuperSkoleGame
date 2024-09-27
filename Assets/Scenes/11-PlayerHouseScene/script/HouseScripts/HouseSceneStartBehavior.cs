using Cinemachine;
using Scenes._10_PlayerScene.Scripts;
using Scenes._11_PlayerHouseScene.script.CameraScripts;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.HouseScripts
{
    public class HouseSceneStartBehavior : MonoBehaviour
    {
        [SerializeField] private GameObject buildingSystemParent;
        [SerializeField] private GameObject uiBuilding;
        [SerializeField] private GameObject cameraMovement;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        private GameObject spawnedPlayer;

        // Start is called before the first frame update
        private void Start()
        {
            //Debug.Log("Trying to set player obj in house start");
            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;

            buildingSystemParent.SetActive(false);
            uiBuilding.SetActive(buildingSystemParent.activeSelf);
            cameraMovement.GetComponent<CameraMovement>().enabled = buildingSystemParent.activeSelf;
        }

        public void EnableBuildingSystem()
        {
            if (!buildingSystemParent.activeSelf)
            {
                buildingSystemParent.SetActive(true);
                cameraMovement.GetComponent<CameraMovement>().enabled = buildingSystemParent.activeSelf;
                spawnedPlayer.SetActive(false);
                virtualCamera.Follow = cameraMovement.transform;
                virtualCamera.LookAt = cameraMovement.transform;
            }
            else
            {        
                buildingSystemParent.SetActive(false);
                cameraMovement.GetComponent<CameraMovement>().enabled = buildingSystemParent.activeSelf;
                spawnedPlayer.SetActive(true);
                virtualCamera.Follow = spawnedPlayer.transform;
                virtualCamera.LookAt = spawnedPlayer.transform;
            }

            uiBuilding.SetActive(buildingSystemParent.activeSelf);
        }
    }
}

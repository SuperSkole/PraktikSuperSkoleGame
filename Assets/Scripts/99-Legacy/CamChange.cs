using _99_Legacy.Vehicle;
using Cinemachine;
using UnityEngine;

namespace _99_Legacy
{
    public class CamChange : MonoBehaviour
    {
        public int camAngleDif;
        //public Transform player; // Assign your player GameObject in the Inspector
        public Transform car;    // Assign your car GameObject in the Inspector
        private CarController carController;

        public GameObject camera_one;

        Transform initialAngle;

        Quaternion camPlayer;
        Quaternion camCar;



        // Start is called before the first frame update
        void Start()
        {
            SetupCamera();
        }

        private void SetupCamera()
        {
            initialAngle = camera_one.transform;
            camAngleDif = 25;
            camPlayer = Quaternion.Euler(initialAngle.rotation.eulerAngles.x - camAngleDif,
                initialAngle.rotation.eulerAngles.y,
                initialAngle.rotation.eulerAngles.z);

            camCar = Quaternion.Euler(initialAngle.rotation.eulerAngles.x + camAngleDif,
                initialAngle.rotation.eulerAngles.y,
                initialAngle.rotation.eulerAngles.z);
        }

        /// <summary>
        /// Maybe Fix this later with camera angels?
        /// </summary>
        /// <param name="player"></param>
        public void ChangeCamToPlayer(Transform player)
        {
            camera_one.GetComponent<CinemachineVirtualCamera>().Follow = player;
            camera_one.GetComponent<CinemachineVirtualCamera>().LookAt = player;
            camera_one.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 40;
            camera_one.transform.rotation = initialAngle.transform.rotation;
            camera_one.transform.rotation = Quaternion.Euler(initialAngle.rotation.eulerAngles.x - camAngleDif,
                initialAngle.rotation.eulerAngles.y,
                initialAngle.rotation.eulerAngles.z); 
        }

        public void ChangeCamToCar()
        {
            camera_one.GetComponent<CinemachineVirtualCamera>().Follow = car;
            camera_one.GetComponent<CinemachineVirtualCamera>().LookAt = car;
            camera_one.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 100;
            if(initialAngle == null)
            {
                SetupCamera();
            }
            camera_one.transform.rotation = Quaternion.Euler(initialAngle.rotation.eulerAngles.x + camAngleDif, 
                initialAngle.rotation.eulerAngles.y, 
                initialAngle.rotation.eulerAngles.z);
        }

    }
}

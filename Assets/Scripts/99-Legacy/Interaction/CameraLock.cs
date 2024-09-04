using UnityEngine;

namespace _99_Legacy.Interaction
{
    public class CameraLock : MonoBehaviour
    {
        void Update()
        {
            // Lock the rotation of the camera on the X and Y axes
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        }
    }
}
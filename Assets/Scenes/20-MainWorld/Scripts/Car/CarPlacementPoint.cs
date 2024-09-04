using UnityEngine;

namespace Scenes._20_MainWorld.Scripts.Car
{
    public class CarPlacementPoint : MonoBehaviour
    {
        public bool isColliding { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            isColliding = true;
        }
        private void OnTriggerExit(Collider other)
        {
            isColliding = false;
        }
    }
}

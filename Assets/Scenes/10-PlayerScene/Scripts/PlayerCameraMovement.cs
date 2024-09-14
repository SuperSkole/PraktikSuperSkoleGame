using UnityEngine;

namespace Scenes._10_PlayerScene.Scripts
{
    public class PlayerCameraMovement : MonoBehaviour
    {

        [SerializeField] private bool cameraMoveWithMouse;
        [SerializeField] private bool cameraMoveWithDrag;

        [SerializeField] private float turnSpeed;
        private Rigidbody rb;
        // Start is called before the first frame update
        void Start()
        {
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
            rb = GetComponent<Rigidbody>();


        }

        // Update is called once per frame
        void FixedUpdate()
        {
            float y = Input.GetAxis("Mouse X") * turnSpeed;
            if (cameraMoveWithMouse)
            {

                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + y, 0);
            }
            if (cameraMoveWithDrag)
            {
                if (Input.GetMouseButton(1))
                {
                    rb.rotation = Quaternion.Euler(0, transform.eulerAngles.y + y, 0);
                }
            }
        }

    }
}

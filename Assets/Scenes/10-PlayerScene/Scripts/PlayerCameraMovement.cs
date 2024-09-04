using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{

    [SerializeField] private bool cameraMoveWithMouse;
    [SerializeField] private bool cameraMoveWithDrag;

    [SerializeField] private float turnSpeed;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;



    }

    // Update is called once per frame
    void Update()
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
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + y, 0);
            }
        }
    }

}

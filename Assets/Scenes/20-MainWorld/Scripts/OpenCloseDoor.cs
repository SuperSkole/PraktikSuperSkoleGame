using System.Collections;
using UnityEngine;

public class OpenCloseDoor : MonoBehaviour
{
    public void OpenDoor()
    {
        StartCoroutine(DoorMovement(true));
        //transform.localRotation = Quaternion.Euler(0,90f,0);
        //Debug.Log("OpenCloseDoor/OpenDoor");
    }
    public void CloseDoor()
    {
        StartCoroutine(DoorMovement(false));
    }

    private IEnumerator DoorMovement(bool open)
    {
        // The rotation angle depending on whether the door is opening or closing
        Quaternion targetRotation = open ? Quaternion.Euler(0f, 90f, 0f) : Quaternion.Euler(0f, 0f, 0f);

        // Store the initial rotation at the start of the coroutine
        Quaternion initialRotation = transform.localRotation;

        // The time it takes for the door to fully open or close
        float duration = 1.0f;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            // Increase elapsed time by the time between frames
            elapsedTime += Time.deltaTime;

            // Interpolate between the initial and target rotation based on the elapsed time
            transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);

            // Yield and wait for the next frame
            yield return null;
        }

        // Ensure the final rotation is exactly the target rotation
        transform.localRotation = targetRotation;
    }

    //private IEnumerator doorMovement(bool open)
    //{
    //    float yHolder = transform.localRotation.y;
    //    Quaternion yRotMax = Quaternion.Euler(0f, 90f, 0f);
    //    Quaternion yRotMin = Quaternion.Euler(0f, 0f, 0f);
    //    if (open)
    //    {
    //        while(transform.localRotation.y <= yRotMax.y && open)
    //        {
    //            yield return new WaitForSeconds(0.01f);
    //            yHolder++;
    //            Vector3 rot = new Vector3(transform.localRotation.x,
    //                transform.localRotation.y + yHolder,
    //                transform.localRotation.z);
    //            transform.localRotation = Quaternion.Euler(rot);

    //        }
    //    }
    //    else if (!open)
    //    {
    //        while (transform.localRotation.y >= yRotMin.y && !open)
    //        {
    //            yield return new WaitForSeconds(0.01f);
    //            yHolder--;
    //            Vector3 rot = new Vector3(transform.localRotation.x,
    //                transform.localRotation.y + yHolder,
    //                transform.localRotation.z);
    //            transform.localRotation = Quaternion.Euler(rot);
    //        }
    //    }
    //}
}

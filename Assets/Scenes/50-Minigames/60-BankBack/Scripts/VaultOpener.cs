using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultOpener : MonoBehaviour
{
    [SerializeField] private GameObject codeBlock;
    [SerializeField] private GameObject vaultHandle;
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private Transform codeBlockTransform;
    private Vector3 codeBlockDestination;
    private bool moving = false;
    private bool startMove = true;
    float speed = 0;
    float handleSpeed = 200;
    float codeBlockSpeed = 0.1f;
    float moved;

    /// <summary>
    /// Moves the vault door and rotates the handles
    /// </summary>
    void Update()
    {
        if(startMove)
        {
            if(codeBlockDestination == Vector3.zero || codeBlockDestination == null)
            {
                codeBlockDestination = new Vector3(vaultHandle.transform.position.x, vaultHandle.transform.position.y, vaultHandle.transform.position.z);
            }
            codeBlockTransform.position = Vector3.MoveTowards(codeBlockTransform.position, codeBlockDestination, codeBlockSpeed * Time.deltaTime);
            if(codeBlock.transform.position == codeBlockDestination)
            {
                startMove = false;
                moving = true;
            }
        }
        if(moving)
        {
            Vector3 velocity = new Vector3(speed * Time.deltaTime, 0, 0);
            moved += velocity.x;
            transform.RotateAround(rotationPoint.position, Vector3.down, velocity.x);
            velocity = new Vector3(handleSpeed * Time.deltaTime, 0, 0);
            //vaultHandle.transform.RotateAround(vaultHandle.transform.position, Vector3.forward, velocity.x);
            Vector3 eulers = vaultHandle.transform.localRotation.eulerAngles;
            vaultHandle.transform.localRotation = Quaternion.Euler(new Vector3(velocity.x, eulers.y, eulers.z + eulers.x));
            speed += 0.01f;
            //Stops movement after a bit
            if(moved >= 100)
            {
                moving = false;
            }
        }
    }

    /// <summary>
    /// Starts the movement of the vault door
    /// </summary>
    public void StartMove()
    {
        moving = true;
        codeBlock.SetActive(false);
    }
}

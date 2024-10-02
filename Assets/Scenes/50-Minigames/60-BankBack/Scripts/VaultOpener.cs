using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultOpener : MonoBehaviour
{
    [SerializeField] private GameObject codeBlock;
    [SerializeField] private GameObject vaultHandle;

    private bool moving = false;
    float speed = 0;
    float handleSpeed = 100;
    float moved;

    /// <summary>
    /// Moves the vault door and rotates the handles
    /// </summary>
    void Update()
    {
        if(moving)
        {
            Vector3 velocity = new Vector3(speed * Time.deltaTime, 0, 0);
            moved += velocity.x;
            transform.Translate(velocity);
            velocity = new Vector3(handleSpeed * Time.deltaTime, 0, 0);
            vaultHandle.transform.RotateAround(vaultHandle.transform.position, Vector3.forward, velocity.x);
            speed += 0.001f;
            //Stops movement after a bit
            if(moved >= 10)
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

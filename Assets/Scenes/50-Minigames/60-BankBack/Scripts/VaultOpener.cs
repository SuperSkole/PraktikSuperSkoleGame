using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultOpener : MonoBehaviour
{
    [SerializeField] private GameObject codeBlock;
    [SerializeField] private GameObject vaultHandle;
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private Transform codeBlockTransform;
    [SerializeField] private AudioSource handleAudioSource;
    [SerializeField] private AudioClip handleSound;
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip doorSound;
    private Vector3 codeBlockDestination;
    private bool moving = false;
    private bool startMove = false;
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
            if(!handleAudioSource.isPlaying)
            {
                handleAudioSource.PlayOneShot(handleSound);
            }
            Vector3 velocity = new Vector3(handleSpeed * Time.deltaTime, 0, 0);
            Vector3 eulers = vaultHandle.transform.localRotation.eulerAngles;
            vaultHandle.transform.localRotation = Quaternion.Euler(new Vector3(velocity.x, eulers.y, eulers.z + eulers.x));
            if(codeBlockDestination == Vector3.zero || codeBlockDestination == null)
            {
                codeBlockDestination = new Vector3(vaultHandle.transform.position.x + 0.01f, vaultHandle.transform.position.y - 0.04f, vaultHandle.transform.position.z - 0.39f);
            }
            codeBlockTransform.position = Vector3.MoveTowards(codeBlockTransform.position, codeBlockDestination, codeBlockSpeed * Time.deltaTime);
            codeBlockSpeed += 0.1f;
            if(codeBlock.transform.position == codeBlockDestination)
            {
                startMove = false;
                moving = true;
                doorAudioSource.PlayOneShot(doorSound);
            }
        }
        if(moving)
        {
            Vector3 velocity = new Vector3(speed * Time.deltaTime, 0, 0);
            moved += velocity.x;
            transform.RotateAround(rotationPoint.position, Vector3.down, velocity.x);
            
            speed += 0.1f;
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
        startMove = true;
    }
}

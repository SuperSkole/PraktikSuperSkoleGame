using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkTestTransform : NetworkBehaviour
{
    /// <summary>
    /// If is server, spin the player a bunch to see they work
    /// </summary>
    void Update()
    {
        if (IsServer)
        {
            float theta = Time.frameCount / 10.0f;
            transform.position = new Vector3((float)Math.Cos(theta), 0.0f, (float)Math.Sin(theta));
        }
    }
}
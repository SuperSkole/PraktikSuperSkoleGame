using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using static UnityEngine.CullingGroup;

namespace HelloWorld
{
    public class NetworkTestPlayer : NetworkBehaviour
    {
        List<Color> colorList = new() { Color.red, Color.blue, Color.cyan, Color.green, Color.grey, Color.yellow, Color.magenta };

        Color color = Color.red;
        Color baseColor = new Color();
        SpriteRenderer sprite;

        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public NetworkVariable<FixedString32Bytes> colorPick = new NetworkVariable<FixedString32Bytes>();
        public NetworkVariable<Color> colorUsed = new NetworkVariable<Color>();

        /// <summary>
        /// Sets up the player when they spawn in, on all clients
        /// </summary>
        public override void OnNetworkSpawn()
        {
            sprite = GetComponent<SpriteRenderer>();
            colorUsed.OnValueChanged += ColorChanged;
            if (IsServer)
            {
                if (colorUsed.Value == null || colorUsed.Value == Color.white || colorUsed.Value == baseColor)
                {
                    color = colorList[Random.Range(0, colorList.Count)];
                    colorUsed.Value = color;
                }
                sprite.color = colorUsed.Value;
            }

            if (IsClient)
            {
                if(!(colorUsed.Value == null || colorUsed.Value == Color.white || colorUsed.Value == baseColor))
                {
                    sprite.color = colorUsed.Value;
                }
            }
        }

        /// <summary>
        /// When removing from network, remove the call
        /// </summary>
        public override void OnNetworkDespawn()
        {
            colorUsed.OnValueChanged -= ColorChanged;
        }

        /// <summary>
        /// Change the colors
        /// </summary>
        /// <param name="previousColor">Former color</param>
        /// <param name="currentColor">Current color</param>
        public void ColorChanged(Color previousColor, Color currentColor)
        {
            if(previousColor != currentColor)
            {
                sprite.color = currentColor;
            }
        }

        /// <summary>
        /// Calls for the player's color to change from the server
        /// </summary>
        /// <param name="newColor">The new color</param>
        [ServerRpc(RequireOwnership = false)]
        public void ServerUpdateColorServerRPC(Color newColor)
        {
            sprite.color = newColor;
            //colorChange.ColorChange(newColor);
            UpdateColorClientRPC(newColor);
        }

        /// <summary>
        /// Changes the players colors on the client
        /// </summary>
        /// <param name="newColor">The new color</param>
        [ClientRpc]
        void UpdateColorClientRPC(Color newColor)
        {
            //colorChange.ColorChange(newColor);
            sprite.color = newColor;
        }

        /// <summary>
        /// When the color value is changed, call to the server
        /// </summary>
        /// <param name="previous">The former color</param>
        /// <param name="current">The new color</param>
        public void OnStateChanged(Color previous, Color current)
        {
            if(current != previous)
                ServerUpdateColorServerRPC(current);
        }

        /// <summary>
        /// Requests to move
        /// </summary>
        public void Move()
        {
            SubmitPositionRequestServerRpc();
        }

        /// <summary>
        /// Tells serverside to do movement
        /// </summary>
        [ServerRpc]
        void SubmitPositionRequestServerRpc()
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }

        /// <summary>
        /// Gets random new spot to be in
        /// </summary>
        /// <returns></returns>
        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        /// <summary>
        /// Updates the transform position
        /// </summary>
        void Update()
        {
            transform.position = Position.Value;
        }
    }
}

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
    public class HelloWorldPlayer : NetworkBehaviour
    {
        private Dictionary<string, int> colorMap = new Dictionary<string, int>
        {
            { "orange", 0 },
            { "blue", 1 },
            { "red", 2 },
            { "green", 3 },
            {"white", 4 },
            {"pink", 5 }
        };

        List<Color> colorList = new() { Color.red, Color.blue, Color.cyan, Color.green, Color.grey, Color.yellow, Color.magenta };

        Color color = Color.red;
        Color baseColor = new Color();
        SpriteRenderer sprite;

        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public NetworkVariable<FixedString32Bytes> colorPick = new NetworkVariable<FixedString32Bytes>();
        public NetworkVariable<Color> colorUsed = new NetworkVariable<Color>();
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

        public override void OnNetworkDespawn()
        {
            colorUsed.OnValueChanged -= ColorChanged;
        }

        public void ColorChanged(Color previousColor, Color currentColor)
        {
            if(previousColor != currentColor)
            {
                sprite.color = currentColor;
            }
        }






        [ServerRpc(RequireOwnership = false)]
        public void ServerUpdateColorServerRPC(Color newColor)
        {
            sprite.color = newColor;
            //colorChange.ColorChange(newColor);
            UpdateColorClientRPC(newColor);
        }

        [ClientRpc]
        void UpdateColorClientRPC(Color newColor)
        {
            //colorChange.ColorChange(newColor);
            sprite.color = newColor;
        }

        public void OnStateChanged(Color previous, Color current)
        {
            if(current != previous)
                ServerUpdateColorServerRPC(current);
        }

        public void Move()
        {
            SubmitPositionRequestRpc();
        }

        [Rpc(SendTo.Server)]
        void SubmitPositionRequestRpc(RpcParams rpcParams = default)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            transform.position = Position.Value;
        }
    }
}

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
    public class HelloWorldPlayer : NetworkBehaviour // TODO: Change color by sending an int value to the manager
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

        //ColorChanging colorChange;
        Color color = Color.red;
        SpriteRenderer sprite;

        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public NetworkVariable<FixedString32Bytes> colorPick = new NetworkVariable<FixedString32Bytes>();
        public NetworkVariable<Color> colorUsed = new NetworkVariable<Color>();
        public override void OnNetworkSpawn()
        {
            //colorPick.OnValueChanged += OnStateChanged;
            sprite = GetComponent<SpriteRenderer>();
            if (IsOwner)
            {
                Move();
                #region Set up color
                //ISkeletonComponent skeleton = GetComponentInChildren<ISkeletonComponent>();
                //if (skeleton == null)
                //{
                //    Debug.LogError("PlayerManager.SetupPlayer(): " +
                //                   "ISkeleton component not found on spawned player.");
                //    return;
                //}
                //colorChange = GetComponent<ColorChanging>();
                //colorChange.SetSkeleton(skeleton);
                
                //GameObject originPlayer = GameObject.Find("PlayerMonster");
                //color = originPlayer.GetComponent<PlayerData>().MonsterColor;
                //color = "pink";
                //colorChange.ColorChange(color);
                Debug.Log("test 1");
                //ToggleServerRpc(color);
                //colorPick.Value = color;
                //colorChange.ColorChange(colorPick.Value.ToString());
                Debug.Log("test 2");
                //GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                colorUsed.OnValueChanged += OnStateChanged;
                color = colorList[Random.Range(0, colorList.Count)];
                colorUsed.Value = color;
                //ServerUpdateColor(color);
                #endregion
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
            ServerUpdateColorServerRPC(current);
        }

        public override void OnNetworkDespawn()
        {
            //colorPick.OnValueChanged -= OnStateChanged;
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

        //public void OnStateChanged(FixedString32Bytes previous, FixedString32Bytes current)
        //{
        //    color = current.ToString();
        //    if (color == null)
        //        color = "green";
        //    colorChange.ColorChange(color);
        //}

        //[Rpc(SendTo.Server)]
        //public void ToggleServerRpc(string input)
        //{
        //    colorPick.Value = input;
        //}


    }
}

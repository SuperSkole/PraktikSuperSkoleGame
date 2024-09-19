using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

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
        ColorChanging colorChange;
        string color;


        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        //public NetworkVariable<string> randombullshit = new NetworkVariable<string>();
        public NetworkVariable<int> colorPick = new NetworkVariable<int>(); // This somehow break stuff !!DO NOT USE!!
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
                #region Set up color
                ISkeletonComponent skeleton = GetComponentInChildren<ISkeletonComponent>();
                if (skeleton == null)
                {
                    Debug.LogError("PlayerManager.SetupPlayer(): " +
                                   "ISkeleton component not found on spawned player.");
                    return;
                }
                colorChange = GetComponent<ColorChanging>();
                colorChange.SetSkeleton(skeleton);

                GameObject originPlayer = GameObject.Find("PlayerMonster");
                color = originPlayer.GetComponent<PlayerData>().MonsterColor;
                colorChange.ColorChange(color);
                #endregion
            }
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
            //Position.Value = randomPosition;
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            //transform.position = Position.Value;
        }
        
        
    }
}

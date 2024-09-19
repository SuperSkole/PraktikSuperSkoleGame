using CORE;
using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using Spine;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using LoadSave;
using JetBrains.Annotations;

public class RpcTest : NetworkBehaviour
{
    //public NetworkVariable<string> colorPick = new NetworkVariable<string>(); // This somehow break stuff !!DO NOT USE!!
    ColorChanging colorChange;
    string color;

    public override void OnNetworkSpawn()
    {
        if (!IsServer && IsOwner) //Only send an RPC to the server from the client that owns the NetworkObject of this NetworkBehaviour instance
        {
            ServerOnlyRpc(0, NetworkObjectId);
            //State.OnValueChanged += OnStateChanged;
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
            //colorChange.ColorChange(color);
            //ToggleServerRpc();
            #endregion
        }
    }

    //public void OnStateChanged(string previous, string current)
    //{
    //    color = current;
    //    colorChange.ColorChange(color);
    //}

    //[Rpc(SendTo.Server)]
    //public void ToggleServerRpc()
    //{
    //    State.Value = color;
    //}

    [Rpc(SendTo.ClientsAndHost)]
    void ClientAndHostRpc(int value, ulong sourceNetworkObjectId)
    {
        Debug.Log($"Client Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
        if (IsOwner) //Only send an RPC to the owner of the NetworkObject
        {
            ServerOnlyRpc(value + 1, sourceNetworkObjectId);
        }
    }

    [Rpc(SendTo.Server)]
    void ServerOnlyRpc(int value, ulong sourceNetworkObjectId)
    {
        Debug.Log($"Server Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
        ClientAndHostRpc(value, sourceNetworkObjectId);
    }
}

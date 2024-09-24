using CORE;
using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class MP_Movement : NetworkBehaviour
{
    ColorChanging colorChange;

    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public NetworkVariable<FixedString32Bytes> colorPick = new NetworkVariable<FixedString32Bytes>();
    public override void OnNetworkSpawn()
    {
        ISkeletonComponent skeleton = GetComponentInChildren<ISkeletonComponent>();
        colorChange = GetComponent<ColorChanging>();
        colorChange.SetSkeleton(skeleton);
        if (skeleton == null)
        {
            Debug.LogError("PlayerManager.SetupPlayer(): " +
                           "ISkeleton component not found on spawned player.");
            return;
        }
        if (IsServer)
        {
            if (MonsterColorIsSet())
            {
                colorChange.ColorChange(colorPick.Value.ToString());
            }
        }

        if (IsClient)
        {
            if (IsOwner)
            {
                GameObject originPlayer = GameObject.Find("PlayerMonster");
                colorPick.Value = originPlayer.GetComponent<PlayerData>().MonsterColor;
            }
            if (MonsterColorIsSet())
            {
                colorChange.ColorChange(colorPick.Value.ToString());
            }
        }
    }

    bool MonsterColorIsSet()
    {
        string color = colorPick.Value.ToString();
        if (color != "white" && color != "" && color != null)
            return true;
        return false;
    }

    public override void OnNetworkDespawn()
    {
        //colorUsed.OnValueChanged -= ColorChanged;
    }

    //public void ColorChanged(Color previousColor, Color currentColor)
    //{
    //    //if (previousColor != currentColor)
    //    //{
    //    //    sprite.color = currentColor;
    //    //}
    //}

    //[ServerRpc(RequireOwnership = false)]
    //public void ServerUpdateColorServerRPC(Color newColor)
    //{
    //    //sprite.color = newColor;
    //    //colorChange.ColorChange(newColor);
    //    UpdateColorClientRPC(newColor);
    //}

    //[ClientRpc]
    //void UpdateColorClientRPC(Color newColor)
    //{
    //    //colorChange.ColorChange(newColor);
    //    //sprite.color = newColor;
    //}

    //public void OnStateChanged(Color previous, Color current)
    //{
    //    if (current != previous)
    //        ServerUpdateColorServerRPC(current);
    //}

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

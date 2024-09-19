using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayerHandler : MonoBehaviour
{
    public NetworkVariable<string> colorPick = new NetworkVariable<string>();
    ColorChanging colorChange;
    string color;


}

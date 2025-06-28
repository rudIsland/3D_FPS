using Fusion;
using UnityEngine;


enum MyButtons
{
    Forward = 0,
    Backward = 1,
    Left = 2,
    Right = 3,
    Jump=4
}

public struct NetworkInputData : INetworkInput
{
    public Vector3 aimDirection;
    public NetworkButtons Buttons;
}

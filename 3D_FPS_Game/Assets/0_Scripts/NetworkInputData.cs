using Fusion;
using UnityEngine;


public struct NetworkInputData : INetworkInput
{
    public const byte BUTTON_JUMP = 0;
    public const byte BUTTON_Attack = 1;

    public Vector3 mvDir; //움직임
    public NetworkButtons buttons;
    public Vector2 aimDir; //바라보는 방향
}

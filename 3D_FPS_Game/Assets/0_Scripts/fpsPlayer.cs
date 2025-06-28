using Fusion;
using UnityEngine;
using UnityEngine.Windows;

public class fpsPlayer : NetworkBehaviour
{
    [SerializeField] private float moveSpeed=5.0f;
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        NetworkButtons buttons = default;

        if (GetInput<NetworkInputData>(out var input))
        {
            buttons = input.Buttons;
        }

        // compute pressed/released state
        var pressed = buttons.GetPressed(ButtonsPrevious);
        var released = buttons.GetReleased(ButtonsPrevious);

        // store latest input as 'previous' state we had
        ButtonsPrevious = buttons;

        // movement (check for down)
        var vector = default(Vector3);

        if (buttons.IsSet(MyButtons.Forward)) { vector.z += 1; }
        if (buttons.IsSet(MyButtons.Backward)) { vector.z -= 1; }

        if (buttons.IsSet(MyButtons.Left)) { vector.x -= 1; }
        if (buttons.IsSet(MyButtons.Right)) { vector.x += 1; }

        if (vector != Vector3.zero)
            Debug.Log($"[Input] Move Vector: {vector}");

        if (pressed.IsSet(MyButtons.Jump))
        {
            Debug.Log("[Input] Jump Pressed");
        }

        DoMove(vector);
    }

    void DoMove(Vector3 vector)
    {
        Vector3 MoveDir = vector.normalized*moveSpeed*Time.timeScale;
        characterController.Move(MoveDir);
    }

    void DoJump()
    {
        // dummy method with no logic in it
    }
}

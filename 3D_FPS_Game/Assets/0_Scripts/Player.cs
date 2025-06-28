using Fusion;
using UnityEngine;
using UnityEngine.Windows;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController netCharacterController;

    [SerializeField] private Transform cameraRoot; // MainCamera�� �θ�
    [SerializeField] private Camera playerCamera; // MainCamera ����
    [SerializeField] private float moveSpeed=2.5f;
    [SerializeField] private float mouseSensitivity = 0.2f;

    private float pitch;


    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            playerCamera.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            playerCamera.enabled = false;
        }

    }

    private void Awake()
    {
        netCharacterController = GetComponent<NetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            // ī�޶� ���� �̵� ���� ��� (XZ ���)
            Vector3 forward = Vector3.ProjectOnPlane(cameraRoot.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(cameraRoot.right, Vector3.up).normalized;
            Vector3 moveDir = (right * data.mvDir.x + forward * data.mvDir.y).normalized;

            // ȸ�� �����ؼ� move �Լ��� ����
            netCharacterController.Move(
                moveDir * moveSpeed * Runner.DeltaTime,
                data.aimDir * mouseSensitivity,
                cameraRoot // pitch �����
                );
            if (data.buttons.IsSet(NetworkInputData.BUTTON_JUMP))
                netCharacterController.Jump();
        }
    }

}

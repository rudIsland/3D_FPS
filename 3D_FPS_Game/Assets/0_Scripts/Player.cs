using Fusion;
using UnityEngine;
using UnityEngine.Windows;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController netCharacterController;

    [SerializeField] private Transform cameraRoot; // MainCamera의 부모
    [SerializeField] private Camera playerCamera; // MainCamera 참조
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
            // 카메라 기준 이동 방향 계산 (XZ 평면)
            Vector3 forward = Vector3.ProjectOnPlane(cameraRoot.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(cameraRoot.right, Vector3.up).normalized;
            Vector3 moveDir = (right * data.mvDir.x + forward * data.mvDir.y).normalized;

            // 회전 포함해서 move 함수에 전달
            netCharacterController.Move(
                moveDir * moveSpeed * Runner.DeltaTime,
                data.aimDir * mouseSensitivity,
                cameraRoot // pitch 제어용
                );
            if (data.buttons.IsSet(NetworkInputData.BUTTON_JUMP))
                netCharacterController.Jump();
        }
    }

}

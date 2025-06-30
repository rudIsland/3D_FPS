using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using UnityEngine;
using UnityEngine.Windows;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController netCharacterController;

    [SerializeField] private Transform cameraRoot; // MainCamera의 부모
    [SerializeField] private Camera playerCamera;
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
        MoveAndRotate();
    }



 
    private void MoveAndRotate() //이동과 회전 함수
    {
        if (GetInput(out NetworkInputData data))
        {
            //foward는 카메라가 바라보는 방향으로 이동하기 위함
            Vector3 forward = new Vector3(cameraRoot.forward.x, 0f, cameraRoot.forward.z); //앞뒤 방향 벡터
            Vector3 right = new Vector3(cameraRoot.right.x, 0f, cameraRoot.right.z); //양옆 방향벡터
            //movdeDir은 forward가 방향벡터이므로 XZ축이 지속해서 양수인 것을 막기위해 입력값을 곱해주기 위함.
            Vector3 moveDir = (forward * data.mvDir.z + right * data.mvDir.x).normalized;

            // 회전 포함해서 move 함수에 전달
            netCharacterController.Move(
                (moveDir * moveSpeed * Runner.DeltaTime).normalized, //이동벡터 * 속도 * 프레임
                data.aimDir * mouseSensitivity, //XZ회전 벡터
                cameraRoot // Y축 회전 pitch 제어용
                );
            if (data.buttons.IsSet(NetworkInputData.BUTTON_JUMP))
                netCharacterController.Jump();
        }
    }
}

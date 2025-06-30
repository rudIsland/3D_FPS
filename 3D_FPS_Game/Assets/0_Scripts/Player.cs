using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using UnityEngine;
using UnityEngine.Windows;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController netCharacterController;

    [SerializeField] private Transform cameraRoot; // MainCamera�� �θ�
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



 
    private void MoveAndRotate() //�̵��� ȸ�� �Լ�
    {
        if (GetInput(out NetworkInputData data))
        {
            //foward�� ī�޶� �ٶ󺸴� �������� �̵��ϱ� ����
            Vector3 forward = new Vector3(cameraRoot.forward.x, 0f, cameraRoot.forward.z); //�յ� ���� ����
            Vector3 right = new Vector3(cameraRoot.right.x, 0f, cameraRoot.right.z); //�翷 ���⺤��
            //movdeDir�� forward�� ���⺤���̹Ƿ� XZ���� �����ؼ� ����� ���� �������� �Է°��� �����ֱ� ����.
            Vector3 moveDir = (forward * data.mvDir.z + right * data.mvDir.x).normalized;

            // ȸ�� �����ؼ� move �Լ��� ����
            netCharacterController.Move(
                (moveDir * moveSpeed * Runner.DeltaTime).normalized, //�̵����� * �ӵ� * ������
                data.aimDir * mouseSensitivity, //XZȸ�� ����
                cameraRoot // Y�� ȸ�� pitch �����
                );
            if (data.buttons.IsSet(NetworkInputData.BUTTON_JUMP))
                netCharacterController.Jump();
        }
    }
}

using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using UnityEngine;
using UnityEngine.Windows;

public class Player : NetworkBehaviour
{
    private readonly string MoveParamX = "MoveX";
    private readonly string MoveParamY = "MoveY";

    private float currentSpeedX = 0f;
    private float currentSpeedY = 0f;
    public float smoothTime = 0.1f; //õõ�� �ø���

    private NetworkCharacterController netCharacterController;
    private Animator animator;

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
        animator = GetComponentInChildren<Animator>();

    }

    public override void FixedUpdateNetwork()
    {
        MoveAndRotate();
    }



 
    private void MoveAndRotate() //�̵��� ȸ��
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

            //�ִϸ��̼� ����
            currentSpeedX = Mathf.Lerp(currentSpeedX, data.mvDir.x, Runner.DeltaTime / smoothTime);
            currentSpeedY = Mathf.Lerp(currentSpeedY, data.mvDir.z, Runner.DeltaTime / smoothTime);
            animator.SetFloat(MoveParamX, currentSpeedX);
            animator.SetFloat(MoveParamY, currentSpeedY); //�յ� �̵��� �Ķ���͸� �ٲ�����Ѵ�.
        }
    }
}

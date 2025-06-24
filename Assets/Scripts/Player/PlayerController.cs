using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun
{
    private Rigidbody playerRigidBody;
    private PlayerAction input;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float verticalSpeed = 5f;
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float maxSpeed = 10f;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private Transform Beam;

    private bool dashPressed = false;
    private Vector3 lastMoveDirection = Vector3.forward;

    private bool isBeamOn = false;

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        input = new PlayerAction();
    }

    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            //이동 활성화
            input.Enable();
        }
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            //메인 카메라 찾아주기
            if (mainCamera == null)
            {
                mainCamera = transform.Find("Main Camera").GetComponent<Camera>();
                mainCamera.gameObject.SetActive(true);
                mainCamera.GetComponent<PlayerCameraAim>()?.SetInput(input);
                mainCamera.GetComponent<PlayerCameraAim>().target = transform;
            }
        }
        //자식에 beam찾아서 넣어주기
        Beam = transform.Find("Beam");
    }

    private void OnDisable()
    {
        if (photonView.IsMine)
        {
            //이동 비활성화
            input.Disable();
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (input.PlayerActionMap.Dash.triggered)
        {
            dashPressed = true;
        }

        OnBeam();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        Move();

        if (dashPressed)
        {
            Dash();
            dashPressed = false;
        }
    }

    private void Move()
    {
        // 1. 입력 받기
        Vector2 moveInput = input.PlayerActionMap.Move.ReadValue<Vector2>();

        // 2. 카메라 기준 방향 설정
        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;

        // y축 방향 제거 (지면 기준 방향으로)
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // 3. 카메라 기준 이동 방향 계산
        Vector3 moveDirection = camForward * moveInput.y + camRight * moveInput.x;

        // 4. 수직 이동 처리 (Space / Ctrl)
        float y = 0f;
        if (Keyboard.current.spaceKey.isPressed) y += 1f;
        if (Keyboard.current.leftCtrlKey.isPressed) y -= 1f;

        // 5. 최종 이동 벡터
        Vector3 force = moveDirection.normalized * moveSpeed + Vector3.up * (y * verticalSpeed);

        // 6. 이동 적용
        playerRigidBody.AddForce(force, ForceMode.Force);
 
        // 7. 마지막 방향 저장
        if (force != Vector3.zero)
        {
            lastMoveDirection = force;
        }

        // 8. 최대 속도 제한
        if (playerRigidBody.velocity.magnitude > maxSpeed)
        {
            playerRigidBody.velocity = playerRigidBody.velocity.normalized * maxSpeed;
        }

        // 9. 이동 방향이 있을 때 회전
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp
                (transform.rotation, targetRotation, 10f * Time.fixedDeltaTime);
            //transform.rotation = targetRotation;
        }
    }

    private void Dash()
    {
        playerRigidBody.AddForce(lastMoveDirection.normalized
                                 * dashForce, ForceMode.Impulse);
    }


    private void OnBeam()
    {
        if (!photonView.IsMine) return;

        bool attackHeld = input.PlayerActionMap.Attack.ReadValue<float>() > 0.1f;

        if (attackHeld != isBeamOn)
        {
            isBeamOn = attackHeld;
            Beam.gameObject.SetActive(isBeamOn);
            photonView.RPC("SetBeamActive", RpcTarget.Others, isBeamOn);
        }
    }

    [PunRPC]
    private void SetBeamActive(bool isActive)
    {
        if (Beam == null)
        {
            Debug.LogWarning("Beam is not assigned when SetBeamActive is called.");
            return;
        }

        Beam.gameObject.SetActive(isActive);
    }
}